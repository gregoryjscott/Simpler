using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public class ObjectMappingDynamicChildNode : ObjectMappingNode
    {
        public override object CreateInstance(object value = null)
        {
            return value;
        }

        public override void SetValue(dynamic instance, object value)
        {
            var instanceValue = CreateInstance(value);
            ((ExpandoObject)instance as IDictionary<String, object>)[Name] = instanceValue;
        }

        public override object GetValue(dynamic instance)
        {
            return ((ExpandoObject)instance as IDictionary<String, object>)[Name];
        }
    }
}
