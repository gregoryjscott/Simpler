using System;
using Simpler.Core;

namespace Simpler.Tests.Core.Mocks
{
    [InjectTasks]
    public class MockParentTask : Task
    {
        // Sub-jobs
        public MockSubTask<DateTime> MockSubClass { get; set; }

        // Outputs
        public bool SubTaskWasInjected { get; private set; }

        public override void Run()
        {
            SubTaskWasInjected = (MockSubClass != null);
        }
    }
}
