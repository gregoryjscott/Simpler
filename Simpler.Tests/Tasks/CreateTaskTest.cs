using NUnit.Framework;
using Simpler.Tasks;
using Simpler.Tests.Mocks;
using Simpler.Tests.Injection.Mocks;

namespace Simpler.Tests.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var task = new CreateTask { TaskType = typeof(MockTask) };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.TaskInstance, Is.InstanceOf<MockTask>());
            Assert.That(task.TaskInstance.GetType().Name, Is.Not.EqualTo("MockTaskWithOnExecuteAttributeProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var task = new CreateTask { TaskType = typeof(MockTaskWithAttributes) };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.TaskInstance, Is.InstanceOf<MockTaskWithAttributes>());
            Assert.That(task.TaskInstance.GetType().Name, Is.EqualTo("MockTaskWithAttributesProxy"));
        }

        [Test]
        public void should_inject_sub_tasks_before_execution_if_given_type_is_decorated_with_inject_sub_tasks_attribute()
        {
            // Arrange
            var task = new CreateTask { TaskType = typeof(MockParentTask) };

            // Act
            task.Execute();
            ((MockParentTask)task.TaskInstance).Execute();

            // Assert
            Assert.That(((MockParentTask)task.TaskInstance).SubTaskWasInjected, Is.True);
        }
    }
}