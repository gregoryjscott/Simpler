using System;

namespace Simpler.Tests.Core.Mocks
{
    [MockOverride]
    public class MockTaskWithOverrideAttribute : Task
    {
        public bool WasExecuted { get; set; }
        public bool OverrideShouldProceed { get; set; }

        public override void Execute()
        {
            WasExecuted = true;
        }
    }
}