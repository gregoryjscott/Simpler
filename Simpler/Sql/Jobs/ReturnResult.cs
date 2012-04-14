using Simpler.Sql.Jobs.Internal;

namespace Simpler.Sql.Jobs
{
    public class ReturnResult : InOutJob<ReturnResult.In, ReturnResult.Out>
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

        public override void Run()
        {
            RunCommandAction.ConnectionName = _In.ConnectionName;
            RunCommandAction.Sql = _In.Sql;
            RunCommandAction.Values = _In.Values;
            RunCommandAction.CommandAction = command =>
                                                 {
                                                     _Out = new Out {RowsAffected = command.ExecuteNonQuery()};
                                                 };
            RunCommandAction.Run();
        }
    }
}