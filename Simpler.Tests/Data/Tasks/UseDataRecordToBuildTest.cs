using NUnit.Framework;
using Simpler.Data.Tasks;
using System.Data;
using Moq;
using Simpler.Data.Exceptions;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class UseDataRecordToBuildTest
    {
        [Test]
        public void should_create_an_instance_of_given_object_type()
        {
            // Arrange
            var task = Task.Create<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            task.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Object, Is.InstanceOf(typeof(MockObject)));
        }

        [Test]
        public void should_populate_object_using_all_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.Create<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);
            task.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Object.Age, Is.EqualTo(21));
        }

        [Test]
        public void should_throw_exception_if_a_data_record_column_is_not_a_property_of_the_object_class()
        {
            // Arrange
            var task = Task.Create<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
            mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");
            task.DataRecord = mockDataRecord.Object;

            // Act & Assert
            Assert.Throws(typeof(NoPropertyForColumnException), task.Execute);
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.Create<UseDataRecordToBuild<MockObject>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

            task.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Object.Age, Is.Null);
        }
    }
}
