using System;

namespace Simpler.Tests.Construction.Mocks
{
    [Override]
    public class MockTaskWithOverrideAttribute : Task
    {
        public DateTime OverrideWasCalledTime;
        public DateTime WasExecutedTime;

        public bool OverrideWasCalledBeforeTheTaskWasExecuted { get { return OverrideWasCalledTime < WasExecutedTime; } }

        public override void Execute()
        {
            WasExecutedTime = DateTime.Now;
        }
    }
}