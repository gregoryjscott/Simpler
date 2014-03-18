using NUnit.Framework;
using Simpler.Data.PropertyParseTree;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class PropertyParseTreeArrayChildNodeTest
    {
        [Test]
        public void should_set_value_for_array_child_node()
        {
            //arrange
            var array = new MockPet[1];
            var propertyParseTreeArrayChildNode = new PropertyParseTreeArrayChildNode
                {
                    Name = "0",
                    PropertyType = typeof (MockPet)
                };

            // Act
            propertyParseTreeArrayChildNode.SetValue(array, null);

            // Assert
            Assert.That(array[0], Is.Not.Null);
        }

        [Test]
        public void should_get_value_for_array_child_node()
        {
            //arrange
            var array = new [] { new MockPet {Name = "Doug"} };
            var propertyParseTreeArrayChildNode = new PropertyParseTreeArrayChildNode
                {
                    Name = "0",
                    PropertyType = typeof (MockPet)
                };

            // Act
            var value = (MockPet)propertyParseTreeArrayChildNode.GetValue(array);

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Name, Is.EqualTo("Doug"));
        }

        [Test]
        public void should_create_object_for_array_child_node()
        {
            //arrange
            var propertyParseTreeArrayChildNode = new PropertyParseTreeArrayChildNode
                {
                    PropertyType = typeof (MockPet)
                };

            // Act
            var value = propertyParseTreeArrayChildNode.CreateObject(null);

            // Assert
            Assert.That(value, Is.TypeOf(typeof(MockPet)));
        }
    }
}