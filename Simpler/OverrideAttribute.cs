using System;
using Castle.DynamicProxy;

namespace Simpler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class OverrideAttribute : Attribute
    {
        public abstract void RunOverride(IInvocation run);
    }
}