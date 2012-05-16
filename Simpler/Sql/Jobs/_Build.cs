using System;
using System.Data;
using Moq;
using NUnit.Framework;
using Simpler.Sql.Exceptions;
using Simpler._Mocks;

namespace Simpler.Sql.Jobs
{
    public class _Build<T> : InOutJob<_Build<T>.Input, _Build<T>.Output> 
    {
        public class Input
        {
            public virtual IDataRecord DataRecord { get; set; }
        }

        public class Output
        {
            public virtual T Object { get; set; }
        }

        public override void Run()
        {
            Out.Object = (T) Activator.CreateInstance(typeof (T));
            var objectType = typeof(T);

            for (var i = 0; i < In.DataRecord.FieldCount; i++)
            {
                var columnName = In.DataRecord.GetName(i);
                var propertyInfo = objectType.GetProperty(columnName);

                if (propertyInfo == null)
                {
                    throw new NoPropertyForColumnException(columnName, objectType.FullName);
                }

                var columnValue = In.DataRecord[columnName];
                if (columnValue.GetType() != typeof(DBNull))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    }

                    columnValue = Convert.ChangeType(columnValue, propertyType);
                    propertyInfo.SetValue(Out.Object, columnValue, null);
                }
            }
        }

        public override void Test()
        {
            Test<_Build<MockObject>>.Should(
                "create an instance of given object type",
                job =>
                {
                    job.In.DataRecord = new Mock<IDataRecord>().Object;
                    job.Run();
                    var newObject = job.Out.Object;

                    Assert.That(newObject, Is.InstanceOf(typeof (MockObject)));
                });

            Test<_Build<MockObject>>.Should(
                "populate object using all columns in the data record",
                job =>
                {
                    var mockDataRecord = new Mock<IDataRecord>();
                    mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
                    mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
                    mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
                    mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
                    mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);

                    job.In.DataRecord = mockDataRecord.Object;
                    job.Run();
                    var newObject = job.Out.Object;

                    Assert.That(newObject.Name, Is.EqualTo("John Doe"));
                    Assert.That(newObject.Age, Is.EqualTo(21));
                });

            Test<_Build<MockObject>>.Should(
                "throw exception if a data record column is not a property of the object class",
                job =>
                {
                    var mockDataRecord = new Mock<IDataRecord>();
                    mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                    mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
                    mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");

                    job.In.DataRecord = mockDataRecord.Object;

                    Assert.Throws(typeof(NoPropertyForColumnException), job.Run);
                });

            Test<_Build<MockObject>>.Should(
                "allow object to have properties w/o matching columns in record",
                job =>
                {
                    var mockDataRecord = new Mock<IDataRecord>();
                    mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                    mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
                    mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

                    job.In.DataRecord = mockDataRecord.Object;
                    job.Run();
                    var newObject = job.Out.Object;

                    Assert.That(newObject.Name, Is.EqualTo("John Doe"));
                    Assert.That(newObject.Age, Is.Null);
                });
        }
    }
}
