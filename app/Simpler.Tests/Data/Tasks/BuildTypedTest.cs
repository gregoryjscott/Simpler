using System.Collections.Generic;
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
            var task = Task.New<BuildObject<MockPerson>>();
            var mapTask = Task.New<BuildPropertyParseTree>();
            mapTask.In.InitialType = typeof(MockPerson);
            mapTask.In.Columns = new Dictionary<string, int> { { "Name", 0 } };
            mapTask.Execute();
            task.In.PropertyParseTree = mapTask.Out.PropertyParseTree;

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
            var task = Task.New<BuildObject<MockPerson>>();
            var mapTask = Task.New<BuildPropertyParseTree>();
            mapTask.In.InitialType = typeof(MockPerson);
            mapTask.In.Columns = new Dictionary<string, int> { { "Name", 0 }, { "Age", 1 } };
            mapTask.Execute();
            task.In.PropertyParseTree = mapTask.Out.PropertyParseTree;

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(0)).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(1)).Returns(21);
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
            var mapTask = Task.New<BuildPropertyParseTree>();
            mapTask.In.InitialType = typeof(MockPerson);
            mapTask.In.Columns = new Dictionary<string, int> { { "Name", 0 }, { "TheCakeIsALie", 1 } };

            // Act & Assert
            Assert.Throws(typeof(CheckException), mapTask.Execute);
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.New<BuildObject<MockPerson>>();
            var mapTask = Task.New<BuildPropertyParseTree>();
            mapTask.In.InitialType = typeof(MockPerson);
            mapTask.In.Columns = new Dictionary<string, int> { { "Name", 0 } };
            mapTask.Execute();
            task.In.PropertyParseTree = mapTask.Out.PropertyParseTree;

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(0)).Returns("John Doe");

            task.In.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Object.Age, Is.Null);
        }

        [Test]
        public void should_build_enum_properties()
        {
            // Arrange
            var task = Task.New<BuildObject<MockPerson>>();
            var mapTask = Task.New<BuildPropertyParseTree>();
            mapTask.In.InitialType = typeof(MockPerson);
            mapTask.In.Columns = new Dictionary<string, int> { { "MockEnum", 0 } };
            mapTask.Execute();
            task.In.PropertyParseTree = mapTask.Out.PropertyParseTree;

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("MockEnum");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(0)).Returns("One");
            task.In.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.MockEnum, Is.EqualTo(MockEnum.One));
        }
    }
}