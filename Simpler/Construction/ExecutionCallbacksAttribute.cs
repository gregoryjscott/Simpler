using System;

namespace Simpler.Construction
{
    /// <summary>
    /// Base class for custom attributes that can be used to respond to a task execution
    /// events.  These can be used to address cross-cutting concerns (e.g. logging).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ExecutionCallbacksAttribute : Attribute
    {
        public abstract void BeforeExecute(Task taskBeingExecuted);
        public abstract void AfterExecute(Task taskBeingExecuted);
        public abstract void OnError(Task taskBeingExecuted, Exception exception);
    }
}