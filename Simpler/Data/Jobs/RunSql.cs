using Simpler.Data.Jobs.Internal;

namespace Simpler.Data.Jobs
{
    // todo - Using Job<TI, TO> here just to get the InjectSubJobs attribute.
    public class RunSql : InOutJob<RunSql.In, RunSql.Out>
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