using Castle.DynamicProxy;
using Simpler.Construction.Tasks;

namespace Simpler.Construction.Interceptors
{
    /// <summary>
    /// Implementation of IInterceptor that is used to intercept a task's Execute() method.  This class
    /// delegates all the work to the InterceptTaskExecution task.
    /// </summary>
    public class TaskExecutionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptTaskExecution = new InterceptTaskExecution { Invocation = invocation };
            interceptTaskExecution.Execute();
        }
    }
}