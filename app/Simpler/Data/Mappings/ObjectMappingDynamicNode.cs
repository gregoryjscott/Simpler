using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public class ObjectMappingDynamicNode : ObjectMappingObjectNode
    {
        public override object CreateInstance(object value = null)
        {
            return new ExpandoObject();
        }
    }
}
