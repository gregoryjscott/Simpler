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
            if (String.IsNullOrEmpty(In.ConnectionName)) throw new ArgumentException("ConnectionName property must be set.");
            if (String.IsNullOrEmpty(In.Sql)) throw new ArgumentException("Sql property must be set.");

            var connectionString = ConfigurationManager.ConnectionStrings[In.ConnectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[In.ConnectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = provider.CreateConnection())
            {
                if (connection == null) throw new CreateConnectionException(connectionString, providerName);

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
