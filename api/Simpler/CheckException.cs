using System;

namespace Simpler
{
    public class CheckException : Exception
    {
        public CheckException(string message) : base(message) {}
    }
}
