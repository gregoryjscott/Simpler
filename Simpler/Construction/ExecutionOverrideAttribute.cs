using System;
using Castle.DynamicProxy;

namespace Simpler.Construction
{
    /// <summary>
    /// Base class for custom attributes that can be used to completely override the task
    /// execution.  It becomes the responsibility of the ExecutionOverride() to execute
    /// the task by calling Proceed() on the given execution invocation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ExecutionOverrideAttribute : Attribute
    {
        public abstract void ExecutionOverride(IInvocation executeInvocation);
    }
}