using Castle.DynamicProxy;
using Simpler.Construction.Jobs;

namespace Simpler.Construction.Interceptors
{
    /// <summary>
    /// Implementation of IInterceptor that is used to intercept a job's Execute() method.  This class
    /// delegates all the work to the InterceptJobExecution job.
    /// </summary>
    public class JobExecutionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptJobExecution = new InterceptJobExecution { Invocation = invocation };
            interceptJobExecution.Execute();
        }
    }
}