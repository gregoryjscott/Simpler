using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockParentSimpleTask : SimpleTask
    {
        public bool SubTaskWasInjected { get; private set; }

        public MockSubSimpleTask<DateTime> MockSubSimpleClass { get; set; }

        public override void Execute()
        {
            SubTaskWasInjected = (MockSubSimpleClass != null);
        }
    }
}
