using System;

namespace Simpler.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when something unexpected happens while creating a database connection.
    /// </summary>
    public class CreateConnectionException : Exception
    {
        public CreateConnectionException(string connectionName, string providerName)
            : base(String.Format("Error while trying to create a DbProviderFactory connection using a connectionString setting with a name of {0}, with a provider type of {1}.", connectionName, providerName)) { }
    }
}
