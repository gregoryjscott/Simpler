using System.Dynamic;
using NUnit.Framework;
using Simpler.Data.PropertyMappingTree;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class DynamicNodeTest
    {
        [Test]
        public void should_set_value_for_dynamic_node()
        {
            //arrange
            var obj = new MockPerson();
            var propertyParseTreeArrayNode = new DynamicNode()
                {
                    Name = "Other",
                    PropertyInfo = obj.GetType().GetProperty("Other"),
                    PropertyType = typeof (object)
                };

            // Act
            propertyParseTreeArrayNode.SetValue(obj, null);

            // Assert
            Assert.That(obj.Other, Is.Not.Null);
        }

        [Test]
        public void should_get_value_for_dynamic_node()
        {
            //arrange
            var obj = new MockPerson{Other = new ExpandoObject()};
            var propertyParseTreeArrayNode = new DynamicNode()
            {
                Name = "Other",
                PropertyInfo = obj.GetType().GetProperty("Other"),
                PropertyType = typeof(object)
            };

            // Act
            var value = propertyParseTreeArrayNode.GetValue(obj);

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(obj.Other));
        }

        [Test]
        public void should_create_object_for_dynamic_node()
        {
            //arrange
            var propertyParseTreeArrayNode = new DynamicNode()
            {
                Name = "Other",
                PropertyType = typeof(object)
            };

            // Act
            var value = propertyParseTreeArrayNode.CreateObject();

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.TypeOf(typeof(ExpandoObject)));
        }
    }
}