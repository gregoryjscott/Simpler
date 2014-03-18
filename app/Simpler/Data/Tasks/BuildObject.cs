using System;
using System.Data;
using System.Dynamic;
using Simpler.Data.PropertyParseTree;

namespace Simpler.Data.Tasks
{
    public class BuildObject<T> : InOutTask<BuildObject<T>.Input, BuildObject<T>.Output>
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
            public PropertyParseTree.PropertyParseTree PropertyParseTree { get; set; }
        }

        public class Output
        {
            public T Object { get; set; }
        }

        public void Parse(PropertyParseTreeNode treeNode, object obj)
        {
            //if the treeNode does not have a index then just set the value to the instance.
            if (treeNode.Index == null)
            {
                treeNode.SetValue(obj, null);
            }
            else
            {
                //get the value from the data reader and set the value
                var value = In.DataRecord.GetValue((int)treeNode.Index);
                if (value != null && value.GetType() != typeof(DBNull))
                {
                    treeNode.SetValue(obj, value);
                }
            }
            
            //get the newly created object
            var childObj = treeNode.GetValue(obj);

            //continue down the parse tree
            foreach (var childNode in treeNode.Nodes)
            {
                Parse(childNode, childObj);
            }
        }

        public override void Execute()
        {
            //create the root object
            var obj = In.PropertyParseTree.CreateObject();
            foreach (var node in In.PropertyParseTree.Nodes)
            {
                Parse(node, obj);
            }
            Out.Object = (T)obj;
        }
    }
}