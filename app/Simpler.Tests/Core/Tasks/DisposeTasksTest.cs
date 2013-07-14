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
            var mockSubTask = new MockSubSimpleTask<DateTime>();
            var mockParentTask = new MockParentSimpleTask { MockSubSimpleClass = mockSubTask };
            var task = new DisposeSimpleTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubSimpleTask<DateTime>).FullName } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_task_property_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubSimpleTask<DateTime>();
            var mockParentTask = new MockParentSimpleTask { MockSubSimpleClass = mockSubTask };
            var task = new DisposeSimpleTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_task_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentSimpleTask { MockSubSimpleClass = new MockSubSimpleTask<DateTime>() };
            var task = new DisposeSimpleTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubSimpleTask<DateTime>).FullName } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubSimpleClass, Is.Null);
        }

        [Test]
        public void should_not_set_sub_task_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentSimpleTask { MockSubSimpleClass = new MockSubSimpleTask<DateTime>() };
            var task = new DisposeSimpleTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };

            // Act
            task.Execute();

            // Assert
            Assert.That(mockParentTask.MockSubSimpleClass, Is.Not.Null);
        }
    }
}
