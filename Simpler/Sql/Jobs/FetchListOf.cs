using System.Collections.Generic;
using System.Data;

namespace Simpler.Sql.Jobs
{
    /// <summary>
    /// Job that will create a list of objects of the given type T using the results of the given command.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list to return.</typeparam>
    public class FetchListOf<T> : Job
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-jobs
        public virtual UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Run()
        {
            // Create the sub-jobs.
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuild<T>();

            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    UseDataRecordToBuild.DataRecord = dataReader;
                    UseDataRecordToBuild.Run();
                    objectList.Add(UseDataRecordToBuild.Object);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }
    }
}
