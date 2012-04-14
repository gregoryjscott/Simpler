using NUnit.Framework;
using System.Data;
using Moq;
using Simpler.Sql.Exceptions;
using Simpler.Sql.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class UseDataRecordToBuildTest
    {
        [Test]
        public void should_create_an_instance_of_given_object_type()
        {
            // Arrange
            var job = Job.New<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            job.DataRecord = mockDataRecord.Object;

            // Act
            job.Run();

            // Assert
            Assert.That(job.Object, Is.InstanceOf(typeof(MockObject)));
        }

        [Test]
        public void should_populate_object_using_all_columns_in_the_data_record()
        {
            // Arrange
            var job = Job.New<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);
            job.DataRecord = mockDataRecord.Object;

            // Act
            job.Run();

            // Assert
            Assert.That(job.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(job.Object.Age, Is.EqualTo(21));
        }

        [Test]
        public void should_throw_exception_if_a_data_record_column_is_not_a_property_of_the_object_class()
        {
            // Arrange
            var job = Job.New<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
            mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");
            job.DataRecord = mockDataRecord.Object;

            // Act & Assert
            Assert.Throws(typeof(NoPropertyForColumnException), job.Run);
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            // Arrange
            var job = Job.New<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

            job.DataRecord = mockDataRecord.Object;

            // Act
            job.Run();

            // Assert
            Assert.That(job.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(job.Object.Age, Is.Null);
        }
    }
}
