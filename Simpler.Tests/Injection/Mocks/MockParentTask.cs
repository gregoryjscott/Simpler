using System;
using Simpler.Attributes;

namespace Simpler.Tests.Injection.Mocks
{
    [InjectSubTasks]
    public class MockParentTask : Task
    {
        // Sub-tasks
        public MockSubTask<DateTime> MockSubClass { get; set; }

        // Outputs
        public bool SubTaskWasInjected { get; private set; }

        public override void Execute()
        {
            SubTaskWasInjected = true;
        }
    }
}
