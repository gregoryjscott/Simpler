using System;
using System.Dynamic;

namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyParseTreeRootNode : PropertyParseTree
    {
        public override object CreateObject(object value = null)
        {
            if (IsDynamicProperty)
            {
                return new ExpandoObject();
            }

            return Activator.CreateInstance(PropertyType);
        }
    }
}