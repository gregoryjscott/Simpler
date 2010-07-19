using System;
using Castle.Core.Interceptor;
using Simpler.Attributes;

namespace Simpler.Tasks
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

            Invocation.Proceed();

            for (var i = callbackAttributes.Length - 1; i >= 0; i--)
            {
                ((ExecutionCallbacksAttribute)callbackAttributes[i]).AfterExecute(ExecutingTask);
            }
        }
    }
}