using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class JobTest
    {
        [Test]
        public void should_inject_sub_jobs_before_execution_if_given_type_is_decorated_with_inject_sub_jobs_attribute()
        {
            // Arrange
            var job = Job.Create<MockParentJob>();

            // Act
            job.Execute();

            // Assert
            Assert.That(job.SubJobWasInjected, Is.True);
        }
    }
}
