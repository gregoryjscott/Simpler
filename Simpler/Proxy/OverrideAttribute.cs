using System;
using Castle.DynamicProxy;

namespace Simpler.Proxy
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class OverrideAttribute : Attribute
    {
        public abstract void RunOverride(IInvocation run);
    }
}