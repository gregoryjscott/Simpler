using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockFirstAttribute : EventsAttribute
    {
        public override void BeforeExecute(T task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterExecute(T task)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(T task, Exception exception)
        {
            ((MockTaskWithAttributes)task).CallbackQueue.Enqueue("First.OnError");
        }
    }
}