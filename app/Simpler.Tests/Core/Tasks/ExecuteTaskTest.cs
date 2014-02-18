using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class ExecuteTaskTest
    {
        [Test]
        public void should_send_notifications_before_and_after_the_task_is_executed()
        {
            var taskWithAttributes = new MockTaskWithAttributes();
            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributes.Execute);

            Execute.Now<ExecuteTask>(et => {
                et.In.Task = taskWithAttributes;
                et.In.Invocation = mockInvocation.Object;
            });

            VerifyEvents(taskWithAttributes);
        }

        [Test]
        public void should_send_notifications_if_task_execution_throws_an_unhandled_exception()
        {
            var taskWithAttributesThatThrows = new MockTaskThatThrowsWithAttributes();
            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);

            Assert.Throws<Mocks.MockException>(() => Execute.Now<ExecuteTask>(et => {
                et.In.Task = taskWithAttributesThatThrows;
                et.In.Invocation = mockInvocation.Object;
            }));

            VerifyEventsAndErrors(taskWithAttributesThatThrows);
        }

        [Test]
        public void should_send_notifications_after_the_task_is_executed_even_if_exception_occurs()
        {
            var taskWithAttributesThatThrows = new MockTaskThatThrowsWithAttributes();
            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);

            var throwHappened = false;
            try
            {
                try
                {
                    Execute.Now<ExecuteTask>(et => {
                        et.In.Task = taskWithAttributesThatThrows;
                        et.In.Invocation = mockInvocation.Object;
                    });
                }
                finally
                {
                    VerifyEventsAndErrors(taskWithAttributesThatThrows);
                }
            }
            catch (Mocks.MockException)
            {
                throwHappened = true;
            }
            Assert.That(throwHappened, "The exception should still happen.");
        }

        [Test]
        public void should_allow_the_task_execution_to_be_overriden()
        {
            var taskWithOverride = new MockTaskWithOverrideAttribute();
            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithOverride.Execute);
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(taskWithOverride);

            Execute.Now<ExecuteTask>(et => {
                et.In.Task = taskWithOverride;
                et.In.Invocation = mockInvocation.Object;
            });

            Assert.That(taskWithOverride.OverrideWasCalledBeforeTheTaskWasExecuted);
        }

        #region Helpers

        static void VerifyEvents(MockTaskWithAttributes taskWithAttributes)
        {
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

        static void VerifyEventsAndErrors(MockTaskThatThrowsWithAttributes taskThatThrowsWithAttributes)
        {
            Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Count, Is.EqualTo(7));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (taskThatThrowsWithAttributes.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(taskThatThrowsWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }

        #endregion
    }
}
