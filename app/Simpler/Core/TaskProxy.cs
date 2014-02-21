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

                    return Invoke(method, methodCall, _executeOverride);
                }
                catch (Exception e)
                {
                    for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                    {
                        ((EventsAttribute)callbackAttributes[i]).OnError(_task, e);
                    }

                    return ReturnMessageForException(e, message);
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
                return Invoke(method, methodCall);
            }
            catch (Exception e)
            {
                return ReturnMessageForException(e, message);
            }
        }

        #region Helpers

        IMessage Invoke(MethodInfo method, IMethodCallMessage methodCall, Action<Task> executeOverride = null)
        {
            if (executeOverride != null)
            {
                executeOverride(_task);
                return new ReturnMessage(null, null, 0, methodCall.LogicalCallContext, methodCall);
            }

            var result = method.Invoke(_task, methodCall.InArgs);
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        }

        static IMessage ReturnMessageForException(Exception e, IMessage messageReceived)
        {
            if (e is TargetInvocationException && e.InnerException != null)
            {
                return new ReturnMessage(e.InnerException, messageReceived as IMethodCallMessage);
            }

            return new ReturnMessage(e, messageReceived as IMethodCallMessage);
        }

        #endregion

    }
}