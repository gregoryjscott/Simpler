using System;

namespace Simpler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class EventsAttribute : Attribute
    {
        public abstract void BeforeExecute(T task);
        public abstract void AfterExecute(T task);
        public abstract void OnError(T task, Exception exception);
    }
}