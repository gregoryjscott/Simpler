using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    public static class Db
    {
        public static IDbConnection Connect(string connectionName)
        {
            var connectionConfig = ConfigurationManager.ConnectionStrings[connectionName];
            Check.That(connectionConfig != null, "A connectionString with name {0} was not found in the configuration file.", connectionName);

            var connectionString = connectionConfig.ConnectionString;
            var providerName = connectionConfig.ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);

            var connection = provider.CreateConnection();
            Check.That(connection != null,
                "Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", connectionName, providerName);

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }

        public static T[] Get<T>(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var results = Get(connection, sql, values, timeout);
            return results.Read<T>();
        }

        public static Results Get(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            Results results = null;

            Core.ExecuteCommand(connection, sql, values, command => {
                command.CommandTimeout = timeout;
                var reader = command.ExecuteReader();
                results = new Results(reader);
            });

            return results;
        }

        public static int NonQuery(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var result = default(int);

            Core.ExecuteCommand(connection, sql, values, command => {
                command.CommandTimeout = timeout;
                result = command.ExecuteNonQuery();
            });

            return result;
        }

        public static object Scalar(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var scalar = default(object);

            Core.ExecuteCommand(connection, sql, values, command => {
                command.CommandTimeout = timeout;
                scalar = command.ExecuteScalar();
            });

            return scalar;
        }

        public class Core
        {
            public static string[] FindParameters(string sql)
            {
                var findParameters = Execute.Now<FindParameters>((fp => { fp.In.CommandText = sql; }));
                findParameters.Execute();
                return findParameters.Out.ParameterNames;
            }

            public static void ExecuteCommand(IDbConnection connection, string sql, object values, Action<IDbCommand> action)
            {
                using (var command = connection.CreateCommand())
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.Connection = connection;
                    command.CommandText = sql;

                    if (values != null)
                    {
                        var buildParameters = Task.New<BuildParameters>();
                        buildParameters.In.Command = command;
                        buildParameters.In.Values = values;
                        buildParameters.Execute();
                    }

                    action(command);
                }
            }
        }
    }
}
