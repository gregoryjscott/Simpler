using NUnit.Framework;
using Simpler.Injection.Tasks;
using System;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Injection.Tasks
{
    [TestFixture]
    public class DisposeSubTasksTest
    {
        [Test]
        public void should_dispose_sub_task_property_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubClass = mockSubTask };
            var task = new DisposeSubTasks { TaskContainingSubTasks = mockParentTask, InjectedSubTaskPropertyNames = new string[] { typeof(MockSubTask<DateTime>).FullName } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_task_property_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubClass = mockSubTask };
            var task = new DisposeSubTasks { TaskContainingSubTasks = mockParentTask, InjectedSubTaskPropertyNames = new string[] { "bogus" } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_task_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubClass = new MockSubTask<DateTime>() };
            var task = new DisposeSubTasks { TaskContainingSubTasks = mockParentTask, InjectedSubTaskPropertyNames = new string[] { typeof(MockSubTask<DateTime>).FullName } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubClass, Is.Null);
        }

        [Test]
        public void should_not_set_sub_task_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubClass = new MockSubTask<DateTime>() };
            var task = new DisposeSubTasks { TaskContainingSubTasks = mockParentTask, InjectedSubTaskPropertyNames = new string[] { "bogus" } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubClass, Is.Not.Null);
        }
    }
}
