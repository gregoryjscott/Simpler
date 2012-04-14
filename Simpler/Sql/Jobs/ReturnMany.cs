using System;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class ReturnMany<TModel> : InOutJob<ReturnMany<TModel>.In, ReturnMany<TModel>.Out>
    {
        public class In
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Out
        {
            public TModel[] Models { get; set; }
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
                    _Out = new Out {Models = Fetch.ObjectsFetched};
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