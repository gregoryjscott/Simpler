using Castle.DynamicProxy;
using System;

namespace Simpler.Proxy.Jobs
{
    public class CreateJob : Job
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type JobType { get; set; }

        // Outputs
        public virtual object JobInstance { get; private set; }

        public override void Run()
        {
            JobInstance = ProxyGenerator.CreateClassProxy(JobType, new Interceptor());
        }
    }
}
