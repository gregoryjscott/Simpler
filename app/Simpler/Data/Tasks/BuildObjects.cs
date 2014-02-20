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
        public BuildTyped<T> BuildTyped { get; set; }
        public BuildDynamic BuildDynamic { get; set; }

        public override void Execute()
        {
            Func<IDataReader, Dictionary<string, BuildMappings.ObjectMapping>, T> buildObject;
            if (typeof (T).FullName == "System.Object")
            {
                buildObject = (reader, map) =>
                              {
                                  BuildDynamic.In.DataRecord = reader;
                                  BuildDynamic.Execute();
                                  return BuildDynamic.Out.Object;
                              };
            }
            else
            {
                buildObject = (reader, map) =>
                              {
                                  BuildTyped.In.Map = map;
                                  BuildTyped.In.DataRecord = reader;
                                  BuildTyped.Execute();
                                  return BuildTyped.Out.Object;
                              };
            }

            var objectList = new List<T>();
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
                objectList.Add(buildObject(In.Reader, BuildMappings.Out.ObjectMapping));
            } while (In.Reader.Read());

            Out.Objects = objectList.ToArray();
        }
    }
}
