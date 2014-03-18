using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Data.PropertyParseTree;

namespace Simpler.Data.Tasks
{
    public class ParseColumn : InTask<ParseColumn.Input>
    {
        public class Input
        {
            public int ColumnIndex { get; set; }
            public string ColumnName { get; set; }
            public PropertyParseTree.PropertyParseTree PropertyParseTree { get; set; }
        }

        private void ParseColumnName(PropertyParseTree.PropertyParseTree propertyParseTree, string path = "")
        {
            var remainingPath = In.ColumnName.Substring(path.Length);
            //check if we have reached the end of the path
            if (remainingPath.Length == 0)
            {
                ((PropertyParseTreeNode) propertyParseTree).Index = In.ColumnIndex;
                return;
            }

            var propertyType = propertyParseTree.PropertyType;

            //if the parent is a dyanmic no need to look at properties just set it
            if (propertyType == null || propertyType.FullName == "System.Object")
            {
                propertyParseTree[remainingPath] = new PropertyParseTreeDynamicChildNode();
                ParseColumnName(propertyParseTree[remainingPath], path + remainingPath);
                return;
            }

            //if the parent is an array we need to pull the index off and create a treeNode.
            if (propertyType.IsArray)
            {
                //pull the index off the remainingPath
                var index = new String(remainingPath.TakeWhile(Char.IsDigit).ToArray());

                //create the treeNode if it doesn't exist
                if(propertyParseTree.Nodes.All(x => x.Name != index))
                {
                    propertyParseTree[index] = new PropertyParseTreeArrayChildNode
                    {
                        PropertyType = propertyType.GetElementType()
                    };
                }

                ParseColumnName(propertyParseTree[index], path + index);
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

            //check if the treeNode already exisits in the parse tree
            if (!propertyParseTree.Nodes.Any(x => x.Name == propertyInfo.Name))
            {
                //determine the type of the treeNode and add it to the parse tree
                if (propertyInfo.PropertyType.FullName == "System.Object")
                {
                    propertyParseTree[propertyInfo.Name] = new PropertyParseTreeDynamicNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
                else if (propertyInfo.PropertyType.IsArray)
                {
                    propertyParseTree[propertyInfo.Name] = new PropertyParseTreeArrayNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
                else
                {
                    propertyParseTree[propertyInfo.Name] = new PropertyParseTreeObjectNode
                    {
                        PropertyInfo = propertyInfo,
                        PropertyType = propertyInfo.PropertyType
                    };
                }
            }

            ParseColumnName(propertyParseTree[propertyInfo.Name], path + propertyInfo.Name);
        }

        public override void Execute()
        {
            ParseColumnName(In.PropertyParseTree);
        }
    }
}
