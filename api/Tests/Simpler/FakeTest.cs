using NUnit.Framework;
using Simpler.Mocks;

namespace Simpler
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
    }
}
