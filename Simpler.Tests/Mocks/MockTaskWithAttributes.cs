using System.Collections.Generic;

namespace Simpler.Tests.Mocks
{
    [First, Second]
    public class MockJobWithAttributes : Job
    {
        public MockJobWithAttributes()
        {
            CallbackQueue = new Queue<string>();
        }

        public virtual Queue<string> CallbackQueue { get; private set; }

        public override void Run()
        {
            CallbackQueue.Enqueue("Execute");
        }
    }
}