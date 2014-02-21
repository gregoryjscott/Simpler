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
    public class BuildComplexTypedTest
    {
        static IDataReader SetupReader()
        {
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Columns.Add("PetName", Type.GetType("System.String"));
            table.Columns.Add("PetAge", Type.GetType("System.Int32"));
            table.Columns.Add("Vechiles0Make", Type.GetType("System.String"));
            table.Columns.Add("Vechiles0Model", Type.GetType("System.String"));
            table.Columns.Add("Vechiles1Make", Type.GetType("System.String"));
            table.Columns.Add("Vechiles1Model", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "John Doe", "21", "Fiddo", "2", "Dodge", "Durango", "Dodge", "Durango" });
            table.Rows.Add(new object[] { "Jane Doe", "19", "Spot", "3", "Jeep", "Wrangler", "Jeep", "Wrangler" });
            return table.CreateDataReader();
        }

        [Test]
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildObjects<MockComplexPerson>>();
            task.In.Reader = SetupReader();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Objects.Count(), Is.EqualTo(2));
            Assert.That(task.Out.Objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Objects[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void should_return_nested_complex_objects()
        {
            // Arrange
            var task = Task.New<BuildObjects<MockComplexPerson>>();
            task.In.Reader = SetupReader();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Objects.Count(), Is.EqualTo(2));
            Assert.That(task.Out.Objects[0].Pet.Name, Is.EqualTo("Fiddo"));
            Assert.That(task.Out.Objects[1].Pet.Name, Is.EqualTo("Spot"));
            Assert.That(task.Out.Objects[0].Vechiles[0].Make, Is.EqualTo("Dodge"));
            Assert.That(task.Out.Objects[1].Vechiles[1].Make, Is.EqualTo("Jeep"));
        }
    }
}