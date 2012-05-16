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
                    _Out = new Output {Object = command.ExecuteScalar()};
                };

            RunAction._In = new _RunAction.Input
                                {
                                    ConnectionName = _In.ConnectionName,
                                    Sql = _In.Sql,
                                    Values = _In.Values,
                                    Action = action
                                };
            RunAction.Run();
        }
    }
}