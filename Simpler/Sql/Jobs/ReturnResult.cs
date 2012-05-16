using System;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class ReturnResult : InOutJob<ReturnResult.Input, ReturnResult.Output>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public int RowsAffected { get; set; }
        }

        public _RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    Out.RowsAffected = command.ExecuteNonQuery();
                };

            RunAction.In.ConnectionName = In.ConnectionName;
            RunAction.In.Sql = In.Sql;
            RunAction.In.Values = In.Values;
            RunAction.In.Action = action;
            RunAction.Run();
        }
    }
}