using NUnit.Framework;
using Simpler.Tasks;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Tasks
{
    [TestFixture]
    public class CreateInstanceOfTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_on_execute_attribute()
        {
            // Arrange
            var task = new CreateInstanceOf<MockTask>();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.TaskInstance, Is.InstanceOf<MockTask>());
            Assert.That(task.TaskInstance.GetType().Name, Is.Not.EqualTo("MockTaskWithOnExecuteAttributeProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_on_execute_attribute()
        {
            // Arrange
            var task = new CreateInstanceOf<MockTaskWithAttributes>();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.TaskInstance, Is.InstanceOf<MockTaskWithAttributes>());
            Assert.That(task.TaskInstance.GetType().Name, Is.EqualTo("MockTaskWithAttributesProxy"));
        }
    }
}