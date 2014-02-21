using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public abstract class ObjectMappingNode : ObjectMapping
    {
        public string Name { get; set; }
        public int? ColumnIndex { get; set; }

        public abstract object CreateInstance(object value = null);
        public abstract void SetValue(object instance, object value);
        public abstract object GetValue(object instance);
    }
}
