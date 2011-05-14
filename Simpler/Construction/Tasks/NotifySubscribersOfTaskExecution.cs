using System;
using Castle.DynamicProxy;

namespace Simpler.Construction.Tasks
{
    /// <summary>
    /// Task that notifies all subscibers to task execution events.  Subscriptions are made by decorating the
    /// task with an attibute that is a subclass of ExecutionCallbacksAttribute.
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

            // Send BeforeExecute notifications.
            foreach (var t in callbackAttributes)
            {
                ((ExecutionCallbacksAttribute) t).BeforeExecute(ExecutingTask);
            }

            // Execute, and temporarily catch any exceptions so notifications can be sent.
            try
            {
                Invocation.Proceed();
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