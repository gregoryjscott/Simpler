using Simpler.Attributes;

namespace Simpler.Tests.Mocks.Attributes
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
    }
}