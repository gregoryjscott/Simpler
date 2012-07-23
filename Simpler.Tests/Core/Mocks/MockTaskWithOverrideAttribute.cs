using System;

namespace Simpler.Tests.Core.Mocks
{
    [MockOverride]
    public class MockTaskWithOverrideAttribute : Task
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