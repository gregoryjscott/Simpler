using NUnit.Framework;
using Simpler.Injection.Jobs;
using System;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Injection.Jobs
{
    [TestFixture]
    public class DisposeSubJobsTest
    {
        [Test]
        public void should_dispose_sub_job_property_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubJob = new MockSubJob<DateTime>();
            var mockParentJob = new MockParentJob { MockSubClass = mockSubJob };
            var job = new DisposeSubJobs { JobContainingSubJobs = mockParentJob, InjectedSubJobPropertyNames = new string[] { typeof(MockSubJob<DateTime>).FullName } };

            // Act
            job.Execute();

            // Assert
            Assert.That(mockSubJob.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_job_property_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockSubJob = new MockSubJob<DateTime>();
            var mockParentJob = new MockParentJob { MockSubClass = mockSubJob };
            var job = new DisposeSubJobs { JobContainingSubJobs = mockParentJob, InjectedSubJobPropertyNames = new string[] { "bogus" } };

            // Act
            job.Execute();

            // Assert
            Assert.That(mockSubJob.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_job_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentJob = new MockParentJob { MockSubClass = new MockSubJob<DateTime>() };
            var job = new DisposeSubJobs { JobContainingSubJobs = mockParentJob, InjectedSubJobPropertyNames = new string[] { typeof(MockSubJob<DateTime>).FullName } };

            // Act
            job.Execute();

            // Assert
            Assert.That(mockParentJob.MockSubClass, Is.Null);
        }

        [Test]
        public void should_not_set_sub_job_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            // Arrange
            var mockParentJob = new MockParentJob { MockSubClass = new MockSubJob<DateTime>() };
            var job = new DisposeSubJobs { JobContainingSubJobs = mockParentJob, InjectedSubJobPropertyNames = new string[] { "bogus" } };

            // Act
            job.Execute();

            // Assert
            Assert.That(mockParentJob.MockSubClass, Is.Not.Null);
        }
    }
}
