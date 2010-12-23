using System;

namespace Simpler.Data.Exceptions
{
    public class SingleNotFoundException : Exception
    {
        public SingleNotFoundException(string message) : base(message) { }
    }
}
