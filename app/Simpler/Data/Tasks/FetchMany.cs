using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class FetchMany<T> : IO<FetchMany<T>.Ins, FetchMany<T>.Outs>  
    {
        public class Ins
        {
            public IDbCommand SelectCommand { get; set; }
        }

        public class Outs
        {
            public T[] ObjectsFetched { get; set; }
        }

        public BuildObject<T> BuildObject { get; set; }

        public override void Execute()
        {
            var objectList = new List<T>();

            using (var dataReader = In.SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    BuildObject.In.DataRecord = dataReader;
                    BuildObject.Execute();
                    var newObject = BuildObject.Out.Object;

                    objectList.Add(newObject);
                }
            }

            Out.ObjectsFetched = objectList.ToArray();
        }
    }
}
