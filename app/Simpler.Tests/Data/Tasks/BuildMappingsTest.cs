using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 2}, {"Vechiles0Make", 2},
                    {"Vechiles0Model", 2}, {"Vechiles1Make", 2}, {"Vechiles1Model", 2}
                };
            task.In.RootType = typeof (MockComplexPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ObjectMapping.Count(), Is.EqualTo(4));
            Assert.That(task.Out.ObjectMapping["Pet"].Children.Count(), Is.EqualTo(2));
            Assert.That(task.Out.ObjectMapping["Vechiles"].Children.Count(), Is.EqualTo(2));
            Assert.That(task.Out.ObjectMapping["Vechiles"].Children["0"].Children.Count(), Is.EqualTo(2));
        }
    }
}