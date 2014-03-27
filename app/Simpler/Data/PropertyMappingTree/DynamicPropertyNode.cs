using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Simpler.Data.PropertyMappingTree
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicPropertyNode : AbstractNode
    {
        public override object CreateObject(object value = null)
        {
            return value;
        }

        public override void SetValue(dynamic instance, object value)
        {
            var instanceValue = CreateObject(value);
            ((ExpandoObject)instance as IDictionary<String, object>)[Name] = instanceValue;
        }

        public override object GetValue(dynamic instance)
        {
            return ((ExpandoObject)instance as IDictionary<String, object>)[Name];
        }
    }
}
