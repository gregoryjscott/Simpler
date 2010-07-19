using Castle.Core.Interceptor;

namespace Simpler.Tasks
{
    public class InterceptTaskExecution : Task
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }
        public virtual NotifySubscribersOfTaskExecution NotifySubscribersOfTaskExecution { get; set; }

        public override void Execute()
        {
            if (NotifySubscribersOfTaskExecution == null) NotifySubscribersOfTaskExecution = new NotifySubscribersOfTaskExecution();

            if (Invocation.Method.Name.Equals("Execute"))
            {
                NotifySubscribersOfTaskExecution.ExecutingTask = (Task)Invocation.InvocationTarget;
                NotifySubscribersOfTaskExecution.Invocation = Invocation;
                NotifySubscribersOfTaskExecution.Execute();
            }
            else
            {
                Invocation.Proceed();
            }
        }
    }
}