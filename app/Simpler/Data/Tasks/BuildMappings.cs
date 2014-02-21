using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simpler.Data.Tasks
{
    public class BuildMappings : InOutTask<BuildMappings.Input, BuildMappings.Output>
    {
        public class Input
        {
            public Dictionary<string, int> ColumnNames { get; set; }
            public Type RootType { get; set; }
        }

        public class Output
        {
            public ObjectMapping ObjectMapping { get; set; }
        }

        public void ProcessColumnName(ObjectMapping objectMapping, string columnName, string path = "")
        {
            var remainingPath = columnName.Substring(path.Length);
            //check if we have reached the end of the path
            if (remainingPath.Length == 0)
            {
                ((ObjectMappingNode)objectMapping).ColumnIndex = In.ColumnNames[columnName];
                return;
            }

            //if the parent is a dyanmic no need to look at properties just set it
            if (objectMapping.PropertyType == null || objectMapping.PropertyType.FullName == "System.Object") 
            {
                objectMapping[remainingPath] = new ObjectMappingDynamicChildNode();
                ProcessColumnName(objectMapping[remainingPath], columnName, path + remainingPath);
                return;
            }

            //if the parent is an array we need to pull the index off and create a node.
            if (objectMapping.PropertyType.IsArray) 
            {
                //pull the index off the remainingPath
                var index = new String(remainingPath.TakeWhile(Char.IsDigit).ToArray());

                //create the node if it doesn't exist
                if (!objectMapping.ContainsColumn(index))
                {
                    objectMapping[index] = new ObjectMappingArrayChildNode
                        {
                            PropertyType = objectMapping.PropertyType.GetElementType()
                        };
                }

                ProcessColumnName(objectMapping[index], columnName, path + index);
                return;
            }

            //attempt to find an exact match
            var propertyInfo = objectMapping.PropertyType.GetProperty(remainingPath);
            if (propertyInfo == null)
            {
                //if we can't find an exact match find a property that starts with the remmaining path
                propertyInfo = objectMapping.PropertyType.GetProperties().FirstOrDefault(x => remainingPath.StartsWith(x.Name));
            }

            Check.That(propertyInfo != null, "The DataRecord contains column '{0}' to a property or nested property.", columnName);
            
            //check if it already exisits
            if (!objectMapping.ContainsColumn(propertyInfo.Name))
            {
                if (propertyInfo.PropertyType.FullName == "System.Object")
                {
                    objectMapping[propertyInfo.Name] = new ObjectMappingDynamicNode { PropertyInfo = propertyInfo, PropertyType = propertyInfo.PropertyType };
                }
                else if (propertyInfo.PropertyType.IsArray)
                {
                    objectMapping[propertyInfo.Name] = new ObjectMappingArrayNode { PropertyInfo = propertyInfo, PropertyType = propertyInfo.PropertyType };
                }
                else
                {
                    objectMapping[propertyInfo.Name] = new ObjectMappingObjectNode { PropertyInfo = propertyInfo, PropertyType = propertyInfo.PropertyType};
                }
            }

            ProcessColumnName(objectMapping[propertyInfo.Name], columnName, path + propertyInfo.Name);
        }

        public override void Execute()
        {
            var root = new ObjectMappingRoot { PropertyType = In.RootType };
            var sortedColumnNames = In.ColumnNames.OrderBy(s => s.Key);
            foreach (var columnName in sortedColumnNames)
            {
                ProcessColumnName(root, columnName.Key);
            }
            Out.ObjectMapping = root;
        }
    }
}
