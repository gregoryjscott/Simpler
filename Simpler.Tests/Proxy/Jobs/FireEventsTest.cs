using Castle.DynamicProxy;
using NUnit.Framework;
using Moq;
using Simpler.Proxy.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Proxy.Jobs
{
    [TestFixture]
    public class FireEventsTest
    {
        static void VerifyFiveCallbacks(MockJobWithAttributes jobWithAttributes)
        {
            Assert.That(jobWithAttributes.CallbackQueue.Count, Is.EqualTo(5));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (jobWithAttributes.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(jobWithAttributes.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }
        static void VerifySevenCallbacks(MockJobWithAttributesThatThrows jobWithAttributesThatThrows)
        {
            Assert.That(jobWithAttributesThatThrows.CallbackQueue.Count, Is.EqualTo(7));

            // Note: Attributes on a class are not returned in any order and therefore the it can not be assumed that the
            // first attribute will receive the first callback.
            if (jobWithAttributesThatThrows.CallbackQueue.Peek().Contains("First"))
            {
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
            }
            else
            {
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.Before"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.Before"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Execute"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.OnError"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.OnError"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("First.After"));
                Assert.That(jobWithAttributesThatThrows.CallbackQueue.Dequeue(), Is.EqualTo("Second.After"));
            }
        }

        [Test]
        public void should_send_notifications_before_and_after_the_job_is_executed()
        {
            // Arrange
            var job = new FireEvents();

            var jobWithAttributes = new MockJobWithAttributes();
            job.Job = jobWithAttributes;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(jobWithAttributes.Run);
            job.Invocation = mockInvocation.Object;

            // Act
            job.Run();

            // Assert
            VerifyFiveCallbacks(jobWithAttributes);
        }

        [Test]
        public void should_send_notifications_if_job_execution_throws_an_unhandled_exception()
        {
            // Arrange
            var job = new FireEvents();

            var jobWithAttributesThatThrows = new MockJobWithAttributesThatThrows();
            job.Job = jobWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(jobWithAttributesThatThrows.Run);
            job.Invocation = mockInvocation.Object;

            // Act
            Assert.Throws(typeof(TestException), job.Run);

            // Assert
            VerifySevenCallbacks(jobWithAttributesThatThrows);
        }

        [Test]
        public void should_send_notifications_after_the_job_is_executed_even_if_exception_occurs()
        {
            // Arrange
            var job = new FireEvents();

            var jobWithAttributesThatThrows = new MockJobWithAttributesThatThrows();
            job.Job = jobWithAttributesThatThrows;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(jobWithAttributesThatThrows.Run);
            job.Invocation = mockInvocation.Object;

            var throwHappened = false;
            try
            {
                try
                {
                    // Act (this will throw an exception)
                    job.Run();
                }
                finally
                {
                    // Assert
                    VerifySevenCallbacks(jobWithAttributesThatThrows);
                }
            }
            catch (TestException)
            {
                throwHappened = true;
            }
            Assert.That(throwHappened, "The exception should still happen.");
        }

        [Test]
        public void should_allow_the_job_execution_to_be_overriden()
        {
            // Arrange
            var job = new FireEvents();

            var jobWithOverride = new MockJobWithOverrideAttribute();
            job.Job = jobWithOverride;

            var mockInvocation = new Mock<IInvocation>();
            mockInvocation.Setup(invocation => invocation.Proceed()).Callback(jobWithOverride.Run);
            mockInvocation.Setup(invocation => invocation.InvocationTarget).Returns(jobWithOverride);
            job.Invocation = mockInvocation.Object;

            // Act
            job.Run();

            // Assert
            Assert.That(jobWithOverride.OverrideWasCalledBeforeTheJobWasExecuted);
        }
    }
}