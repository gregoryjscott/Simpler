using Castle.DynamicProxy;
using Simpler.Construction.Tasks;

namespace Simpler.Construction.Interceptors
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