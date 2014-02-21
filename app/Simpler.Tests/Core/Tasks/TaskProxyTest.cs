using Castle.DynamicProxy;
using NUnit.Framework;
using Moq;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;
using MockException = Simpler.Tests.Core.Mocks.MockException;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class TaskProxyTest
    {
        static void VerifyFiveCallbacks(MockTaskWithAttributes taskWithAttributes)
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
        static void VerifySevenCallbacks(MockTaskThatThrowsWithAttributes taskThatThrowsWithAttributes)
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

        [Test]
        public void should_send_notifications_before_and_after_the_task_is_executed()
        {
            var taskWithAttributes = Task.New<MockTaskWithAttributes>();
            taskWithAttributes.Execute();

            VerifyFiveCallbacks(taskWithAttributes);
        }

        [Test]
        public void should_send_notifications_if_task_execution_throws_an_unhandled_exception()
        {
            var taskWithAttributesThatThrows = Task.New<MockTaskThatThrowsWithAttributes>();

            Assert.Throws(typeof(MockException), taskWithAttributesThatThrows.Execute);
            VerifySevenCallbacks(taskWithAttributesThatThrows);
        }

        [Test]
        public void should_send_notifications_after_the_task_is_executed_even_if_exception_occurs()
        {
            var taskWithAttributesThatThrows = Task.New<MockTaskThatThrowsWithAttributes>();

            var throwHappened = false;
            try
            {
                try
                {
                    // Act (this will throw an exception)
                    taskWithAttributesThatThrows.Execute();
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

        //[Test]
        //public void should_allow_the_task_execution_to_be_overriden()
        //{
        //    // Arrange
        //    var task = Task.New<ExecuteTask>();

        //    var taskWithOverride = new MockTaskWithOverrideAttribute();
        //    task.In.Task = taskWithOverride;

        //    var mockInvocation = new Mock<IInvocation>();
        //    mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithOverride.Execute);
        //    mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(taskWithOverride);
        //    task.In.Invocation = mockInvocation.Object;

        //    // Act
        //    task.Execute();

        //    // Assert
        //    Assert.That(taskWithOverride.OverrideWasCalledBeforeTheTaskWasExecuted);
        //}
    }
}