using NUnit.Framework;
using Simpler.Data.Tasks;
using System.Data;
using Moq;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildTypedTest
    {
        [Test]
        public void should_build_an_instance_of_given_type()
        {
            // Arrange
            var task = Task.New<BuildTyped<MockPerson>>();

            var mockDataRecord = new Mock<IDataRecord>();
            task.In.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object, Is.InstanceOf(typeof(MockPerson)));
        }

        [Test]
        public void should_populate_typed_object_using_all_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.New<BuildTyped<MockPerson>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);
            task.In.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Object.Age, Is.EqualTo(21));
        }

        [Test]
        public void should_throw_exception_if_a_data_record_column_is_not_a_property_of_the_object_class()
        {
            // Arrange
            var task = Task.New<BuildTyped<MockPerson>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
            mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");
            task.In.DataRecord = mockDataRecord.Object;

            // Act & Assert
            Assert.Throws(typeof(CheckException), task.Execute);
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.New<BuildTyped<MockPerson>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");

            task.In.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Object.Age, Is.Null);
        }
    }
}