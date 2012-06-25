using System;
using System.Data;

namespace Simpler.Data.Jobs
{
    public class ReturnResult : InOutJob<ReturnResult.Input, ReturnResult.Output>
    {
        public class Input
        {
            public IDbConnection Connection { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public int RowsAffected { get; set; }
        }

        public RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    Out.RowsAffected = command.ExecuteNonQuery();
                };

            RunAction.In.Connection = In.Connection;
            RunAction.In.Sql = In.Sql;
            RunAction.In.Values = In.Values;
            RunAction.In.Action = action;
            RunAction.Run();
        }
    }
}