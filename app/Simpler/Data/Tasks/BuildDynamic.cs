using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Simpler.Data.Tasks
{
    public class BuildDynamic: InOutTask<BuildDynamic.Input, BuildDynamic.Output>
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
        }

        public class Output
        {
            public dynamic Object { get; set; }
        }

        public override void Execute()
        {
            Out.Object = new ExpandoObject();
            var dictionary = Out.Object as IDictionary<string, Object>;
            for (var i = 0; i < In.DataRecord.FieldCount; i++)
            {
                var columnName = In.DataRecord.GetName(i);
                var columnValue = In.DataRecord[columnName];
                if (columnValue.GetType() != typeof (DBNull))
                {
                    dictionary.Add(columnName, columnValue);
                }
            }
        }
    }
}
