using System;

namespace Simpler.Construction
{
    /// <summary>
    /// Base class for custom attributes that can be used to respond to a job execution
    /// events.  These can be used to address cross-cutting concerns (e.g. logging).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ExecutionCallbacksAttribute : Attribute
    {
        public abstract void BeforeExecute(Job jobBeingExecuted);
        public abstract void AfterExecute(Job jobBeingExecuted);
        public abstract void OnError(Job jobBeingExecuted, Exception exception);
    }
}