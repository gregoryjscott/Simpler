using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildPropertyParseTreeTest
    {
        [Test]
        public void should_build_a_tree_for_primitives()
        {
            // Arrange
            var task = Task.New<BuildPropertyParseTree>();
            task.In.Columns = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }
                };
            task.In.InitialType = typeof(MockPerson);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.PropertyParseTree.Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Name"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Age"], Is.Not.Null);
        }

        [Test]
        public void should_build_a_tree_for_nested_objects()
        {
            // Arrange
            var task = Task.New<BuildPropertyParseTree>();
            task.In.Columns = new Dictionary<string, int>
                {
                    {"PetName", 0}, {"PetAge", 1}
                };
            task.In.InitialType = typeof(MockPerson);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.PropertyParseTree.Nodes.Count(), Is.EqualTo(1));
            Assert.That(task.Out.PropertyParseTree["Pet"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Pet"]["Name"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Pet"]["Age"], Is.Not.Null);
        }

        [Test]
        public void should_build_a_tree_for_arrays()
        {
            // Arrange
            var task = Task.New<BuildPropertyParseTree>();
            task.In.Columns = new Dictionary<string, int>
                {
                    {"Vehicles0Make", 0}, {"Vehicles0Model", 1}, {"Vehicles1Make", 2}, {"Vehicles1Model", 3}
                };
            task.In.InitialType = typeof(MockPerson);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.PropertyParseTree.Nodes.Count(), Is.EqualTo(1));
            Assert.That(task.Out.PropertyParseTree["Vehicles"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"]["Make"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"]["Model"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"]["Make"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"]["Model"], Is.Not.Null);
        }

        [Test]
        public void should_build_a_tree_for_dynamics()
        {
            // Arrange
            var task = Task.New<BuildPropertyParseTree>();
            task.In.Columns = new Dictionary<string, int>
                {
                    {"OtherCity", 0}, {"OtherState", 1}
                };
            task.In.InitialType = typeof(MockPerson);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.PropertyParseTree.Nodes.Count(), Is.EqualTo(1));
            Assert.That(task.Out.PropertyParseTree["Other"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Other"]["State"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Other"]["City"], Is.Not.Null);
        }

        [Test]
        public void should_build_a_tree()
        {
            // Arrange
            var task = Task.New<BuildPropertyParseTree>();
            task.In.Columns = new Dictionary<string, int>
                {
                    { "Name", 0 }, { "Age", 1 }, {"PetName" , 2}, {"PetAge", 3}, {"Vehicles0Make", 4},
                    {"Vehicles0Model", 5}, {"Vehicles1Make", 6}, {"Vehicles1Model", 7}, {"OtherCity", 8}
                };
            task.In.InitialType = typeof(MockPerson);
            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.PropertyParseTree.Nodes.Count(), Is.EqualTo(5));
            Assert.That(task.Out.PropertyParseTree["Name"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Age"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Pet"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Pet"]["Name"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Pet"]["Age"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"]["Make"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["0"]["Model"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"].Nodes.Count(), Is.EqualTo(2));
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"]["Make"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Vehicles"]["1"]["Model"], Is.Not.Null);
            Assert.That(task.Out.PropertyParseTree["Other"].Nodes.Count(), Is.EqualTo(1));
            Assert.That(task.Out.PropertyParseTree["Other"]["City"], Is.Not.Null);
        }
    }
}