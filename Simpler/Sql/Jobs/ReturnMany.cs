using System;
using System.Data;

namespace Simpler.Sql.Jobs
{
    public class ReturnMany<TModel> : InOutJob<ReturnMany<TModel>.Input, ReturnMany<TModel>.Output>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Output
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
                    _Out = new Output {Models = Fetch.ObjectsFetched};
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