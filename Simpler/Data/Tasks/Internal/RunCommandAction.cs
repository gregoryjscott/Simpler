using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Tasks.Internal
{
    // todo - Using Task<TI, TO> here just to get the InjectSubTasks attribute.
    public class RunCommandAction : Task<object, object>
    {
        // Inputs
        public string ConnectionName { get; set; }
        public string Sql { get; set; }
        public object Values { get; set; }
        public Action<IDbCommand> CommandAction { get; set; }

        // Sub-tasks
        public BuildParameters BuildParameters { get; set; }

        public override void Execute()
        {
            if (String.IsNullOrEmpty(ConnectionName)) throw new ArgumentException("ConnectionName property must be set.");
            if (String.IsNullOrEmpty(Sql)) throw new ArgumentException("Sql property must be set.");

            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[ConnectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = provider.CreateConnection())
            {
                if (connection == null) throw new CreateConnectionException(connectionString, providerName);

                connection.ConnectionString = connectionString;

                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = Sql;

                    if (Values != null)
                    {
                        BuildParameters.CommandWithParameters = command;
                        BuildParameters.ObjectWithValues = Values;
                        BuildParameters.Execute();                        
                    }

                    CommandAction(command);
                }
            }
        }
    }
}
