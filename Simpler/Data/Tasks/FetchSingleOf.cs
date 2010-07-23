using System.Data;
using Simpler.Injection;

namespace Simpler.Data.Tasks
{
    [InjectSubTasks]
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
