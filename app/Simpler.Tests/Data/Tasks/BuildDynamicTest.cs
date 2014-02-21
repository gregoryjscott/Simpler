using System.Collections.Generic;
using NUnit.Framework;
using Simpler.Data.Tasks;
using System.Data;
using Moq;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildDynamicTest
    {
        [Test]
        public void should_populate_dynamic_object_using_all_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.New<BuildObject<dynamic>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(0)).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(1)).Returns(21);
            task.In.DataRecord = mockDataRecord.Object;
            var mapTask = Task.New<BuildMappings>();
            mapTask.In.ColumnNames = new Dictionary<string, int> { { "Name", 0 }, { "Age", 1 } };
            mapTask.Execute();
            task.In.ObjectMapping = mapTask.Out.ObjectMapping;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Object.Age, Is.EqualTo(21));
        }

        [Test]
        public void should_populate_nested_dynamic_objects_using_all_columns_in_the_data_record()
        {
            // Arrange
            var task = Task.New<BuildObject<MockPerson>>();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(3);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(0)).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(1)).Returns(21);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(2)).Returns("StuffCake");
            mockDataRecord.Setup(dataRecord => dataRecord.GetValue(2)).Returns("TheCakeIsALie");
            task.In.DataRecord = mockDataRecord.Object;
            var mapTask = Task.New<BuildMappings>();
            mapTask.In.RootType = typeof (MockPerson);
            mapTask.In.ColumnNames = new Dictionary<string, int> { { "Name", 0 }, { "Age", 1 },  {"StuffCake", 2 } };
            mapTask.Execute();
            task.In.ObjectMapping = mapTask.Out.ObjectMapping;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Object.Stuff.Cake, Is.EqualTo("TheCakeIsALie"));
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Object.Age, Is.EqualTo(21));
        }

    }
}