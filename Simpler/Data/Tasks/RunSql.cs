using Simpler.Data.Tasks.Internal;

namespace Simpler.Data.Tasks
{
    // todo - Using Task<TI, TO> here just to get the InjectSubTasks attribute.
    public class RunSql : InOutTask<RunSql.In, RunSql.Out>
    {
        public class In
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Out
        {
            public int RowsAffected { get; set; }
        }

        public RunCommandAction RunCommandAction { get; set; }

        public override void Execute()
        {
            RunCommandAction.ConnectionName = Input.ConnectionName;
            RunCommandAction.Sql = Input.Sql;
            RunCommandAction.Values = Input.Values;
            RunCommandAction.CommandAction = command =>
                                                 {
                                                     Output = new Out {RowsAffected = command.ExecuteNonQuery()};
                                                 };
            RunCommandAction.Execute();
        }
    }
}