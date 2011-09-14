using System;
using Castle.DynamicProxy;

namespace Simpler.Construction.Tasks
{
    /// <summary>
    /// Task that notifies all subscribers to task execution.  Subscriptions are made by decorating the
    /// task with an attibute that is a subclass of ExecutionCallbacksAttribute or ExecutionOverrideAttribute.
    /// </summary>
    public class NotifySubscribersOfTaskExecution : Task
    {
        // Inputs
        public virtual Task ExecutingTask { get; set; }
        public virtual IInvocation Invocation { get; set; }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(ExecutingTask.GetType(),
                                                                   typeof (ExecutionCallbacksAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(ExecutingTask.GetType(), typeof(ExecutionOverrideAttribute));

            // Send BeforeExecute notifications.
            foreach (var t in callbackAttributes)
            {
                ((ExecutionCallbacksAttribute) t).BeforeExecute(ExecutingTask);
            }

            // Execute, and temporarily catch any exceptions so notifications can be sent.
            try
            {
                // If the task execution has been overridden, simply the pass on the task, otherwise proceed with the invocation.
                if (overrideAttribute != null)
                {
                    ((ExecutionOverrideAttribute)overrideAttribute).ExecutionOverride(Invocation);
                }
                else
                {
                    Invocation.Proceed();
                }
            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((ExecutionCallbacksAttribute) callbackAttributes[i]).OnError(ExecutingTask, exception);
                }

                throw;
            }
            finally
            {
                // Send the AfterExecute notifications.
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((ExecutionCallbacksAttribute) callbackAttributes[i]).AfterExecute(ExecutingTask);
                }
            }
        }
    }
}