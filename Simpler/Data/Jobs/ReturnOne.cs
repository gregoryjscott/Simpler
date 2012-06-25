using System;
using System.Data;
using System.Linq;

namespace Simpler.Data.Jobs
{
    public class ReturnOne<T> : InOutJob<ReturnOne<T>.Input, ReturnOne<T>.Output>
    {
        public class Input
        {
            public IDbConnection Connection { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public T Model { get; set; }
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
                    Out.Model = FetchMany.ObjectsFetched.Single();
                };

            RunAction.In.Connection = In.Connection;
            RunAction.In.Sql = In.Sql;
            RunAction.In.Values = In.Values;
            RunAction.In.Action = action;
            RunAction.Run();
        }
    }
}