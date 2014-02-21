using System;
using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class BuildObjects<T>: InOutTask<BuildObjects<T>.Input, BuildObjects<T>.Output>
    {
        public class Input
        {
            public IDataReader Reader { get; set; }
        }

        public class Output
        {
            public T[] Objects { get; set; }
        }

        public BuildTyped<T> BuildTyped { get; set; }
        public BuildDynamic BuildDynamic { get; set; }

        public override void Execute()
        {
            Func<IDataReader, T> buildObject;
            if (typeof (T).FullName == "System.Object")
            {
                buildObject = reader => {
                    BuildDynamic.In.DataRecord = reader;
                    BuildDynamic.Execute();
                    return BuildDynamic.Out.Object;
                };
            }
            else
            {
                buildObject = reader => {
                    BuildTyped.In.DataRecord = reader;
                    BuildTyped.Execute();
                    return BuildTyped.Out.Object;
                };
            }

            var objectList = new List<T>();
            while (In.Reader.Read())
            {
                objectList.Add(buildObject(In.Reader));
            }
            Out.Objects = objectList.ToArray();
        }
    }
}
