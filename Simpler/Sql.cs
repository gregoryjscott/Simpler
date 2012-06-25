using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Data.Jobs;

namespace Simpler
{
    public class Sql
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

        public static T[] ReturnMany<T>(string sql, object values = null, string connectionName = null)
        {
            var returnMany = Job.New<ReturnMany<T>>();
            returnMany.In.Sql = sql;
            returnMany.In.Values = values;
            returnMany.In.ConnectionName = connectionName;
            returnMany.Run();

            return returnMany.Out.Models;
        }

        public static T ReturnOne<T>(string sql, object values = null, string connectionName = null)
        {
            var returnOne = Job.New<ReturnOne<T>>();
            returnOne.In.Sql = sql;
            returnOne.In.Values = values;
            returnOne.In.ConnectionName = connectionName;
            returnOne.Run();

            return returnOne.Out.Model;
        }
    }
}
