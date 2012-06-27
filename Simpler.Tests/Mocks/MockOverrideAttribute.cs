using System;
using System.Threading;
using Castle.DynamicProxy;
using Simpler.Proxy;

namespace Simpler.Tests.Mocks
{
    public class MockOverrideAttribute : OverrideAttribute
    {
        public override void RunOverride(IInvocation run)
        {
            ((MockJobWithOverrideAttribute)run.InvocationTarget).OverrideWasCalledTime = DateTime.Now;
            Thread.Sleep(100);
            run.Proceed();
        }
    }
}