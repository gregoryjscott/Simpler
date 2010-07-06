using System;

namespace Simpler.Sql.Exceptions
{
    public class ObjectPersistanceException : Exception
    {
        public ObjectPersistanceException(string message) : base(message) { }
    }
}
