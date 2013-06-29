using Castle.DynamicProxy;
using NUnit.Framework;
using Moq;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;
using MockException = Simpler.Tests.Core.Mocks.MockException;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class ExecuteTaskTest
    {
        static void VerifyFiveCallbacks(MockSimpleTaskWithAttributes simpleTaskWithAttributes)
        {
            Assert.That(simpleTaskWithAttributes.CallbackQueue.Count, Is.EqualTo(5));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (simpleTaskWithAttributes.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(simpleTaskWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }
        static void VerifySevenCallbacks(MockSimpleTaskThatThrowsWithAttributes simpleTaskThatThrowsWithAttributes)
        {
            Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Count, Is.EqualTo(7));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (simpleTaskThatThrowsWithAttributes.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(simpleTaskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }

        [Test]
        public void should_send_notifications_before_and_after_the_task_is_executed()
        {
            // Arrange
            var task = SimpleTask.New<ExecuteSimpleTask>();

            var taskWithAttributes = new MockSimpleTaskWithAttributes();
            task.In.SimpleTask = taskWithAttributes;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributes.Execute);
            task.In.Invocation = mockInvocation.Object;

            // Act
            task.Execute();

            // Assert
            VerifyFiveCallbacks(taskWithAttributes);
        }

        [Test]
        public void should_send_notifications_if_task_execution_throws_an_unhandled_exception()
        {
            // Arrange
            var task = SimpleTask.New<ExecuteSimpleTask>();

            var taskWithAttributesThatThrows = new MockSimpleTaskThatThrowsWithAttributes();
            task.In.SimpleTask = taskWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);
            task.In.Invocation = mockInvocation.Object;

            // Act
            Assert.Throws(typeof(MockException), task.Execute);

            // Assert
            VerifySevenCallbacks(taskWithAttributesThatThrows);
        }

        [Test]
        public void should_send_notifications_after_the_task_is_executed_even_if_exception_occurs()
        {
            // Arrange
            var task = SimpleTask.New<ExecuteSimpleTask>();

            var taskWithAttributesThatThrows = new MockSimpleTaskThatThrowsWithAttributes();
            task.In.SimpleTask = taskWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);
            task.In.Invocation = mockInvocation.Object;

            var throwHappened = false;
            try
            {
                try
                {
                    // Act (this will throw an exception)
                    task.Execute();
                }
                finally
                {
                    // Assert
                    VerifySevenCallbacks(taskWithAttributesThatThrows);
                }
            }
            catch (MockException)
            {
                throwHappened = true;
            }
            Assert.That(throwHappened, "The exception should still happen.");
        }

        [Test]
        public void should_allow_the_task_execution_to_be_overriden()
        {
            // Arrange
            var task = SimpleTask.New<ExecuteSimpleTask>();

            var taskWithOverride = new MockSimpleTaskWithOverrideAttribute();
            task.In.SimpleTask = taskWithOverride;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithOverride.Execute);
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(taskWithOverride);
            task.In.Invocation = mockInvocation.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(taskWithOverride.OverrideWasCalledBeforeTheTaskWasExecuted);
        }
    }
}