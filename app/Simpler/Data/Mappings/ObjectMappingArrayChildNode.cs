using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public class ObjectMappingArrayChildNode : ObjectMappingNode
    {
        public override object CreateInstance(object value = null)
        {
            if (value == null)
            {
                return Activator.CreateInstance(PropertyType);
            }

            //I think this can be moved into a constructor
            var propertyType = PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }
            //end of thought

            if (propertyType.IsEnum)
            {
                value = Enum.Parse(propertyType, value.ToString());
            }

            return Convert.ChangeType(value, propertyType);
        }

        public override void SetValue(object instance, object value)
        {
            //this int parse can be moved into a constructor
            ((Array)instance).SetValue(CreateInstance(value), int.Parse(Name));
        }

        public override object GetValue(object instance)
        {
            return ((Array) instance).GetValue(int.Parse(Name));
        }
    }
}
