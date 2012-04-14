using System.Collections.Generic;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class _Fetch<T> : Job
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-jobs
        public virtual _Build<T> Build { get; set; }

        public override void Run()
        {
            // Create the sub-jobs.
            if (Build == null) Build = new _Build<T>();

            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Build.DataRecord = dataReader;
                    Build.Run();
                    objectList.Add(Build.Object);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }
    }
}
