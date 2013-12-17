using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSecondAttribute : EventsAttribute
    {
        public override void BeforeExecute(T task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterExecute(T task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(T task, Exception exception)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}