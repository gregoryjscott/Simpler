using System;
using System.Data;

namespace Simpler.Data.Jobs
{
    public class ReturnMany<T> : InOutJob<ReturnMany<T>.Input, ReturnMany<T>.Output>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public T[] Models { get; set; }
        }

        public RunAction RunAction { get; set; }
        public FetchMany<T> FetchMany { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    FetchMany.SelectCommand = command;
                    FetchMany.Run();
                    Out.Models = FetchMany.ObjectsFetched;
                };

            RunAction.In.ConnectionName = In.ConnectionName;
            RunAction.In.Sql = In.Sql;
            RunAction.In.Values = In.Values;
            RunAction.In.Action = action;
            RunAction.Run();
        }
    }
}