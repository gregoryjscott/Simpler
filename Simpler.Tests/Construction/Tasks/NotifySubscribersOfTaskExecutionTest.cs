using System;
using Castle.DynamicProxy;
using NUnit.Framework;
using Simpler.Construction.Tasks;
using Simpler.Tests.Construction.Mocks;
using Moq;

namespace Simpler.Tests.Construction.Tasks
{
    [TestFixture]
    public class NotifySubscribersOfTaskExecutionTest
    {
        [Test]
        public void should_send_notifications_before_and_after_the_task_is_executed()
        {
            // Arrange
            var task = new NotifySubscribersOfTaskExecution();

            var taskWithAttributes = new MockTaskWithAttributes();
            task.ExecutingTask = taskWithAttributes;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributes.Execute);
            task.Invocation = mockInvocation.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(taskWithAttributes.CallbackQueue.Count, Is.EqualTo(5));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (taskWithAttributes.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(taskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }

        [Test]
        public void should_send_notifications_if_task_execution_throws_an_unhandled_exception()
        {
            // Arrange
            var task = new NotifySubscribersOfTaskExecution();

            var taskWithAttributesThatThrows = new MockTaskWithAttributesThatThrows();
            task.ExecutingTask = taskWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);
            task.Invocation = mockInvocation.Object;

            // Act
            Assert.Throws(typeof(Exception), task.Execute);

            // Assert
            Assert.That(taskWithAttributesThatThrows.CallbackQueue.Count, Is.EqualTo(5));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (taskWithAttributesThatThrows.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
            }
            else
            {
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(taskWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
            }
        }

        [Test]
        public void should_allow_the_task_execution_to_be_overriden()
        {
            // Arrange
            var task = new NotifySubscribersOfTaskExecution();

            var taskWithOverride = new MockTaskWithOverrideAttribute();
            task.ExecutingTask = taskWithOverride;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithOverride.Execute);
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(taskWithOverride);
            task.Invocation = mockInvocation.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(taskWithOverride.OverrideWasCalledBeforeTheTaskWasExecuted);
        }
    }
}