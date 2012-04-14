using System;
using Simpler.Injection;

namespace Simpler.Tests.Mocks
{
    [InjectSubJobs]
    public class MockParentJob : Job
    {
        // Sub-jobs
        public MockSubJob<DateTime> MockSubClass { get; set; }

        // Outputs
        public bool SubJobWasInjected { get; private set; }

        public override void Execute()
        {
            SubJobWasInjected = (MockSubClass != null);
        }
    }
}
