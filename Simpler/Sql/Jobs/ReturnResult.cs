using System;
using System.Data;

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

        public _RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    _Out = new Out {RowsAffected = command.ExecuteNonQuery()};
                };

            RunAction
                .Set(new _RunAction.In
                     {
                         ConnectionName = _In.ConnectionName,
                         Sql = _In.Sql,
                         Values = _In.Values,
                         Action = action
                     })
                .Run();
        }
    }
}