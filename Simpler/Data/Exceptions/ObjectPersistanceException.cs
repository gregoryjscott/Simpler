using System;

namespace Simpler.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when something unexpected happens while persisting an object to the database.
    /// </summary>
    public class ObjectPersistanceException : Exception
    {
        public ObjectPersistanceException(string message) : base(message) { }
    }
}
