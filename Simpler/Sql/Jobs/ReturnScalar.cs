using System;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class ReturnScalar : InOutJob<ReturnScalar.In, ReturnScalar.Out>
    {
        public class In
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Out
        {
            public Object Object { get; set; }
        }

        public _RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    _Out = new Out {Object = command.ExecuteScalar()};
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