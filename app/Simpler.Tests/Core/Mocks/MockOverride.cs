using System;
using System.Threading;
using Castle.DynamicProxy;

namespace Simpler.Tests.Core.Mocks
{
    public class MockOverrideAttribute : OverrideAttribute
    {
        //public override void ExecuteOverride(IInvocation execute)
        //{
        //    ((MockTaskWithOverrideAttribute)execute.InvocationTarget).OverrideWasCalledTime = DateTime.Now;
        //    Thread.Sleep(100);
        //    execute.Proceed();
        //}
    }
}