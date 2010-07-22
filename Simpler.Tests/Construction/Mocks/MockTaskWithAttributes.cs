using System.Collections.Generic;

namespace Simpler.Tests.Construction.Mocks
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