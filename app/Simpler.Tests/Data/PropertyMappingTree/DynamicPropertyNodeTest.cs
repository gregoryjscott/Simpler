using System.Dynamic;
using NUnit.Framework;
using Simpler.Data.PropertyMappingTree;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class DynamicPropertyNodeTest
    {
        [Test]
        public void should_set_value_for_dynamic_node()
        {
            //arrange
            dynamic obj = new ExpandoObject();
            var propertyParseTreeArrayNode = new DynamicPropertyNode()
                {
                    Name = "City",
                    PropertyType = typeof (string)
                };

            // Act
            propertyParseTreeArrayNode.SetValue(obj, "Anchorage");

            // Assert
            Assert.That(obj.City, Is.EqualTo("Anchorage"));
        }

        [Test]
        public void should_get_value_for_dynamic_node()
        {
            //arrange
            dynamic obj = new ExpandoObject();
            obj.City = "Anchorage";
            var propertyParseTreeArrayNode = new DynamicPropertyNode()
            {
                Name = "City",
                PropertyType = typeof(string)
            };

            // Act
            var value = propertyParseTreeArrayNode.GetValue(obj);

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(obj.City));
        }

        [Test]
        public void should_create_object_for_dynamic_node()
        {
            //arrange
            var propertyParseTreeArrayNode = new DynamicPropertyNode()
            {
                Name = "City",
                PropertyType = typeof(string)
            };

            // Act
            var value = propertyParseTreeArrayNode.CreateObject("Anchorage");

            // Assert
            Assert.That(value, Is.EqualTo("Anchorage"));
        }
    }
}