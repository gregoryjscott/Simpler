using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class FakeTest
    {
        [Test]
        public void should_fake_subtasks()
        {
            var mockParentTask = Task.New<MockParentTask>();
            Fake.SubTasks(mockParentTask);
            mockParentTask.Execute();

            Assert.That(mockParentTask.SubTaskWasExecuted, Is.False);
        }

        [Test]
        public void should_create_fake_task()
        {
            var mockTask = Fake.Task<MockTask>();
            Assert.That(mockTask, Is.Not.Null);
        }

        [Test]
        public void should_override_execute_with_given_action()
        {
            var overrideHappened = false;
            var mockTask = Fake.Task<MockTask>(t => overrideHappened = true);
            mockTask.Execute();

            Assert.That(overrideHappened, Is.True);
        }
    }
}
