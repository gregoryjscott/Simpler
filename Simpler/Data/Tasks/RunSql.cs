using Simpler.Data.Tasks.Internal;

namespace Simpler.Data.Tasks
{
    // todo - Using Task<TI, TO> here just to get the InjectSubTasks attribute.
    public class RunSql : Task<object, object>
    {
        // Inputs
        public string ConnectionName { get; set; }
        public string Sql { get; set; }
        public object Values { get; set; }

        // Outputs
        public int RowsAffected { get; private set; }

        // Sub-tasks
        public RunCommandAction RunCommandAction { get; set; }

        public override void Execute()
        {
            RunCommandAction.ConnectionName = ConnectionName;
            RunCommandAction.Sql = Sql;
            RunCommandAction.Values = Values;
            RunCommandAction.CommandAction = command =>
                                             {
                                                 RowsAffected = command.ExecuteNonQuery();
                                             };
            RunCommandAction.Execute();
        }
    }
}