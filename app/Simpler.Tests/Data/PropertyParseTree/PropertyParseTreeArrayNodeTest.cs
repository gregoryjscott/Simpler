using NUnit.Framework;
using Simpler.Data.PropertyParseTree;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class PropertyParseTreeArrayNodeTest
    {
        [Test]
        public void should_set_value_for_array_node()
        {
            //arrange
            var obj = new MockPerson();
            var propertyParseTreeArrayNode = new PropertyParseTreeArrayNode
                {
                    Name = "Vehicles",
                    PropertyInfo = obj.GetType().GetProperty("Vehicles"),
                    PropertyType = typeof (MockVehicle[])
                };

            // Act
            propertyParseTreeArrayNode.SetValue(obj, null);

            // Assert
            Assert.That(obj.Vehicles, Is.Not.Null);
        }

        [Test]
        public void should_get_value_for_array()
        {
            //arrange
            var obj = new MockPerson {Vehicles = new MockVehicle[0]};
            var propertyParseTreeArrayNode = new PropertyParseTreeArrayNode
                {
                    Name = "Vehicles",
                    PropertyInfo = obj.GetType().GetProperty("Vehicles"),
                    PropertyType = typeof (MockVehicle[])
                };

            // Act
            var value = propertyParseTreeArrayNode.GetValue(obj);

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(obj.Vehicles));
        }

        [Test]
        public void should_create_object_for_array()
        {
            //arrange
            var propertyParseTreeArrayNode = new PropertyParseTreeArrayNode
                {
                    PropertyType = typeof (MockVehicle[])
                };

            // Act
            var value = propertyParseTreeArrayNode.CreateObject(null);

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.TypeOf(typeof(MockVehicle[])));
        }
    }
}