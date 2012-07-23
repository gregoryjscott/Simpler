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
        public void should_dispose_sub_job_property_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubClass = mockSubTask };
            var job = new DisposeTasks { Owner = mockParentTask, InjectedTaskNames = new string[] { typeof(MockSubTask<DateTime>).FullName } };

            // Act
            job.Run();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_job_property_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubClass = mockSubTask };
            var job = new DisposeTasks { Owner = mockParentTask, InjectedTaskNames = new string[] { "bogus" } };

            // Act
            job.Run();

            // Assert
            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_job_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubClass = new MockSubTask<DateTime>() };
            var job = new DisposeTasks { Owner = mockParentTask, InjectedTaskNames = new string[] { typeof(MockSubTask<DateTime>).FullName } };

            // Act
            job.Run();

            // Assert
            Assert.That(mockParentTask.MockSubClass, Is.Null);
        }

        [Test]
        public void should_not_set_sub_job_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentTask = new MockParentTask { MockSubClass = new MockSubTask<DateTime>() };
            var job = new DisposeTasks { Owner = mockParentTask, InjectedTaskNames = new string[] { "bogus" } };

            // Act
            job.Run();

            // Assert
            Assert.That(mockParentTask.MockSubClass, Is.Not.Null);
        }
    }
}
