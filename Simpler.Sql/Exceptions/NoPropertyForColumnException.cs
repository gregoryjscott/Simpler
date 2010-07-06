using System;

namespace Simpler.Sql.Exceptions
{
    public class NoPropertyForColumnException : Exception
    {
        public NoPropertyForColumnException(string message) : base(message) { }
    }
}
