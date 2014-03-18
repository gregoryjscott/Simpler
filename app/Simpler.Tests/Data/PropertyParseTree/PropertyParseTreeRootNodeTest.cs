using NUnit.Framework;
using Simpler.Data.PropertyParseTree;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.PropertyParseTree
{
    [TestFixture]
    public class PropertyParseTreeRootNodeTest
    {
        [Test]
        public void should_create_object_for_object_node()
        {
            //arrange
            var propertyParseTreeRootNode = new PropertyParseTreeRootNode()
            {
                PropertyType = typeof(MockPerson)
            };

            // Act
            var value = propertyParseTreeRootNode.CreateObject();

            // Assert
            Assert.That(value, Is.TypeOf(typeof(MockPerson)));
        }
    }
}