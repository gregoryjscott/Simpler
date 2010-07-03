using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTask.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SubTaskInjectionAttribute : Attribute
    {
        public bool Enabled { get; set; }
    }
}
