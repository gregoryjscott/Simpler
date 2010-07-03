using System;
using Castle.Core.Interceptor;

namespace Simpler.Tasks
{
    public class InterceptExecutionOf<T> : Task where T : Task
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }
        public virtual NotifySubscribersToExecutionOf<T> NotifySubscribersToExecution { get; set; }

        public override void Execute()
        {
            if (NotifySubscribersToExecution == null)
            {
                NotifySubscribersToExecution = new NotifySubscribersToExecutionOf<T>();
            }

            if (Invocation.Method.Name.Equals("Execute"))
            {
                var task = Invocation.InvocationTarget as T;
                if (task == null) throw new Exception("Invocation target is not the expected type of Task.");

                NotifySubscribersToExecution.ExecutingTask = task;
                NotifySubscribersToExecution.Execute();
            }
            else
            {
                Invocation.Proceed();
            }
        }
    }
}