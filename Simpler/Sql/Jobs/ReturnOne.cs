using System;
using System.Data;
using System.Linq;

namespace Simpler.Sql.Jobs
{
    public class ReturnOne<TModel> : InOutJob<ReturnOne<TModel>.Input, ReturnOne<TModel>.Output>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
        {
            public TModel Model { get; set; }
        }

        public _RunAction RunAction { get; set; }
        public _Fetch<TModel> Fetch { get; set; }

        public override void Run()
        {
            Action<IDbCommand> action =
                command =>
                {
                    Fetch.SelectCommand = command;
                    Fetch.Run();
                    _Out.Model = Fetch.ObjectsFetched.Single();
                };

            RunAction._In.ConnectionName = _In.ConnectionName;
            RunAction._In.Sql = _In.Sql;
            RunAction._In.Values = _In.Values;
            RunAction._In.Action = action;
            RunAction.Run();
        }
    }
}