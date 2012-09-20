using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class FetchMany<T> : InOutTask<FetchMany<T>.Input, FetchMany<T>.Output>  
    {
        public class Input
        {
            public virtual IDbCommand SelectCommand { get; set; }
        }

        public class Output
        {
            public virtual T[] ObjectsFetched { get; set; }
        }

        public virtual BuildObject<T> BuildObject { get; set; }

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
