using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using Simpler.Construction.Jobs;
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
            var job = new InterceptJobExecution();

            var mockInvocation = new Mock<IInvocation>();
            var mockJob = new MockJob();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("Execute");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(mockJob);
            job.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<NotifySubscribersOfJobExecution>();
            job.NotifySubscribersOfJobExecution = mockNotifySubscribers.Object;

            // Act
            job.Execute();

            // Assert
            mockNotifySubscribers.VerifySet(notifySubscribers => notifySubscribers.ExecutingJob = mockJob);
            mockNotifySubscribers.Verify(notifySubscribers => notifySubscribers.Execute(), Times.Once());
        }

        [Test]
        public void should_just_allow_invocation_to_proceed_if_the_invocation_method_is_not_Execute()
        {
            // Arrange
            var job = new InterceptJobExecution();

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("NotExecute");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(new MockJob());
            job.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<NotifySubscribersOfJobExecution>();
            job.NotifySubscribersOfJobExecution = mockNotifySubscribers.Object;

            // Act
            job.Execute();

            // Assert
            mockInvocation.Verify(invocation => invocation.Proceed(), Times.Once());
        }
    }
}