using Castle.DynamicProxy;
using NUnit.Framework;
using Moq;
using Simpler.Core.Tasks;
using Simpler.Mocks;
using MockException = Simpler.Mocks.MockException;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class ExecuteTaskTest
    {
        [Test]
        public void should_send_notifications_before_and_after_the_task_is_executed()
        {
            var task = Task.New<ExecuteTask>();

            var taskWithAttributes = new MockTaskWithAttributes();
            task.In.Task = taskWithAttributes;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributes.Execute);
            task.In.Invocation = mockInvocation.Object;

            task.Execute();

            VerifyFiveCallbacks(taskWithAttributes);
        }

        [Test]
        public void should_send_notifications_if_task_execution_throws_an_unhandled_exception()
        {
            var task = Task.New<ExecuteTask>();

            var taskWithAttributesThatThrows = new MockTaskThatThrowsWithAttributes();
            task.In.Task = taskWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);
            task.In.Invocation = mockInvocation.Object;

            Assert.Throws(typeof(MockException), task.Execute);
            VerifySevenCallbacks(taskWithAttributesThatThrows);
        }

        [Test]
        public void should_send_notifications_after_the_task_is_executed_even_if_exception_occurs()
        {
            var task = Task.New<ExecuteTask>();

            var taskWithAttributesThatThrows = new MockTaskThatThrowsWithAttributes();
            task.In.Task = taskWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(taskWithAttributesThatThrows.Execute);
            task.In.Invocation = mockInvocation.Object;

            var throwHappened = false;
            try
            {
                try
                {
                    task.Execute();
                }
                finally
                {
                    VerifySevenCallbacks(taskWithAttributesThatThrows);
                }
            }
            catch (MockException)
            {
                throwHappened = true;
            }
            Assert.That(throwHappened, "The exception should still happen.");
        }

        #region Helpers
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
        #endregion
    }
}