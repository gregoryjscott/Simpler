using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var task = SimpleTask.New<CreateSimpleTask>();
            task.In.TaskType = typeof (MockSimpleTask);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.TaskInstance, Is.InstanceOf<MockSimpleTask>());
            Assert.That(task.Out.TaskInstance.GetType().Name, Is.Not.EqualTo("MockSimpleTaskWithAttributesProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var task = SimpleTask.New<CreateSimpleTask>();
            task.In.TaskType = typeof (MockSimpleTaskWithAttributes);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.TaskInstance, Is.InstanceOf<MockSimpleTaskWithAttributes>());
            Assert.That(task.Out.TaskInstance.GetType().Name, Is.EqualTo("MockSimpleTaskWithAttributesProxy"));
        }
    }
}