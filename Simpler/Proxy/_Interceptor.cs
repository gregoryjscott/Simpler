using Castle.DynamicProxy;
using Simpler.Proxy.Jobs;

namespace Simpler.Proxy
{
    public class _Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptRun = new _InterceptRun { Invocation = invocation };
            interceptRun.Run();
        }
    }
}