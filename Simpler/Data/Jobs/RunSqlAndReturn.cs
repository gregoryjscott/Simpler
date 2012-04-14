using Simpler.Data.Jobs.Internal;

namespace Simpler.Data.Jobs
{
    // todo - Using Job<TI, TO> here just to get the InjectSubJobs attribute.
    public class RunSqlAndReturn<TModel> : InOutJob<RunSqlAndReturn<TModel>.In, RunSqlAndReturn<TModel>.Out>
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

        public override void Execute()
        {
            RunCommandAction.ConnectionName = Input.ConnectionName;
            RunCommandAction.Sql = Input.Sql;
            RunCommandAction.Values = Input.Values;
            RunCommandAction.CommandAction = command =>
                                             {
                                                 FetchModels.SelectCommand = command;
                                                 FetchModels.Execute();
                                                 Output = new Out {Models = FetchModels.ObjectsFetched};
                                             };
            RunCommandAction.Execute();
        }
    }
}