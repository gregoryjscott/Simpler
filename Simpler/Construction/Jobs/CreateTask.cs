using Castle.DynamicProxy;
using System;
using Simpler.Construction.Interceptors;

namespace Simpler.Construction.Jobs
{
    /// <summary>
    /// Job used to instantiate jobs.  If the given job type is decorated with ExecutionCallbacksAttribute
    /// then a proxy class is created so that job execution events can be fired.
    /// </summary>
    public class CreateJob : Job
    {
        /// <summary>
        /// This class provided by Castle that allows for creating proxy classes.  Creating it is expensive - we
        /// only want to do it once.
        /// </summary>
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type JobType { get; set; }

        // Outputs
        public virtual object JobInstance { get; private set; }

        public override void Execute()
        {
            JobInstance =
                Attribute.IsDefined(JobType, typeof (ExecutionCallbacksAttribute))
                    ? ProxyGenerator.CreateClassProxy(JobType, new JobExecutionInterceptor())
                    : Activator.CreateInstance(JobType);
        }
    }
}
