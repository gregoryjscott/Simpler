using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public class ObjectMappingObjectNode : ObjectMappingNode
    {
        public PropertyInfo PropertyInfo { get; set; }

        public override object CreateInstance(object value = null)
        {
            var propertyType = PropertyType;
            if (value == null && propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null;
            }

            if (value == null)
            {
                return Activator.CreateInstance(PropertyType, null);
            }

            //I think this can be moved into a constructor
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
            var instanceValue = CreateInstance(value);
            PropertyInfo.SetValue(instance, instanceValue, null);
        }

        public override object GetValue(object instance)
        {
            return PropertyInfo.GetValue(instance, null);
        }
    }
}
