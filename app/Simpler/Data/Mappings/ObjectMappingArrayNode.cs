using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public class ObjectMappingArrayNode : ObjectMappingObjectNode
    {
        public override object CreateInstance(object value = null)
        {
            return Activator.CreateInstance(PropertyType, new object[] { Children.Count });
        }
    }
}
