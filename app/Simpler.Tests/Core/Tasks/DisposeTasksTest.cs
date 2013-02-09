using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class DisposeTasksTest
    {
        [Test]
        public void should_dispose_sub_task_property_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubTask = mockSubTask };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubTask<DateTime>).FullName } } };

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
            var mockParentTask = new MockParentTask { MockSubTask = mockSubTask };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_task_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubTask = new MockSubTask<DateTime>() };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubTask<DateTime>).FullName } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubTask, Is.Null);
        }

        [Test]
        public void should_not_set_sub_task_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubTask = new MockSubTask<DateTime>() };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubTask, Is.Not.Null);
        }
    }
}
