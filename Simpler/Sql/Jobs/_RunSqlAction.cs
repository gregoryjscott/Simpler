using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Jobs
{
    public class _RunSqlAction : InOutJob<object, object>
    {
        // Inputs
        public string ConnectionName { get; set; }
        public string Sql { get; set; }
        public object Values { get; set; }
        public Action<IDbCommand> CommandAction { get; set; }

        // Sub-jobs
        public _BuildParameters BuildParameters { get; set; }

        public override void Run()
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
                        BuildParameters.Run();                        
                    }

                    CommandAction(command);
                }
            }
        }
    }
}
