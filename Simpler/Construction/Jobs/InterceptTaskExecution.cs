using Castle.DynamicProxy;

namespace Simpler.Construction.Jobs
{
    /// <summary>
    /// Job that is used to intercept the Execute() method on a job and then notify subscribers of the 
    /// job execution events.  This job is called every time a method is called on a job (it will almost
    /// always only have Execute() method anyway).
    /// </summary>
    public class InterceptJobExecution : Job
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }
        public virtual NotifySubscribersOfJobExecution NotifySubscribersOfJobExecution { get; set; }

        public override void Execute()
        {
            if (Invocation.Method.Name.Equals("Execute"))
            {
                if (NotifySubscribersOfJobExecution == null) NotifySubscribersOfJobExecution = new NotifySubscribersOfJobExecution();
                NotifySubscribersOfJobExecution.ExecutingJob = (Job)Invocation.InvocationTarget;
                NotifySubscribersOfJobExecution.Invocation = Invocation;
                NotifySubscribersOfJobExecution.Execute();
            }
            else
            {
                Invocation.Proceed();
            }
        }
    }
}