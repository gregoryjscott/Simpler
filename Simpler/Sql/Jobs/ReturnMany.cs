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
                    _Out = new Out {Models = Fetch.ObjectsFetched};
                };
            RunSqlAction.Run();
        }
    }
}