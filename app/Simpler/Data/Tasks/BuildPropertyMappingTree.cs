using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Simpler.Data.PropertyMappingTree;

namespace Simpler.Data.Tasks
{
    public class BuildPropertyMappingTree : InOutTask<BuildPropertyMappingTree.Input, BuildPropertyMappingTree.Output>
    {
        public class Input
        {
            public Dictionary<string, int> Columns { get; set; }
            public Type InitialType { get; set; }
        }

        public class Output
        {
            public AbstractNode PropertyMappingTree { get; set; }
        }

        public override void Execute()
        {
            var parseColumn = new ParseColumn();

            if (In.InitialType.FullName == "System.Object")
            {
                 parseColumn.In.RootNode = new DynamicNode{ PropertyType = In.InitialType };
            }
            else
            {
                 parseColumn.In.RootNode = new ObjectNode{ PropertyType = In.InitialType };
            }

            //order by the longest column names first
            var columns = In.Columns.OrderByDescending(x => x.Key.Length);
            foreach (var column in columns)
            {
                parseColumn.In.ColumnName = column.Key;
                parseColumn.In.ColumnIndex = column.Value;
                parseColumn.Execute();
            }
            Out.PropertyMappingTree = parseColumn.In.RootNode;
        }
    }
}
