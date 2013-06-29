using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class InjectTasksTest
    {
        [Test]
        public void should_inject_sub_task_if_null()
        {
            // Arrange
            var mockParentTask = new MockParentSimpleTask();
            var task = new InjectSimpleTasks { In = { SimpleTaskContainingSubSimpleTasks = mockParentTask } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubSimpleClass, Is.Not.Null);
        }

        [Test]
        public void should_not_inject_sub_task_if_not_null()
        {
            // Arrange
            var mockDifferentSubTask = new MockSubSimpleTask<DateTime>();
            var mockParentTask = new MockParentSimpleTask { MockSubSimpleClass = mockDifferentSubTask };
            var task = new InjectSimpleTasks { In = { SimpleTaskContainingSubSimpleTasks = mockParentTask } };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.InjectedSubTaskPropertyNames.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_return_a_list_of_all_sub_tasks_that_were_injected()
        {
            // Arrange
            var mockParentTask = new MockParentSimpleTask();
            var task = new InjectSimpleTasks { In = { SimpleTaskContainingSubSimpleTasks = mockParentTask } };

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.InjectedSubTaskPropertyNames[0], Is.EqualTo(typeof(MockSubSimpleTask<DateTime>).FullName));
        }
    }
}
