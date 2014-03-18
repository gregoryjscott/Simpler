using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Data.PropertyParseTree;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class ParseColumnTest
    {
        [Test]
        public void should_throw_exception_if_column_name_does_not_match_any_properties()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "TheCakeIsALie";

            //Act
            Assert.Throws<CheckException>(() => task.Execute());
        }

        [Test]
        public void should_throw_exception_if_column_name_is_only_a_partial_match()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "PetTheCakeIsALie";

            //Act
            Assert.Throws<CheckException>(() => task.Execute());
        }

        [Test]
        public void should_parse_column_names()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
                {
                    PropertyType = typeof (MockPerson)
                };
            task.In.ColumnName = "Name";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.PropertyParseTree["Name"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.PropertyParseTree["Name"], Is.TypeOf(typeof(PropertyParseTreeObjectNode)));
        }

        [Test]
        public void should_parse_column_names_with_nested_objects()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "PetName";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.PropertyParseTree["Pet"], Is.Not.Null);
            Assert.That(task.In.PropertyParseTree["Pet"]["Name"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.PropertyParseTree["Pet"], Is.TypeOf(typeof(PropertyParseTreeObjectNode)));
            Assert.That(task.In.PropertyParseTree["Pet"]["Name"], Is.TypeOf(typeof(PropertyParseTreeObjectNode)));
        }

        [Test]
        public void should_parse_column_names_with_arrays()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "Vehicles0Make";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.PropertyParseTree["Vehicles"], Is.Not.Null);
            Assert.That(task.In.PropertyParseTree["Vehicles"]["0"], Is.Not.Null);
            Assert.That(task.In.PropertyParseTree["Vehicles"]["0"]["Make"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.PropertyParseTree["Vehicles"], Is.TypeOf(typeof(PropertyParseTreeArrayNode)));
            Assert.That(task.In.PropertyParseTree["Vehicles"]["0"], Is.TypeOf(typeof(PropertyParseTreeArrayChildNode)));
            Assert.That(task.In.PropertyParseTree["Vehicles"]["0"]["Make"], Is.TypeOf(typeof(PropertyParseTreeObjectNode)));
        }

        [Test]
        public void should_parse_column_names_that_are_dynamic()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.PropertyParseTree = new PropertyParseTreeRootNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "OtherTheLastManStanding";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.PropertyParseTree["Other"], Is.Not.Null);
            Assert.That(task.In.PropertyParseTree["Other"]["TheLastManStanding"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.PropertyParseTree["Other"], Is.TypeOf(typeof(PropertyParseTreeDynamicNode)));
            Assert.That(task.In.PropertyParseTree["Other"]["TheLastManStanding"], Is.TypeOf(typeof(PropertyParseTreeDynamicChildNode)));
        }

    }
}