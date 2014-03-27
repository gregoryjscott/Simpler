using System;

namespace Simpler.Data.PropertyMappingTree
{
    /// <summary>
    /// 
    /// </summary>
    public class ArrayNode : ObjectNode
    {
        public override object CreateObject(object value = null)
        {
            return Activator.CreateInstance(PropertyType, new object[] { Children.Count });
        }
    }
}
