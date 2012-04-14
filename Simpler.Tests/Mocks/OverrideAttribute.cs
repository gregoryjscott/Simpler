using System;
using System.Threading;
using Castle.DynamicProxy;

namespace Simpler.Tests.Mocks
{
    public class OverrideAttribute : Proxy.OverrideAttribute
    {
        public override void RunOverride(IInvocation run)
        {
            ((MockJobWithOverrideAttribute)run.InvocationTarget).OverrideWasCalledTime = DateTime.Now;
            Thread.Sleep(100);
            run.Proceed();
        }
    }
}