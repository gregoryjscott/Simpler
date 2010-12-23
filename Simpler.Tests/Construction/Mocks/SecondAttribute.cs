using Simpler.Construction;

namespace Simpler.Tests.Construction.Mocks
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

        public override void OnError(Task taskBeingExecuted)
        {
            ((MockTaskWithAttributes)taskBeingExecuted).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}