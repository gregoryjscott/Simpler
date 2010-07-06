using System;

namespace Simpler.Data.Exceptions
{
    public class ObjectPersistanceException : Exception
    {
        public ObjectPersistanceException(string message) : base(message) { }
    }
}
