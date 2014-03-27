using System;
using System.Data;
using System.Dynamic;
using Simpler.Data.PropertyMappingTree;

namespace Simpler.Data.Tasks
{
    public class BuildObject<T> : InOutTask<BuildObject<T>.Input, BuildObject<T>.Output>
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
            public AbstractNode PropertyParse { get; set; }
        }

        public class Output
        {
            public T Object { get; set; }
        }

        public void Parse(AbstractNode node, object obj)
        {
            //if the node does not have a index then just set the value to the instance.
            if (node.Index == null)
            {
                node.SetValue(obj, null);
            }
            else
            {
                //get the value from the data reader and set the value
                var value = In.DataRecord.GetValue((int)node.Index);
                if (value != null && value.GetType() != typeof(DBNull))
                {
                    node.SetValue(obj, value);
                }
            }
            
            //get the newly created object
            var childObj = node.GetValue(obj);

            //continue down the parse tree
            foreach (var childNode in node.Children)
            {
                Parse(childNode, childObj);
            }
        }

        public override void Execute()
        {
            //create the root object
            var obj = In.PropertyParse.CreateObject();
            foreach (var node in In.PropertyParse.Children)
            {
                Parse(node, obj);
            }
            Out.Object = (T)obj;
        }
    }
}