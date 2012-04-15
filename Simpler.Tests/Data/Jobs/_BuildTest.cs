using NUnit.Framework;
using System.Data;
using Moq;
using Simpler.Sql.Exceptions;
using Simpler.Sql.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class _BuildTest
    {
        [Test]
        public void should_create_an_instance_of_given_object_type()
        {
            var mockDataRecord = new Mock<IDataRecord>();

            Test<_Build<MockObject>>.New()
                .Arrange(job => job.Set(new _Build<MockObject>.In {DataRecord = mockDataRecord.Object}))
                .Act()
                .Assert(job => Assert.That(job._Out.Object, Is.InstanceOf(typeof(MockObject))));
        }

        [Test]
        public void should_populate_object_using_all_columns_in_the_data_record()
        {
            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);

            Test<_Build<MockObject>>.New()
                .Arrange(job => job.Set(new _Build<MockObject>.In { DataRecord = mockDataRecord.Object }))
                .Act()
                .Assert(
                    job =>
                    {
                        Assert.That(job._Out.Object.Name, Is.EqualTo("John Doe"));
                        Assert.That(job._Out.Object.Age, Is.EqualTo(21));
                    });
        }

        [Test]
        public void should_throw_exception_if_a_data_record_column_is_not_a_property_of_the_object_class()
        {
            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
            mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");

            Test<_Build<MockObject>>.New()
                .Arrange(job => job.Set(new _Build<MockObject>.In { DataRecord = mockDataRecord.Object }))
                .Assert(job => Assert.Throws(typeof(NoPropertyForColumnException), job.Run));
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            //var mockDataRecord = new Mock<IDataRecord>();
            //mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            //mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            //mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

            //Test<_Build<MockObject>>.New()
            //    .Arrange(job => job.Set(new _Build<MockObject>.In { DataRecord = mockDataRecord.Object }))
            //    .Act()
            //    .Assert(
            //        job =>
            //        {
            //            Assert.That(job._Out.Object.Name, Is.EqualTo("John Doe"));
            //            Assert.That(job._Out.Object.Age, Is.Null);
            //        });

            Test<_Build<MockObject>>.Should(
                "allow object to have properties w/o matching columns in record",
                job =>
                {
                    var mockDataRecord = new Mock<IDataRecord>();
                    mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                    mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
                    mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

                    var newObject = job
                        .Set(new _Build<MockObject>.In {DataRecord = mockDataRecord.Object})
                        .Get().Object;

                    Assert.That(newObject.Name, Is.EqualTo("John Doe"));
                    Assert.That(newObject.Age, Is.Null);
                });
        }
    }
}
