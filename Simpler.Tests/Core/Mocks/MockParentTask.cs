using System;
using Simpler.Core;

namespace Simpler.Tests.Core.Mocks
{
    [InjectJobs]
    public class MockParentTask : Task
    {
        // Sub-jobs
        public MockSubTask<DateTime> MockSubClass { get; set; }

        // Outputs
        public bool SubJobWasInjected { get; private set; }

        public override void Run()
        {
            SubJobWasInjected = (MockSubClass != null);
        }
    }
}
