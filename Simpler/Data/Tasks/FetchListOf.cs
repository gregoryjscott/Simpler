using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class FetchListOf<T> : Task
    {
        // Inputs
        public IDbCommand SelectCommand { get; set; }

        // Outputs
        public T[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks if null (this won't be necessary after dependency injection is implemented).
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuild<T>();

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
