using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Simpler.Core
{
    public class TaskProxy : RealProxy
    {
        readonly Task _task;
        readonly Action<Task> _executeOverride;

        public TaskProxy(Type taskType, object task, Action<Task> executeOverride)
            : base(taskType)
        {
            _task = (Task)task;
            _executeOverride = executeOverride;
        }

        public override IMessage Invoke(IMessage message)
        {
            var methodCall = (IMethodCallMessage)message;
            var method = (MethodInfo)methodCall.MethodBase;

            if (method.Name == "Execute")
            {
                var callbackAttributes = Attribute.GetCustomAttributes(_task.GetType(), typeof(EventsAttribute));

                var beforeTime = DateTime.Now;
                try
                {
                    foreach (var callbackAttribute in callbackAttributes)
                    {
                        ((EventsAttribute)callbackAttribute).BeforeExecute(_task);
                    }

                    if (_executeOverride != null)
                    {
                        _executeOverride(_task);
                        return new ReturnMessage(null, null, 0, methodCall.LogicalCallContext, methodCall);
                    }
                    else
                    {
                        var result = method.Invoke(_task, methodCall.InArgs);
                        return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                    }
                }
                catch (Exception e)
                {
                    for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                    {
                        ((EventsAttribute)callbackAttributes[i]).OnError(_task, e);
                    }

                    if (e is TargetInvocationException && e.InnerException != null)
                    {
                        return new ReturnMessage(e.InnerException, message as IMethodCallMessage);
                    }

                    return new ReturnMessage(e, message as IMethodCallMessage);
                }
                finally
                {
                    for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                    {
                        ((EventsAttribute)callbackAttributes[i]).AfterExecute(_task);
                    }

                    var afterTime = DateTime.Now;
                    var duration = afterTime - beforeTime;
                    _task.Stats.ExecuteDurations.Add(duration);
                }
            }

            try
            {
                var result = method.Invoke(_task, methodCall.InArgs);
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    return new ReturnMessage(e.InnerException, message as IMethodCallMessage);
                }

                return new ReturnMessage(e, message as IMethodCallMessage);
            }
        }
    }
}