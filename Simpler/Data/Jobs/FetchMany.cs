using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Jobs
{
    public class FetchMany<T> : Job
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-jobs
        public virtual BuildObject<T> BuildObject { get; set; }

        public override void Run()
        {
            // Create the sub-jobs.
            if (BuildObject == null) BuildObject = new BuildObject<T>();

            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    BuildObject.In.DataRecord = dataReader;
                    BuildObject.Run();
                    var newObject = BuildObject.Out.Object;

                    objectList.Add(newObject);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }
    }
}
