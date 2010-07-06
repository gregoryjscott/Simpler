using System.Data;

namespace Simpler.Sql.Tasks
{
    public class FetchSingleOf<T> : Task
    {
        // Inputs
        public IDbCommand SelectCommand { get; set; }

        // Outputs
        public T ObjectFetched { get; private set; }

        // Sub-tasks
        public UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks if null (this won't be necessary after dependency injection is implemented).
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuild<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                dataReader.Read();
                UseDataRecordToBuild.DataRecord = dataReader;
                UseDataRecordToBuild.Execute();
                ObjectFetched = UseDataRecordToBuild.Object;
            }
        }
    }
}
