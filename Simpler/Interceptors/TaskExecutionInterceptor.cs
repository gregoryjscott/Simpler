using Castle.Core.Interceptor;
using Simpler.Tasks;

namespace Simpler.Interceptors
{
    public class TaskExecutionInterceptor<T> : IInterceptor where T : Task
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptExecution = new InterceptExecutionOf<T> { Invocation = invocation };
            interceptExecution.Execute();
        }
    }
}