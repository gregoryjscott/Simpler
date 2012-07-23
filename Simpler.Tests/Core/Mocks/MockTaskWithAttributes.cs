using System.Collections.Generic;

namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockTaskWithAttributes : Task
    {
        public MockTaskWithAttributes()
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