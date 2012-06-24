using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Jobs
{
    public class _RunAction : InJob<_RunAction.Input>
    {
        public class Input
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
            public Action<IDbCommand> Action { get; set; }
        }

        public _BuildParameters BuildParameters { get; set; }

        public override void Run()
        {
            Check.That(!String.IsNullOrEmpty(In.ConnectionName), "ConnectionName property must be set.");
            Check.That(!String.IsNullOrEmpty(In.Sql), "Sql property must be set.");

            var connectionString = ConfigurationManager.ConnectionStrings[In.ConnectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[In.ConnectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = provider.CreateConnection())
            {
                Check.That(connection != null, String.Format("Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", In.ConnectionName, providerName));

                connection.ConnectionString = connectionString;

                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = In.Sql;

                    if (In.Values != null)
                    {
                        BuildParameters.Command = command;
                        BuildParameters.Values = In.Values;
                        BuildParameters.Run();                        
                    }

                    In.Action(command);
                }
            }
        }
    }
}
