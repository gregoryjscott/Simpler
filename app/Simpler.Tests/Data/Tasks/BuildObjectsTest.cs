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
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = Task.New<BuildObjects<MockPerson>>();
            task.In.Reader = SetupReader();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Objects.Count(), Is.EqualTo(2));
            Assert.That(task.Out.Objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.Objects[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void should_build_typed_objects_if_given_strong_type()
        {
            // Arrange
            var task = Task.New<BuildObjects<MockPerson>>();
            task.In.Reader = SetupReader();

            task.BuildTyped = Fake.Task<BuildTyped<MockPerson>>(bt => bt.Out.Object = new MockPerson());
            task.BuildDynamic = Fake.Task<BuildDynamic>();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.BuildTyped.Stats.ExecuteCount, Is.GreaterThan(0));
            Assert.That(task.BuildDynamic.Stats.ExecuteCount, Is.EqualTo(0));
        }

        [Test]
        public void should_build_dynamic_objects_if_given_dynamic_type()
        {
            // Arrange
            var task = Task.New<BuildObjects<dynamic>>();
            task.In.Reader = SetupReader();

            task.BuildTyped = Fake.Task<BuildTyped<dynamic>>();
            task.BuildDynamic = Fake.Task<BuildDynamic>(bd => bd.Out.Object = new MockPerson());

            // Act
            task.Execute();

            // Assert
            Assert.That(task.BuildTyped.Stats.ExecuteCount, Is.EqualTo(0));
            Assert.That(task.BuildDynamic.Stats.ExecuteCount, Is.GreaterThan(0));
        }
    }
}