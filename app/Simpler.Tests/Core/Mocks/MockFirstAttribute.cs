using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockFirstAttribute : EventsAttribute
    {
        public override void BeforeExecute(SimpleTask simpleTask)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterExecute(SimpleTask simpleTask)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(SimpleTask simpleTask, Exception exception)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("First.OnError");
        }
    }
}