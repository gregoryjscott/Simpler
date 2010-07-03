using System.Collections.Generic;
using Simpler.Tests.Mocks.Attributes;

namespace Simpler.Tests.Mocks
{
    [First, Second]
    public class MockTaskWithAttributes : Task
    {
        public MockTaskWithAttributes()
        {
            CallbackQueue = new Queue<string>();
        }

        public virtual Queue<string> CallbackQueue { get; private set; }

        public override void Execute()
        {
            CallbackQueue.Enqueue("Execute");
        }
    }
}