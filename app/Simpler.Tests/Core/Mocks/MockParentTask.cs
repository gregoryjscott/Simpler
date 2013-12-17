using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockParentTask : Task
    {
        public bool SubtaskWasExecuted { get; set; }

        public MockSubTask<DateTime> MockSubClass { get; set; }

        public override void Execute()
        {
            MockSubClass.Execute();
            SubtaskWasExecuted = MockSubClass.WasExecuted;
        }
    }
}
