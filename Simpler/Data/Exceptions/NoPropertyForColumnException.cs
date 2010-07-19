using System;

namespace Simpler.Data.Exceptions
{
    public class NoPropertyForColumnException : Exception
    {
        public NoPropertyForColumnException(string message) : base(message) { }
    }
}
