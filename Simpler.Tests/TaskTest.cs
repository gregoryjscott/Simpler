using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void should_create_new_task()
        {
            var mockTask = Task.New<MockTask>();

            Check.That(mockTask != null, "MockTask was not created.");
        }

        [Test]
        public void should_provide_underlying_task_name()
        {
            var mockTask = Task.New<MockTask>();
            const string expectedName = "Simpler.Tests.Core.Mocks.MockTask";

            Check.That(mockTask.Name == expectedName, "Expected name to be {0}.", expectedName);
        }

        [Test]
        public void should_throw_if_attempt_new_InjectTasks()
        {
            Check.Throws(() => Task.New<InjectTasks>());
        }

        [Test]
        public void should_throw_if_attempt_new_DisposeTasks()
        {
            Check.Throws<CheckException>(() => Task.New<DisposeTasks>());
        }
    }
}
