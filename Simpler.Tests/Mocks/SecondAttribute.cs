using System;
using Simpler.Construction;

namespace Simpler.Tests.Mocks
{
    public class SecondAttribute : ExecutionCallbacksAttribute
    {
        public override void BeforeExecute(Task taskBeingExecuted)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(Task taskBeingExecuted, Exception exception)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}