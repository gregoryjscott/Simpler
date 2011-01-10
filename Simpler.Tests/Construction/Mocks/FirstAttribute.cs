using System;
using Simpler.Construction;

namespace Simpler.Tests.Construction.Mocks
{
    public class FirstAttribute : ExecutionCallbacksAttribute
    {
        public override void BeforeExecute(Task taskBeingExecuted)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(Task taskBeingExecuted, Exception exception)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("First.OnError");
        }
    }
}