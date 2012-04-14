using System;

namespace Simpler.Sql.Exceptions
{
    /// <summary>
    /// Exception thrown when something unexpected happens while persisting an object to the database.
    /// </summary>
    public class ObjectPersistanceException : Exception
    {
        public ObjectPersistanceException(string message) : base(message) { }
    }
}
