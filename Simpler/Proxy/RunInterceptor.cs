using System;
using Castle.DynamicProxy;

namespace Simpler.Proxy
{
    public class RunInterceptor : IInterceptor
    {
        public RunInterceptor(Action<IInvocation> action)
        {
            _action = action;
        }

        Action<IInvocation> _action;

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.Equals("Run"))
            {
                _action(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}