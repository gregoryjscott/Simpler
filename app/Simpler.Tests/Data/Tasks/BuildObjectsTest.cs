using System;
using System.Linq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Moq;
using System.Data;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildObjectsTest
    {
        static IDataReader SetupReader()
        {
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });
            return table.CreateDataReader();
        }

        [Test]
        public void should_return_an_object_for_each_record()
        {
            // Arrange
            var task = Task.New<BuildObjects<MockPerson>>();

            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Columns.Add("PetName", Type.GetType("System.String"));
            table.Columns.Add("Vehicles0Make", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "John Doe", "21", "Doug", "Dodge" });
            table.Rows.Add(new object[] { "Jane Doe", "19", "Spot", "Jeep" });

            task.In.Reader = table.CreateDataReader();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Objects.Count(), Is.EqualTo(2));
            Assert.That(task.Out.Objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Objects[0].Age, Is.EqualTo(21));
            Assert.That(task.Out.Objects[0].Pet.Name, Is.EqualTo("Doug"));
            Assert.That(task.Out.Objects[0].Vehicles[0].Make, Is.EqualTo("Dodge"));
            Assert.That(task.Out.Objects[1].Name, Is.EqualTo("Jane Doe"));
            Assert.That(task.Out.Objects[1].Age, Is.EqualTo(19));
            Assert.That(task.Out.Objects[1].Pet.Name, Is.EqualTo("Spot"));
            Assert.That(task.Out.Objects[1].Vehicles[0].Make, Is.EqualTo("Jeep"));
        }

    }
}