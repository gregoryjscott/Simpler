using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSecondAttribute : EventsAttribute
    {
        public override void BeforeExecute(Task task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterExecute(Task task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(Task task, Exception exception)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}