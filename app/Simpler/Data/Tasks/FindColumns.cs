using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Simpler.Data.PropertyMappingTree;

namespace Simpler.Data.Tasks
{
    public class FindColumns : InOutTask<FindColumns.Input, FindColumns.Output>
    {
        public class Input
        {
            public IDataReader Reader { get; set; }
        }

        public class Output
        {
            public Dictionary<string, int> Columns { get; set; }
        }

        public override void Execute()
        {
            Out.Columns = new Dictionary<string, int>();
            for (var i = 0; i < In.Reader.FieldCount; i++)
            {
                var columnName = In.Reader.GetName(i);
                Check.That(!Out.Columns.ContainsKey(columnName), "The DataRecord contains a duplicate column '{0}'.", columnName);
                Out.Columns[columnName] = i;
            }
        }
    }
}
