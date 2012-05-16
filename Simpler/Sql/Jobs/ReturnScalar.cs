using System;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class ReturnScalar : InOutJob<ReturnScalar.Input, ReturnScalar.Output>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public object Object { get; set; }
        }

        public _RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    _Out.Object = command.ExecuteScalar();
                };

            RunAction._In.ConnectionName = _In.ConnectionName;
            RunAction._In.Sql = _In.Sql;
            RunAction._In.Values = _In.Values;
            RunAction._In.Action = action;
            RunAction.Run();
        }
    }
}