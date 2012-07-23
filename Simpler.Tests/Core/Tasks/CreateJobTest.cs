using NUnit.Framework;
using Simpler.Core.Jobs;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Jobs
{
    [TestFixture]
    public class CreateJobTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var job = new CreateTask { JobType = typeof(MockTask) };

            // Act
            job.Run();

            // Assert
            Assert.That(job.JobInstance, Is.InstanceOf<MockTask>());
            Assert.That(job.JobInstance.GetType().Name, Is.Not.EqualTo("MockJobWithOnExecuteAttributeProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_execution_callbacks_attribute()
        {
            // Arrange
            var job = new CreateTask { JobType = typeof(MockTaskWithAttributes) };

            // Act
            job.Run();

            // Assert
            Assert.That(job.JobInstance, Is.InstanceOf<MockTaskWithAttributes>());
            Assert.That(job.JobInstance.GetType().Name, Is.EqualTo("MockJobWithAttributesProxy"));
        }
    }
}