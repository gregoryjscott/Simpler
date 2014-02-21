using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildMappingsTest
    {
        [Test]
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildMappings>();
            task.In.ColumnNames = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 3}, {"Vechiles0Make", 4},
                    {"Vechiles0Model", 5}, {"Vechiles1Make", 6}, {"Vechiles1Model", 7}
                };
            task.In.RootType = typeof (MockComplexPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ObjectMapping.Count(), Is.EqualTo(4));
            Assert.That(task.Out.ObjectMapping["Pet"].Children.Count(), Is.EqualTo(2));
            Assert.That(task.Out.ObjectMapping["Vechiles"].Children.Count(), Is.EqualTo(2));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"].Children.Count(), Is.EqualTo(2));
        }

        [Test]
        public void should_return_an_type_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildMappings>();
            task.In.ColumnNames = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 3}, {"Vechiles0Make", 4},
                    {"Vechiles0Model", 5}, {"Vechiles1Make", 6}, {"Vechiles1Model", 7}
                };
            task.In.RootType = typeof(MockComplexPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ObjectMapping["Name"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(task.Out.ObjectMapping["Age"].PropertyType, Is.EqualTo(typeof(int?)));
            Assert.That(task.Out.ObjectMapping["Pet"]["Name"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(task.Out.ObjectMapping["Pet"]["Age"].PropertyType, Is.EqualTo(typeof(int?)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"]["Model"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"]["Make"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"]["Model"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"]["Make"].PropertyType, Is.EqualTo(typeof(string)));

            Assert.That(task.Out.ObjectMapping.PropertyType, Is.EqualTo(typeof(MockComplexPerson)));
            Assert.That(task.Out.ObjectMapping["Pet"].PropertyType, Is.EqualTo(typeof(MockPet)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"].PropertyType, Is.EqualTo(typeof(MockVechile)));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"].PropertyType, Is.EqualTo(typeof(MockVechile)));
        }

        [Test]
        public void should_return_contain_column_indexs_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildMappings>();
            task.In.ColumnNames = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 3}, {"Vechiles0Make", 4},
                    {"Vechiles0Model", 5}, {"Vechiles1Make", 6}, {"Vechiles1Model", 7}
                };
            task.In.RootType = typeof(MockComplexPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ObjectMapping["Name"].ColumnIndex, Is.EqualTo(0));
            Assert.That(task.Out.ObjectMapping["Age"].ColumnIndex, Is.EqualTo(1));
            Assert.That(task.Out.ObjectMapping["Pet"]["Name"].ColumnIndex, Is.EqualTo(2));
            Assert.That(task.Out.ObjectMapping["Pet"]["Age"].ColumnIndex, Is.EqualTo(3));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"]["Make"].ColumnIndex, Is.EqualTo(4));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"]["Model"].ColumnIndex, Is.EqualTo(5));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"]["Make"].ColumnIndex, Is.EqualTo(6));
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"]["Model"].ColumnIndex, Is.EqualTo(7));

            Assert.That(task.Out.ObjectMapping["Pet"].ColumnIndex, Is.Null);
            Assert.That(task.Out.ObjectMapping["Vechiles"].ColumnIndex, Is.Null);
            Assert.That(task.Out.ObjectMapping["Vechiles"]["0"].ColumnIndex, Is.Null);
            Assert.That(task.Out.ObjectMapping["Vechiles"]["1"].ColumnIndex, Is.Null);
        }

        [Test]
        public void should_return_property_info_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildMappings>();
            task.In.ColumnNames = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 3}, {"Vechiles0Make", 4},
                    {"Vechiles0Model", 5}, {"Vechiles1Make", 6}, {"Vechiles1Model", 7}
                };
            task.In.RootType = typeof(MockComplexPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(((ObjectMappingObjectNode)task.Out.ObjectMapping["Name"]).PropertyInfo, Is.EqualTo(typeof(MockComplexPerson).GetProperty("Name")));
            Assert.That(((ObjectMappingObjectNode)task.Out.ObjectMapping["Age"]).PropertyInfo, Is.EqualTo(typeof(MockComplexPerson).GetProperty("Age")));
            Assert.That(((ObjectMappingObjectNode)task.Out.ObjectMapping["Pet"]["Age"]).PropertyInfo, Is.EqualTo(typeof(MockPet).GetProperty("Age")));
            Assert.That(((ObjectMappingObjectNode)task.Out.ObjectMapping["Pet"]["Name"]).PropertyInfo, Is.EqualTo(typeof(MockPet).GetProperty("Name")));
        }
    }
}