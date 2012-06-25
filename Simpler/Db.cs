using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Data.Jobs;

namespace Simpler
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
                String.Format("Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", connectionName, providerName));

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }

        public static T[] ReturnMany<T>(IDbConnection connection, string sql, object values = null)
        {
            var returnMany = Job.New<ReturnMany<T>>();
            returnMany.In.Sql = sql;
            returnMany.In.Values = values;
            returnMany.In.Connection = connection;
            returnMany.Run();

            return returnMany.Out.Models;
        }

        public static T ReturnOne<T>(IDbConnection connection, string sql, object values = null)
        {
            var returnOne = Job.New<ReturnOne<T>>();
            returnOne.In.Sql = sql;
            returnOne.In.Values = values;
            returnOne.In.Connection = connection;
            returnOne.Run();

            return returnOne.Out.Model;
        }

        public static int ReturnResult(IDbConnection connection, string sql, object values = null)
        {
            var returnResult = Job.New<ReturnResult>();
            returnResult.In.Sql = sql;
            returnResult.In.Values = values;
            returnResult.In.Connection = connection;
            returnResult.Run();

            return returnResult.Out.RowsAffected;
        }

        public static object ReturnScalar(IDbConnection connection, string sql, object values = null)
        {
            var returnScalar = Job.New<ReturnScalar>();
            returnScalar.In.Sql = sql;
            returnScalar.In.Values = values;
            returnScalar.In.Connection = connection;
            returnScalar.Run();

            return returnScalar.Out.Object;
        }
    }
}
