using System;
using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class FetchMany<T> : InOutTask<FetchMany<T>.Input, FetchMany<T>.Output>  
    {
        public class Input
        {
            public IDbCommand SelectCommand { get; set; }
        }

        public class Output
        {
            public T[] ObjectsFetched { get; set; }
        }

        public BuildTyped<T> BuildTyped { get; set; }
        public BuildDynamic BuildDynamic { get; set; }

        public override void Execute()
        {
            Func<IDataReader, T> buildObject;
            if (typeof(T).FullName == "System.Object")
            {
                buildObject = reader =>
                                  {
                                      BuildDynamic.In.DataRecord = reader;
                                      BuildDynamic.Execute();
                                      return BuildDynamic.Out.Object;
                                  };
            }
            else
            {
                buildObject = reader =>
                                  {
                                      BuildTyped.In.DataRecord = reader;
                                      BuildTyped.Execute();
                                      return BuildTyped.Out.Object;
                                  };
            }

            var objectList = new List<T>();
            using (var dataReader = In.SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    objectList.Add(buildObject(dataReader));
                }
            }
            Out.ObjectsFetched = objectList.ToArray();
        }
    }
}
