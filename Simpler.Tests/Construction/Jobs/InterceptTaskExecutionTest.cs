using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using Simpler.Proxy.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Construction.Jobs
{
    [TestFixture]
    public class InterceptJobExecutionTest
    {
        [Test]
        public void should_intercept_the_invocation_and_notify_subscribers_if_the_invocation_method_is_Execute()
        {
            // Arrange
            var job = new InterceptRun();

            var mockInvocation = new Mock<IInvocation>();
            var mockJob = new MockJob();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("Run");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(mockJob);
            job.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<FireEvents>();
            job.FireEvents = mockNotifySubscribers.Object;

            // Act
            job.Run();

            // Assert
            mockNotifySubscribers.VerifySet(notifySubscribers => notifySubscribers.Job = mockJob);
            mockNotifySubscribers.Verify(notifySubscribers => notifySubscribers.Run(), Times.Once());
        }

        [Test]
        public void should_just_allow_invocation_to_proceed_if_the_invocation_method_is_not_Execute()
        {
            // Arrange
            var job = new InterceptRun();

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("NotExecute");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(new MockJob());
            job.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<FireEvents>();
            job.FireEvents = mockNotifySubscribers.Object;

            // Act
            job.Run();

            // Assert
            mockInvocation.Verify(invocation => invocation.Proceed(), Times.Once());
        }
    }
}