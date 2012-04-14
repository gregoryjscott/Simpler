using System;
using Simpler.Construction;

namespace Simpler.Tests.Mocks
{
    public class FirstAttribute : ExecutionCallbacksAttribute
    {
        public override void BeforeExecute(Job jobBeingExecuted)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterExecute(Job jobBeingExecuted)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(Job jobBeingExecuted, Exception exception)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("First.OnError");
        }
    }
}