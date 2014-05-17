using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockParentTask : Task
    {
        public bool SubTaskWasExecuted { get; set; }

        public MockSubTask<DateTime> MockSubTask { get; set; }

        public override void Execute()
        {
            MockSubTask.Execute();
            SubTaskWasExecuted = MockSubTask.WasExecuted;
        }
    }
}
