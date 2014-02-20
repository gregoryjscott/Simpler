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
            public Dictionary<string, ObjectMapping> ObjectMapping { get; set; }
        }

        public class ObjectMapping
        {
            public PropertyInfo PropertyInfo { get; set; }
            public Type PropertyType { get; set; }
            public int? Column { get; set; }
            public Dictionary<string, ObjectMapping> Children = new Dictionary<string, ObjectMapping>();
        }

        public void ProcessColumnName(Dictionary<string, ObjectMapping> objectMapping, Type currentType, string columnName, string path = "")
        {
            var currentColumnName = columnName.Substring(path.Length);
            if (currentType.IsArray)
            {
                //find if there is an index
                var index = new String(currentColumnName.TakeWhile(Char.IsDigit).ToArray());
                if (!objectMapping.ContainsKey(index))
                {
                    objectMapping[index] = new ObjectMapping { PropertyType = currentType};
                }

                ProcessColumnName(objectMapping[index].Children, currentType.GetElementType(), columnName, path += index);
                return;
            }

            var propertyInfo = currentType.GetProperty(currentColumnName);
            if (propertyInfo != null)
            {
                if (!objectMapping.ContainsKey(currentColumnName))
                {
                    objectMapping[currentColumnName] = new ObjectMapping { PropertyInfo = propertyInfo, PropertyType = currentType, Column = In.ColumnNames[columnName] };
                }
                return;
            }

            var complexPropertyInfo = currentType.GetProperties().FirstOrDefault(x => currentColumnName.StartsWith(x.Name));
            if (complexPropertyInfo != null)
            {
                if (!objectMapping.ContainsKey(complexPropertyInfo.Name))
                {
                    objectMapping[complexPropertyInfo.Name] = new ObjectMapping { PropertyInfo = complexPropertyInfo, PropertyType = currentType };
                }

                ProcessColumnName(objectMapping[complexPropertyInfo.Name].Children, complexPropertyInfo.PropertyType, columnName, path += complexPropertyInfo.Name);
                return;
            }

            //throw an exception mapping not found
        }

        public void CreateMapping(Dictionary<string, ObjectMapping> rootMapping, Type currentType)
        {
            var sortedColumnNames = In.ColumnNames.OrderBy(s => s.Key);
            foreach (var columnName in sortedColumnNames)
            {
                ProcessColumnName(rootMapping, currentType, columnName.Key);
            }
        }

        public override void Execute()
        {
            var root = new Dictionary<string, ObjectMapping>();
            CreateMapping(root, In.RootType);
            Out.ObjectMapping = root;
        }
    }
}
