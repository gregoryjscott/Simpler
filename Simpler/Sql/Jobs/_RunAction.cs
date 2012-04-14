using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Jobs
{
    public class _RunAction : InJob<_RunAction.In>
    {
        public class In
        {
            public string ConnectionName { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
            public Action<IDbCommand> Action { get; set; }
        }

        public _BuildParameters BuildParameters { get; set; }

        public override void Run()
        {
            if (String.IsNullOrEmpty(_In.ConnectionName)) throw new ArgumentException("ConnectionName property must be set.");
            if (String.IsNullOrEmpty(_In.Sql)) throw new ArgumentException("Sql property must be set.");

            var connectionString = ConfigurationManager.ConnectionStrings[_In.ConnectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[_In.ConnectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = provider.CreateConnection())
            {
                if (connection == null) throw new CreateConnectionException(connectionString, providerName);

                connection.ConnectionString = connectionString;

                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = _In.Sql;

                    if (_In.Values != null)
                    {
                        BuildParameters.Command = command;
                        BuildParameters.Values = _In.Values;
                        BuildParameters.Run();                        
                    }

                    _In.Action(command);
                }
            }
        }
    }
}
