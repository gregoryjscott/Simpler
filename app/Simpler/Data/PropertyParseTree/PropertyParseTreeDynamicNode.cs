using System.Dynamic;

namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyParseTreeDynamicNode : PropertyParseTreeObjectNode
    {
        public override object CreateObject(object value = null)
        {
            return new ExpandoObject();
        }
    }
}
