using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSecondAttribute : EventsAttribute
    {
        public override void BeforeExecute(SimpleTask simpleTask)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterExecute(SimpleTask simpleTask)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(SimpleTask simpleTask, Exception exception)
        {
            ((MockSimpleTaskWithAttributes)simpleTask).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}