using System;
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
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            var providerName = ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);
            
            var connection = provider.CreateConnection();
            Check.That(connection != null, 
                "Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", connectionName, providerName);

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }

        public static T[] GetMany<T>(IDbConnection connection, string sql, object values = null)
        {
            var many = new T[] {};

            Action<IDbCommand> action =
                command =>
                {
                    var fetchMany = Task.New<FetchMany<T>>();
                    fetchMany.In.SelectCommand = command;
                    fetchMany.Execute();
                    many = fetchMany.Out.ObjectsFetched;
                };

            var execute = Task.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action = action;
            execute.Execute();

            return many;
        }

        public static T GetOne<T>(IDbConnection connection, string sql, object values = null)
        {
            var one = default(T);

            Action<IDbCommand> action =
                command =>
                    {
                        var fetchMany = Task.New<FetchMany<T>>();
                        fetchMany.In.SelectCommand = command;
                        fetchMany.Execute();
                        one = fetchMany.Out.ObjectsFetched.Single();
                    };

            var execute = Task.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action = action;
            execute.Execute();

            return one;
        }

        public static int GetResult(IDbConnection connection, string sql, object values = null)
        {
            var result = default(int);

            Action<IDbCommand> action =
                command =>
                    {
                        result = command.ExecuteNonQuery();
                    };

            var execute = Task.New<ExecuteAction>();
            execute.In.Connection = connection;
            execute.In.Sql = sql;
            execute.In.Values = values;
            execute.In.Action = action;
            execute.Execute();

            return result;
        }

        public static object GetScalar(IDbConnection connection, string sql, object values = null)
        {
            var scalar = default(object);

            Action<IDbCommand> action =
                command =>
                {
                    scalar = command.ExecuteScalar();
                };

            var executeAction = Task.New<ExecuteAction>();
            executeAction.In.Connection = connection;
            executeAction.In.Sql = sql;
            executeAction.In.Values = values;
            executeAction.In.Action = action;
            executeAction.Execute();

            return scalar;
        }
    }
}
