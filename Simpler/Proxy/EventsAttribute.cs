using System;

namespace Simpler.Proxy
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class EventsAttribute : Attribute
    {
        public abstract void BeforeRun(Job job);
        public abstract void AfterRun(Job job);
        public abstract void OnError(Job job, Exception exception);
    }
}