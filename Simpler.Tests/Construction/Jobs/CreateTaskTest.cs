using NUnit.Framework;
using Simpler.Proxy.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Construction.Jobs
{
    [TestFixture]
    public class CreateJobTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var job = new _CreateJob { JobType = typeof(MockJob) };

            // Act
            job.Run();

            // Assert
            Assert.That(job.JobInstance, Is.InstanceOf<MockJob>());
            Assert.That(job.JobInstance.GetType().Name, Is.Not.EqualTo("MockJobWithOnExecuteAttributeProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var job = new _CreateJob { JobType = typeof(MockJobWithAttributes) };

            // Act
            job.Run();

            // Assert
            Assert.That(job.JobInstance, Is.InstanceOf<MockJobWithAttributes>());
            Assert.That(job.JobInstance.GetType().Name, Is.EqualTo("MockJobWithAttributesProxy"));
        }
    }
}