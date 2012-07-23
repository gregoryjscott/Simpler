using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class InjectJobsTest
    {
        [Test]
        public void should_inject_sub_job_if_null()
        {
            // Arrange
            var mockParentJob = new MockParentTask();
            var job = new InjectTasks { TaskContainingSubTasks = mockParentJob };

            // Act
            job.Run();

            // Assert
            Assert.That(mockParentJob.MockSubClass, Is.Not.Null);
        }

        [Test]
        public void should_not_inject_sub_job_if_not_null()
        {
            // Arrange
            var mockDifferentSubJob = new MockSubTask<DateTime>();
            var mockParentJob = new MockParentTask { MockSubClass = mockDifferentSubJob };
            var job = new InjectTasks { TaskContainingSubTasks = mockParentJob };

            // Act
            job.Run();

            // Assert
            Assert.That(job.InjectedSubJobPropertyNames.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_return_a_list_of_all_sub_jobs_that_were_injected()
        {
            // Arrange
            var mockParentJob = new MockParentTask();
            var job = new InjectTasks { TaskContainingSubTasks = mockParentJob };

            // Act
            job.Run();

            // Assert
            Assert.That(job.InjectedSubJobPropertyNames[0], Is.EqualTo(typeof(MockSubTask<DateTime>).FullName));
        }
    }
}
