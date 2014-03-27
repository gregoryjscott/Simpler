using System.Dynamic;

namespace Simpler.Data.PropertyMappingTree
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicNode : ObjectNode
    {
        public override object CreateObject(object value = null)
        {
            return new ExpandoObject();
        }
    }
}
