using System;
using System.Linq;
using Simpler.Data.PropertyMappingTree;

namespace Simpler.Data.Tasks
{
    public class ParseColumn : InTask<ParseColumn.Input>
    {
        public class Input
        {
            public int ColumnIndex { get; set; }
            public string ColumnName { get; set; }
            public AbstractNode RootNode { get; set; }
        }

        private void ParseColumnName(AbstractNode node, string path = "")
        {
            var remainingPath = In.ColumnName.Substring(path.Length);
            //check if we have reached the end of the path
            if (remainingPath.Length == 0)
            {
                node.Index = In.ColumnIndex;
                return;
            }

            var propertyType = node.PropertyType;

            //if the parent is a dyanmic no need to look at properties just set it
            if (node is DynamicNode)
            {
                node[remainingPath] = new DynamicPropertyNode();
                ParseColumnName(node[remainingPath], path + remainingPath);
                return;
            }

            //if the parent is an array we need to pull the index off and create a TreeNode.
            if (node is ArrayNode)
            {
                //pull the index off the remainingPath
                var index = new String(remainingPath.TakeWhile(Char.IsDigit).ToArray());

                //create the TreeNode if it doesn't exist
                if(node.Children.All(x => x.Name != index))
                {
                    node[index] = new ArrayElementNode
                    {
                        PropertyType = propertyType.GetElementType()
                    };
                }

                ParseColumnName(node[index], path + index);
                return;
            }

            //attempt to find an exact match
            var propertyInfo = propertyType.GetProperty(remainingPath);
            if (propertyInfo == null)
            {
                //if we can't find an exact match find a property that starts with the remmaining path
                propertyInfo = propertyType.GetProperties().FirstOrDefault(x => remainingPath.StartsWith(x.Name));
            }

            Check.That(propertyInfo != null, "The DataRecord contains a column '{0}' that does not match a property or nested property.", In.ColumnName);

            //check if the node already exisits in the parse tree
            if (!node.Children.Any(x => x.Name == propertyInfo.Name))
            {
                //determine the type of the node and add it to the parse tree
                if (propertyInfo.PropertyType.FullName == "System.Object")
                {
                    node[propertyInfo.Name] = new DynamicNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
                else if (propertyInfo.PropertyType.IsArray)
                {
                    node[propertyInfo.Name] = new ArrayNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
                else
                {
                    node[propertyInfo.Name] = new ObjectNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
            }

            ParseColumnName(node[propertyInfo.Name], path + propertyInfo.Name);
        }

        public override void Execute()
        {
            ParseColumnName(In.RootNode);
        }
    }
}
