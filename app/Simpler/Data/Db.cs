using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    public static class Db
    {
        public static IDbConnection Connect(string connectionName)
        {
            var connectionStringConfig = ConfigurationManager.ConnectionStrings[connectionName];
            Check.That(connectionStringConfig != null, "A connectionString with name {0} was not found in the configuration file.", connectionName);

            var connectionString = connectionStringConfig.ConnectionString;
            var providerName = connectionStringConfig.ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);
            
            var connection = provider.CreateConnection();
            Check.That(connection != null, 
                "Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", connectionName, providerName);

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }

        public static T[] GetMany<T>(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var many = new T[] {};

            var execute = Simpler.T.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action =
                command =>
                    {
                        command.CommandTimeout = timeout;

                        var fetchMany = Simpler.T.New<FetchMany<T>>();
                        fetchMany.In.SelectCommand = command;
                        fetchMany.Execute();
                        many = fetchMany.Out.ObjectsFetched;
                    };
            execute.Execute();

            return many;
        }

        public static T GetOne<T>(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var one = default(T);

            var execute = Simpler.T.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action =
                command =>
                    {
                        command.CommandTimeout = timeout;

                        var fetchMany = Simpler.T.New<FetchMany<T>>();
                        fetchMany.In.SelectCommand = command;
                        fetchMany.Execute();
                        one = fetchMany.Out.ObjectsFetched.Single();
                    };
            execute.Execute();

            return one;
        }

        public static int GetResult(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var result = default(int);

            var execute = T.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action =
                command =>
                    {
                        command.CommandTimeout = timeout;

                        result = command.ExecuteNonQuery();
                    };
            execute.Execute();

            return result;
        }

        public static object GetScalar(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            var scalar = default(object);

            var execute = T.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action =
                command =>
                    {
                        command.CommandTimeout = timeout;

                        scalar = command.ExecuteScalar();
                    };
            execute.Execute();

            return scalar;
        }
    }
}
