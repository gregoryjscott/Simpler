using System;

namespace Simpler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class EventsAttribute : Attribute
    {
        public abstract void BeforeExecute(SimpleTask simpleTask);
        public abstract void AfterExecute(SimpleTask simpleTask);
        public abstract void OnError(SimpleTask simpleTask, Exception exception);
    }
}