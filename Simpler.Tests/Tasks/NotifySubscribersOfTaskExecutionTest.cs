using Castle.Core.Interceptor;
using NUnit.Framework;
using Simpler.Tasks;
using Simpler.Tests.Mocks;
using Moq;

namespace Simpler.Tests.Tasks
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
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(() => taskWithAttributes.Execute());
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
    }
}