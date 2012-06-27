using System;
using Simpler.Proxy;

namespace Simpler.Tests.Mocks
{
    public class MockFirstAttribute : EventsAttribute
    {
        public override void BeforeRun(Job job)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("First.Before");
        }

        public override void AfterRun(Job job)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("First.After");
        }

        public override void OnError(Job job, Exception exception)
        {
            ((MockJobWithAttributes)job).CallbackQueue.Enqueue("First.OnError");
        }
    }
}