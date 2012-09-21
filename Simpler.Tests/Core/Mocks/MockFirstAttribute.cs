using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockFirstAttribute : EventsAttribute
    {
        public override void BeforeExecute(Task task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterExecute(Task task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(Task task, Exception exception)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.OnError");
        }
    }
}