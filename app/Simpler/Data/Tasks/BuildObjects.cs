using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Simpler.Data.Tasks
{
    public class BuildObjects<T> : InOutTask<BuildObjects<T>.Input, BuildObjects<T>.Output>  
    {
        public class Input
        {
            public IDataReader Reader { get; set; }
        }

        public class Output
        {
            public T[] Objects { get; set; }
        }

        public BuildMappings BuildMappings { get; set; }
        public BuildObject<T> BuildObject { get; set; }

        public override void Execute()
        {
            var objectList = new List<T>();

            //read the first record off and determine the column mappings
            In.Reader.Read();

            var columns = new Dictionary<string, int>();
            for (var i = 0; i < In.Reader.FieldCount; i++)
            {
                columns.Add(In.Reader.GetName(i), i);
            }
            BuildMappings.In.ColumnNames = columns;
            BuildMappings.In.RootType = typeof (T);
            BuildMappings.Execute();
            
            do
            {
                BuildObject.In.ObjectMapping = BuildMappings.Out.ObjectMapping;
                BuildObject.In.DataRecord = In.Reader;
                BuildObject.Execute();
                objectList.Add(BuildObject.Out.Object);
            } while (In.Reader.Read());

            Out.Objects = objectList.ToArray();
        }
    }
}
