using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using Simpler.Construction.Tasks;
using Simpler.Tests.Construction.Mocks;

namespace Simpler.Tests.Construction.Tasks
{
    [TestFixture]
    public class InterceptTaskExecutionTest
    {
        [Test]
        public void should_intercept_the_invocation_and_notify_subscribers_if_the_invocation_method_is_Execute()
        {
            // Arrange
            var task = new InterceptTaskExecution();

            var mockInvocation = new Mock<IInvocation>();
            var mockTask = new MockTask();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("Execute");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(mockTask);
            task.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<NotifySubscribersOfTaskExecution>();
            task.NotifySubscribersOfTaskExecution = mockNotifySubscribers.Object;

            // Act
            task.Execute();

            // Assert
            mockNotifySubscribers.VerifySet(notifySubscribers => notifySubscribers.ExecutingTask = mockTask);
            mockNotifySubscribers.Verify(notifySubscribers => notifySubscribers.Execute(), Times.Once());
        }

        [Test]
        public void should_just_allow_invocation_to_proceed_if_the_invocation_method_is_not_Execute()
        {
            // Arrange
            var task = new InterceptTaskExecution();

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Method.Name).Returns("NotExecute");
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(new MockTask());
            task.Invocation = mockInvocation.Object;

            var mockNotifySubscribers = new Mock<NotifySubscribersOfTaskExecution>();
            task.NotifySubscribersOfTaskExecution = mockNotifySubscribers.Object;

            // Act
            task.Execute();

            // Assert
            mockInvocation.Verify(invocation => invocation.Proceed(), Times.Once());
        }
    }
}