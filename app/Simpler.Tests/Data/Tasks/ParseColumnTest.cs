using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Data.PropertyMappingTree;
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
            task.In.RootNode = new ObjectNode
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
            task.In.RootNode = new ObjectNode
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
            task.In.RootNode = new ObjectNode
                {
                    PropertyType = typeof (MockPerson)
                };
            task.In.ColumnName = "Name";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.RootNode["Name"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.RootNode["Name"], Is.TypeOf(typeof(ObjectNode)));
        }

        [Test]
        public void should_parse_column_names_with_nested_objects()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.RootNode = new ObjectNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "PetName";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.RootNode["Pet"], Is.Not.Null);
            Assert.That(task.In.RootNode["Pet"]["Name"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.RootNode["Pet"], Is.TypeOf(typeof(ObjectNode)));
            Assert.That(task.In.RootNode["Pet"]["Name"], Is.TypeOf(typeof(ObjectNode)));
        }

        [Test]
        public void should_parse_column_names_with_arrays()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.RootNode = new ObjectNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "Vehicles0Make";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.RootNode["Vehicles"], Is.Not.Null);
            Assert.That(task.In.RootNode["Vehicles"]["0"], Is.Not.Null);
            Assert.That(task.In.RootNode["Vehicles"]["0"]["Make"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.RootNode["Vehicles"], Is.TypeOf(typeof(ArrayNode)));
            Assert.That(task.In.RootNode["Vehicles"]["0"], Is.TypeOf(typeof(ArrayElementNode)));
            Assert.That(task.In.RootNode["Vehicles"]["0"]["Make"], Is.TypeOf(typeof(ObjectNode)));
        }

        [Test]
        public void should_parse_column_names_that_are_dynamic()
        {
            //Arrange
            var task = Task.New<ParseColumn>();
            task.In.RootNode = new ObjectNode
            {
                PropertyType = typeof(MockPerson)
            };
            task.In.ColumnName = "OtherTheLastManStanding";

            //Act
            task.Execute();

            //Assert
            Assert.That(task.In.RootNode["Other"], Is.Not.Null);
            Assert.That(task.In.RootNode["Other"]["TheLastManStanding"], Is.Not.Null);

            //Check the node types
            Assert.That(task.In.RootNode["Other"], Is.TypeOf(typeof(DynamicNode)));
            Assert.That(task.In.RootNode["Other"]["TheLastManStanding"], Is.TypeOf(typeof(DynamicPropertyNode)));
        }

    }
}