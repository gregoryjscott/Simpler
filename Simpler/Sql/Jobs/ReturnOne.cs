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
                    _Out = new Output {Model = Fetch.ObjectsFetched.Single()};
                };

            RunAction
                .Set(new _RunAction.Input
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