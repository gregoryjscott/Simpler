using Moq;
using NUnit.Framework;
using Simpler.Injection.Tasks;
using Simpler.Tests.Injection.Mocks;
using System;

namespace Simpler.Tests.Injection.Tasks
{
    [TestFixture]
    public class InjectSubTasksTest
    {
        [Test]
        public void should_inject_sub_task_if_null()
        {
            // Arrange
            var mockParentTask = new MockParentTask();
            var task = new InjectSubTasks { TaskContainingSubTasks = mockParentTask };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubClass, Is.Not.Null);
        }

        [Test]
        public void should_not_inject_sub_task_if_not_null()
        {
            // Arrange
            var mockDifferentSubTask = new Mock<MockSubTask<DateTime>>();
            var mockParentTask = new MockParentTask { MockSubClass = mockDifferentSubTask.Object };
            var task = new InjectSubTasks { TaskContainingSubTasks = mockParentTask };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.InjectedSubTaskPropertyNames.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_return_a_list_of_all_sub_tasks_that_were_injected()
        {
            // Arrange
            var mockParentTask = new MockParentTask();
            var task = new InjectSubTasks { TaskContainingSubTasks = mockParentTask };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.InjectedSubTaskPropertyNames[0], Is.EqualTo(typeof(MockSubTask<DateTime>).FullName));
        }
    }
}
