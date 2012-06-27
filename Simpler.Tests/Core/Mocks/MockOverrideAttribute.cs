using System;
using System.Threading;
using Castle.DynamicProxy;

namespace Simpler.Tests.Core.Mocks
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