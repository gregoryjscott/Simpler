using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Simpler.Data.PropertyParseTree;

namespace Simpler.Data.Tasks
{
    public class BuildPropertyParseTree : InOutTask<BuildPropertyParseTree.Input, BuildPropertyParseTree.Output>
    {
        public class Input
        {
            public Dictionary<string, int> Columns { get; set; }
            public Type InitialType { get; set; }
        }

        public class Output
        {
            public PropertyParseTree.PropertyParseTree PropertyParseTree { get; set; }
        }

        public override void Execute()
        {
            var parseColumn = new ParseColumn();

            parseColumn.In.PropertyParseTree = new PropertyParseTreeRootNode { PropertyType = In.InitialType };

            //order by the longest column names first
            var columns = In.Columns.OrderByDescending(x => x.Key.Length);
            foreach (var column in columns)
            {
                parseColumn.In.ColumnName = column.Key;
                parseColumn.In.ColumnIndex = column.Value;
                parseColumn.Execute();
            }
            Out.PropertyParseTree = parseColumn.In.PropertyParseTree;
        }
    }
}
