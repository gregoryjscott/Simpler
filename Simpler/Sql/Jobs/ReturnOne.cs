using System.Linq;

namespace Simpler.Sql.Jobs
{
    public class ReturnOne<TModel> : InOutJob<ReturnOne<TModel>.In, ReturnOne<TModel>.Out>
    {
        public class In
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
        }

        public class Out
        {
            public TModel Model { get; set; }
        }

        public _RunSqlAction RunSqlAction { get; set; }
        public _Fetch<TModel> Fetch { get; set; }

        public override void Run()
        {
            RunSqlAction.ConnectionName = _In.ConnectionName;
            RunSqlAction.Sql = _In.Sql;
            RunSqlAction.Values = _In.Values;
            RunSqlAction.CommandAction =
                command =>
                {
                    Fetch.SelectCommand = command;
                    Fetch.Run();
                    _Out = new Out {Model = Fetch.ObjectsFetched.Single()};
                };
            RunSqlAction.Run();
        }
    }
}