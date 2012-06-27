using Castle.DynamicProxy;
using System;

namespace Simpler.Proxy.Jobs
{
    public class CreateJob : Job
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type JobType { get; set; }
        public RunInterceptor RunInterceptor { get; set; }

        // Outputs
        public virtual object JobInstance { get; private set; }

        public override void Run()
        {
            if (RunInterceptor == null)
            {
                RunInterceptor = new RunInterceptor(
                    invocation =>
                        {
                            var interceptRun = new InterceptRun
                                                   {
                                                       Invocation = invocation
                                                   };
                            interceptRun.Run();
                        });
            }

            JobInstance = ProxyGenerator.CreateClassProxy(JobType, RunInterceptor);
        }
    }
}
