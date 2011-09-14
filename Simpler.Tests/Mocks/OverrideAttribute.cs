using System;
using System.Threading;
using Castle.DynamicProxy;
using Simpler.Construction;

namespace Simpler.Tests.Mocks
{
    public class OverrideAttribute : ExecutionOverrideAttribute
    {
        public override void ExecutionOverride(IInvocation executeInvocation)
        {
            ((MockTaskWithOverrideAttribute)executeInvocation.InvocationTarget).OverrideWasCalledTime = DateTime.Now;
            Thread.Sleep(100);
            executeInvocation.Proceed();
        }
    }
}