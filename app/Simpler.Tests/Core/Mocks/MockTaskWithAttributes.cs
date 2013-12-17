using System.Collections.Generic;

namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockTaskWithAttributes : T
    {
        public MockTaskWithAttributes()
        {
            CallbackQueue = new Queue<string>();
        }

        public Queue<string> CallbackQueue { get; set; }

        public override void Execute()
        {
            CallbackQueue.Enqueue("Execute");
        }
    }
}