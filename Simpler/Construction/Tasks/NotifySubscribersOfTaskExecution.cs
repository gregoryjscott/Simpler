using System;
using Castle.DynamicProxy;

namespace Simpler.Construction.Tasks
{
    public class NotifySubscribersOfTaskExecution : Task
    {
        // Inputs
        public virtual Task ExecutingTask { get; set; }
        public virtual IInvocation Invocation { get; set; }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(ExecutingTask.GetType(), typeof(ExecutionCallbacksAttribute));

            for (var i = 0; i < callbackAttributes.Length; i++)
            {
                ((ExecutionCallbacksAttribute)callbackAttributes[i]).BeforeExecute(ExecutingTask);
            }

            try
            {
                Invocation.Proceed();
            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((ExecutionCallbacksAttribute)callbackAttributes[i]).OnError(ExecutingTask, exception);
                }

                throw;
            }

            for (var i = callbackAttributes.Length - 1; i >= 0; i--)
            {
                ((ExecutionCallbacksAttribute)callbackAttributes[i]).AfterExecute(ExecutingTask);
            }
        }
    }
}