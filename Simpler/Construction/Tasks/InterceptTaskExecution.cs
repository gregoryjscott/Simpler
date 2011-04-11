using Castle.DynamicProxy;

namespace Simpler.Construction.Tasks
{
    /// <summary>
    /// Task that is used to intercept the Execute() method on a task and then notify subscribers of the 
    /// task execution events.  This task is called every time a method is called on a task (it will almost
    /// always only have Execute() method anyway).
    /// </summary>
    public class InterceptTaskExecution : Task
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }
        public virtual NotifySubscribersOfTaskExecution NotifySubscribersOfTaskExecution { get; set; }

        public override void Execute()
        {
            if (Invocation.Method.Name.Equals("Execute"))
            {
                if (NotifySubscribersOfTaskExecution == null) NotifySubscribersOfTaskExecution = new NotifySubscribersOfTaskExecution();
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