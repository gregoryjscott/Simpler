using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Tasks
{
    public class UseDataRecordToBuild<T> : Task
    {
        // Inputs
        public IDataRecord DataRecord { get; set; }

        // Outputs
        public T Object { get; private set; }

        public override void Execute()
        {
            Object = (T)Activator.CreateInstance(typeof(T));
            Type objectType = typeof(T);

            for (var i = 0; i < DataRecord.FieldCount; i++)
            {
                string columnName = DataRecord.GetName(i);
                PropertyInfo propertyInfo = objectType.GetProperty(columnName);

                if (propertyInfo == null)
                {
                    throw new NoPropertyForColumnException(String.Format("The DataRecord contains column '{0}' that is not a property of the '{1}' class.", columnName, objectType.FullName));
                }

                object columnValue = DataRecord[columnName];
                if (columnValue.GetType() != typeof(System.DBNull))
                {
                    Type propertyType = propertyInfo.PropertyType;

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
