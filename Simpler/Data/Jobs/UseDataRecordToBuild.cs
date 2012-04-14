using System;
using System.Data;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Jobs
{
    /// <summary>
    /// Job that builds an instance of the given type T using the values found in the given DataRecord.  If the DataRecord contains
    /// any columns that match the name of the property on T, then that column's value will be used to set the property.
    /// </summary>
    /// <typeparam name="T">The type of object to build.</typeparam>
    public class UseDataRecordToBuild<T> : Job
    {
        // Inputs
        public virtual IDataRecord DataRecord { get; set; }

        // Outputs
        public virtual T Object { get; private set; }

        public override void Execute()
        {
            Object = (T)Activator.CreateInstance(typeof(T));
            var objectType = typeof(T);

            for (var i = 0; i < DataRecord.FieldCount; i++)
            {
                var columnName = DataRecord.GetName(i);
                var propertyInfo = objectType.GetProperty(columnName);

                if (propertyInfo == null)
                {
                    throw new NoPropertyForColumnException(columnName, objectType.FullName);
                }

                var columnValue = DataRecord[columnName];
                if (columnValue.GetType() != typeof(System.DBNull))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    }

                    columnValue = Convert.ChangeType(columnValue, propertyType);
                    propertyInfo.SetValue(Object, columnValue, null);
                }
            }
        }
    }
}
