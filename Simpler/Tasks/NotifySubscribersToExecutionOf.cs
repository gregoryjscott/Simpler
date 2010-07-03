using System;
using Simpler.Attributes;

namespace Simpler.Tasks
{
    public class NotifySubscribersToExecutionOf<T> : Task where T : Task
    {
        // Inputs
        public virtual T ExecutingTask { get; set; }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(typeof(T), typeof(ExecutionCallbacksAttribute));

            for (var i = 0; i < callbackAttributes.Length; i++)
            {
                ((ExecutionCallbacksAttribute)callbackAttributes[i]).BeforeExecute(ExecutingTask);
            }

            ExecutingTask.Execute();

            for (var i = callbackAttributes.Length - 1; i >= 0; i--)
            {
                ((ExecutionCallbacksAttribute)callbackAttributes[i]).AfterExecute(ExecutingTask);
            }
        }
    }
}