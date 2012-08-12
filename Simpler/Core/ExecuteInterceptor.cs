using System;
using Castle.DynamicProxy;

namespace Simpler.Core
{
    public class ExecuteInterceptor : IInterceptor
    {
        public ExecuteInterceptor(Action<IInvocation> action)
        {
            _action = action;
        }

        Action<IInvocation> _action;

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.Equals("Execute"))
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