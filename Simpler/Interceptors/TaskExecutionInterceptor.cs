using Castle.Core.Interceptor;
using Simpler.Tasks;

namespace Simpler.Interceptors
{
    public class TaskExecutionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptTaskExecution = new InterceptTaskExecution { Invocation = invocation };
            interceptTaskExecution.Execute();
        }
    }
}