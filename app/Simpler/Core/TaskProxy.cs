using System;
using System.Collections.Generic;
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
            var methodCallMessage = (IMethodCallMessage)message;
            var methodInfo = (MethodInfo)methodCallMessage.MethodBase;

            return methodInfo.Name == "Execute"
                ? InvokeExecute(methodInfo, methodCallMessage, _task, _executeOverride)
                : InvokeOther(methodInfo, methodCallMessage, _task);
        }

        #region Helpers

        static IMessage InvokeMethod(MethodInfo methodInfo, IMethodCallMessage methodCallMessage, Task task, Action<Task> methodOverride = null)
        {
            if (methodOverride != null)
            {
                methodOverride(task);
                return new ReturnMessage(null, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
            }

            var result = methodInfo.Invoke(task, methodCallMessage.InArgs);
            return new ReturnMessage(result, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
        }

        static IMessage InvokeExecute(MethodInfo methodInfo, IMethodCallMessage methodCallMessage, Task task, Action<Task> methodOverride = null)
        {
            var startTime = DateTime.Now;

            var overrideAttribute = Attribute.GetCustomAttribute(task.GetType(), typeof(OverrideAttribute));
            if (overrideAttribute != null && methodOverride == null)
            {
                methodOverride = t => ((OverrideAttribute)overrideAttribute).ExecuteOverride(t);
            }

            var eventAttributes = Attribute.GetCustomAttributes(task.GetType(), typeof(EventsAttribute));
            try
            {
                NotifyBefore(eventAttributes, task);
                return InvokeMethod(methodInfo, methodCallMessage, task, methodOverride);
            }
            catch (Exception e)
            {
                NotifyError(eventAttributes, task, e);
                return HandleException(e, methodCallMessage);
            }
            finally
            {
                NotifyAfter(eventAttributes, task);
                RecordStats(startTime, task);
            }
        }

        static IMessage InvokeOther(MethodInfo methodInfo, IMethodCallMessage methodCallMessage, Task task)
        {
            try
            {
                return InvokeMethod(methodInfo, methodCallMessage, task);
            }
            catch (Exception e)
            {
                return HandleException(e, methodCallMessage);
            }
        }

        static IMessage HandleException(Exception e, IMethodCallMessage methodCallMessage)
        {
            if (e is TargetInvocationException && e.InnerException != null)
            {
                return new ReturnMessage(e.InnerException, methodCallMessage);
            }

            return new ReturnMessage(e, methodCallMessage as IMethodCallMessage);
        }

        static void NotifyBefore(Attribute[] eventAttributes, Task task)
        {
            foreach (var attribute in eventAttributes)
            {
                ((EventsAttribute)attribute).BeforeExecute(task);
            }
        }

        static void NotifyError(Attribute[] eventAttributes, Task task, Exception e)
        {
            for (var i = eventAttributes.Length - 1; i >= 0; i--)
            {
                ((EventsAttribute)eventAttributes[i]).OnError(task, e);
            }
        }

        static void NotifyAfter(Attribute[] eventAttributes, Task task)
        {
            for (var i = eventAttributes.Length - 1; i >= 0; i--)
            {
                ((EventsAttribute)eventAttributes[i]).AfterExecute(task);
            }
        }

        static void RecordStats(DateTime startTime, Task task)
        {
            var endTime = DateTime.Now;
            var duration = endTime - startTime;
            task.Stats.ExecuteDurations.Add(duration);
        }

        #endregion
    }
}