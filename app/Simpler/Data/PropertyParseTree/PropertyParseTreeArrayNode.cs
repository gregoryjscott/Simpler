using System;

namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyParseTreeArrayNode : PropertyParseTreeObjectNode
    {
        public override object CreateObject(object value = null)
        {
            return Activator.CreateInstance(PropertyType, new object[] { Nodes.Count });
        }
    }
}
