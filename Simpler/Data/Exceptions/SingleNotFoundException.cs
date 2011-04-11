using System;

namespace Simpler.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when something other than a single record is returned by FetchSingleOf.
    /// </summary>
    public class SingleNotFoundException : Exception
    {
        public SingleNotFoundException(string message) : base(message) { }
    }
}
