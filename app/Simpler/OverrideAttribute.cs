using System;

namespace Simpler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class OverrideAttribute : Attribute
    {
        //public abstract void ExecuteOverride(IInvocation execute);
    }
}