using System;
using Simpler.Proxy;

namespace Simpler.Tests.Mocks
{
    [_InjectJobs]
    public class MockParentJob : Job
    {
        // Sub-jobs
        public MockSubJob<DateTime> MockSubClass { get; set; }

        // Outputs
        public bool SubJobWasInjected { get; private set; }

        public override void Run()
        {
            SubJobWasInjected = (MockSubClass != null);
        }
    }
}
