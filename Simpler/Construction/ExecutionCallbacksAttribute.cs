using System;

namespace Simpler.Construction
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ExecutionCallbacksAttribute : Attribute
    {
        public abstract void BeforeExecute(Task taskBeingExecuted);
        public abstract void AfterExecute(Task taskBeingExecuted);
        public abstract void OnError(Task taskBeingExecuted, Exception exception);
    }
}