using NUnit.Framework;
using Simpler.Data.PropertyMappingTree;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class ObjectNodeTest
    {
        [Test]
        public void should_set_value_for_object_node()
        {
            //arrange
            var obj = new MockPerson();
            var propertyParseTreeObjectNode = new ObjectNode()
                {
                    Name = "Name",
                    PropertyInfo = obj.GetType().GetProperty("Name"),
                    PropertyType = typeof(string)
                };

            // Act
            propertyParseTreeObjectNode.SetValue(obj, "Richard");

            // Assert
            Assert.That(obj.Name, Is.EqualTo("Richard"));
        }

        [Test]
        public void should_get_value_for_object_node()
        {
            //arrange
            var obj = new MockPerson{ Name = "Richard" };
            var propertyParseTreeObjectNode = new ObjectNode()
                {
                    Name = "Name",
                    PropertyInfo = obj.GetType().GetProperty("Name"),
                    PropertyType = typeof(string)
                };

            // Act
            var value = propertyParseTreeObjectNode.GetValue(obj);

            // Assert
            Assert.That(value, Is.EqualTo("Richard"));
        }

        [Test]
        public void should_create_object_for_object_node()
        {
            //arrange
            var propertyParseTreeObjectNode = new ObjectNode()
            {
                Name = "Name",
                PropertyType = typeof(string)
            };

            // Act
            var value = propertyParseTreeObjectNode.CreateObject("Richard");

            // Assert
            Assert.That(value, Is.EqualTo("Richard"));
        }
    }
}