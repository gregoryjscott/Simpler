using System;

namespace Simpler.Construction
{
    /// <summary>
    /// Base class for custom attributes that can be used to completely override the task
    /// execution.  It becomes the responsibility of the ExecutionOverride() to execute
    /// the task (if appropriate).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ExecutionOverrideAttribute : Attribute
    {
        public abstract void ExecutionOverride(Task task);
    }
}