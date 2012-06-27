using Castle.DynamicProxy;

namespace Simpler.Proxy.Jobs
{
    public class InterceptRun : Job
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }

        // Sub-jobs
        public virtual FireEvents FireEvents { get; set; }

        public override void Run()
        {
            if (FireEvents == null) FireEvents = new FireEvents();
            FireEvents.Job = (Job) Invocation.InvocationTarget;
            FireEvents.Invocation = Invocation;
            FireEvents.Run();
        }
    }
}