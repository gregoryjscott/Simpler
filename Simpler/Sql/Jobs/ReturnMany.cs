using Simpler.Sql.Jobs.Internal;

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

        public RunCommandAction RunCommandAction { get; set; }
        public FetchListOf<TModel> FetchModels { get; set; }

        public override void Run()
        {
            RunCommandAction.ConnectionName = _In.ConnectionName;
            RunCommandAction.Sql = _In.Sql;
            RunCommandAction.Values = _In.Values;
            RunCommandAction.CommandAction =
                command =>
                {
                    FetchModels.SelectCommand = command;
                    FetchModels.Run();
                    _Out = new Out {Models = FetchModels.ObjectsFetched};
                };
            RunCommandAction.Run();
        }
    }
}