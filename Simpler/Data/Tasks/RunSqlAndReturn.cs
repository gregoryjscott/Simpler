using System;
using System.Configuration;
using System.Data.Common;

namespace Simpler.Data.Tasks
{
    // todo - Using Task<TI, TO> here just to get the InjectSubTasks attribute.
    public class RunSqlAndReturn<TModel> : Task<object, object>
    {
        // Inputs
        public string ConnectionName { get; set; }
        public string Sql { get; set; }
        public object Values { get; set; }

        // Outputs
        public TModel[] Models { get; private set; }

        // Sub-tasks
        public BuildParameters BuildParameters { get; set; }
        public FetchListOf<TModel> FetchModels { get; set;}

        public override void Execute()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[ConnectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = provider.CreateConnection())
            {
                if (connection == null) throw new Exception("todo");

                connection.ConnectionString = connectionString;

                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = Sql;

                    BuildParameters.CommandWithParameters = command;
                    BuildParameters.ObjectWithValues = Values;
                    BuildParameters.Execute();

                    FetchModels.SelectCommand = command;
                    FetchModels.Execute();
                }
            }

            Models = FetchModels.ObjectsFetched;
        }
    }
}