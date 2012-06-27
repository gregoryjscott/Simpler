using System;
using Simpler.Proxy;

namespace Simpler.Tests.Mocks
{
    public class MockSecondAttribute : EventsAttribute
    {
        public override void BeforeRun(Job job)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("Second.Before");
        }

        public override void AfterRun(Job job)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("Second.After");
        }

        public override void OnError(Job job, Exception exception)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("Second.OnError");
        }
    }
}