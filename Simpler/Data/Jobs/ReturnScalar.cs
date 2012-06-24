using System;
using System.Data;

namespace Simpler.Data.Jobs
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

        public RunAction RunAction { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    Out.Object = command.ExecuteScalar();
                };

            RunAction.In.ConnectionName = In.ConnectionName;
            RunAction.In.Sql = In.Sql;
            RunAction.In.Values = In.Values;
            RunAction.In.Action = action;
            RunAction.Run();
        }
    }
}