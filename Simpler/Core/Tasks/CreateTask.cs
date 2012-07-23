using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Jobs
{
    public class CreateTask : Task
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type JobType { get; set; }
        public RunInterceptor RunInterceptor { get; set; }

        // Outputs
        public virtual object JobInstance { get; private set; }

        public FireEvents FireEvents { get; set; }

        public override void Run()
        {
            if (RunInterceptor == null)
            {
                RunInterceptor = new RunInterceptor(
                    invocation =>
                        {
                            if (FireEvents == null) FireEvents = new FireEvents();
                            FireEvents.Task = (Task)invocation.InvocationTarget;
                            FireEvents.Invocation = invocation;
                            FireEvents.Run();
                        });
            }

            JobInstance = ProxyGenerator.CreateClassProxy(JobType, RunInterceptor);
        }
    }
}
