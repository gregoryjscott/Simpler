using Castle.DynamicProxy;

namespace Simpler.Proxy.Jobs
{
    public class _InterceptRun : Job
    {
        // Inputs
        public virtual IInvocation Invocation { get; set; }
        public virtual _FireEvents FireEvents { get; set; }

        public override void Run()
        {
            if (Invocation.Method.Name.Equals("Run"))
            {
                if (FireEvents == null) FireEvents = new _FireEvents();
                FireEvents.Job = (Job)Invocation.InvocationTarget;
                FireEvents.Invocation = Invocation;
                FireEvents.Run();
            }
            else
            {
                Invocation.Proceed();
            }
        }
    }
}