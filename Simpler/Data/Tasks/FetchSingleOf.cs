using System.Data;

namespace Simpler.Data.Tasks
{
    public class FetchSingleOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T ObjectFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
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
