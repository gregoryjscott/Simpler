using System;
using System.Reflection;

namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyParseTreeObjectNode : PropertyParseTreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        public override object CreateObject(object value = null)
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
            var instanceValue = CreateObject(value);
            PropertyInfo.SetValue(instance, instanceValue, null);
        }

        public override object GetValue(object instance)
        {
            return PropertyInfo.GetValue(instance, null);
        }
    }
}
