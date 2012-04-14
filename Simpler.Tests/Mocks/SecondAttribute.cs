using System;
using Simpler.Construction;

namespace Simpler.Tests.Mocks
{
    public class SecondAttribute : ExecutionCallbacksAttribute
    {
        public override void BeforeExecute(Job jobBeingExecuted)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterExecute(Job jobBeingExecuted)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(Job jobBeingExecuted, Exception exception)
        {
            ((MockJobWithAttributes)jobBeingExecuted).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}