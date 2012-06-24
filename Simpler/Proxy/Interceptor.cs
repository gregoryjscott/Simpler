using Castle.DynamicProxy;
using Simpler.Proxy.Jobs;

namespace Simpler.Proxy
{
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptRun = new InterceptRun { Invocation = invocation };
            interceptRun.Run();
        }
    }
}