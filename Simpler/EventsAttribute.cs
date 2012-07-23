using System;

namespace Simpler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class EventsAttribute : Attribute
    {
        public abstract void BeforeRun(Task task);
        public abstract void AfterRun(Task task);
        public abstract void OnError(Task task, Exception exception);
    }
}