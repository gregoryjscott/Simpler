using System;
using System.Data;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Jobs
{
    public class _Build<T> : InOutJob<_Build<T>.In, _Build<T>.Out> 
    {
        public class In
        {
            public virtual IDataRecord DataRecord { get; set; }
        }

        public class Out
        {
            public virtual T Object { get; set; }
        }

        public override void Run()
        {
            _Out = new Out {Object = (T) Activator.CreateInstance(typeof (T))};
            var objectType = typeof(T);

            for (var i = 0; i < _In.DataRecord.FieldCount; i++)
            {
                var columnName = _In.DataRecord.GetName(i);
                var propertyInfo = objectType.GetProperty(columnName);

                if (propertyInfo == null)
                {
                    throw new NoPropertyForColumnException(columnName, objectType.FullName);
                }

                var columnValue = _In.DataRecord[columnName];
                if (columnValue.GetType() != typeof(System.DBNull))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    }

                    columnValue = Convert.ChangeType(columnValue, propertyType);
                    propertyInfo.SetValue(_Out.Object, columnValue, null);
                }
            }
        }

        //public override void Test()
        //{
        //    Test<_Build<MockObject>>.Should(
        //        "allow object to have properties w/o matching columns in record",
        //        job =>
        //        {
        //            var mockDataRecord = new Mock<IDataRecord>();
        //            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
        //            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
        //            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

        //            var newObject = job
        //                .Set(new _Build<MockObject>.In { DataRecord = mockDataRecord.Object })
        //                .Get().Object;

        //            Assert.That(newObject.Name, Is.EqualTo("John Doe"));
        //            Assert.That(newObject.Age, Is.Null);
        //        });
        //}
    }
}
