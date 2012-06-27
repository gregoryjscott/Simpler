using System;

namespace Simpler.Tests.Core.Mocks
{
    [MockOverride]
    public class MockJobWithOverrideAttribute : Job
    {
        public DateTime OverrideWasCalledTime;
        public DateTime WasExecutedTime;

        public bool OverrideWasCalledBeforeTheJobWasExecuted { get { return OverrideWasCalledTime < WasExecutedTime; } }

        public override void Run()
        {
            WasExecutedTime = DateTime.Now;
        }
    }
}