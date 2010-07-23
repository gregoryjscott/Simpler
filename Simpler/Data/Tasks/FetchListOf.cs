using System.Collections.Generic;
using System.Data;
using Simpler.Injection;

namespace Simpler.Data.Tasks
{
    [InjectSubTasks]
    public class FetchListOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    UseDataRecordToBuild.DataRecord = dataReader;
                    UseDataRecordToBuild.Execute();
                    objectList.Add(UseDataRecordToBuild.Object);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }
    }
}
