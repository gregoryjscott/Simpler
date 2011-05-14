using System;

namespace Simpler.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when something unexpected occurs when fetching data.
    /// </summary>
    public class FetchException : Exception
    {
        public FetchException(string message) : base(message) { }
    }
}
