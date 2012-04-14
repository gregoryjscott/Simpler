using System;

namespace Simpler.Sql.Exceptions
{
    /// <summary>
    /// Exception thrown when a mapping doesn't make sense.
    /// </summary>
    public class NoPropertyForColumnException : Exception
    {
        public NoPropertyForColumnException(string columnName, string className)
            : base(String.Format("The DataRecord contains column '{0}' that is not a property of the '{1}' class.", columnName, className)) { }
    }
}
