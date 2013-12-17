using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockParentTask : T
    {
        public bool SubTaskWasInjected { get; private set; }

        public MockSubTask<DateTime> MockSubClass { get; set; }

        public override void Execute()
        {
            SubTaskWasInjected = (MockSubClass != null);
        }
    }
}
