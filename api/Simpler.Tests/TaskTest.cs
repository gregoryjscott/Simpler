using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;
using System;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void should_create_new_task()
        {
            var mockTask = Task.New<MockTask>();

            Assert.That(mockTask != null, "MockTask was not created.");
        }

        [Test]
        public void should_provide_underlying_task_name()
        {
            var mockTask = Task.New<MockTask>();
            const string expectedName = "Simpler.Tests.Core.Mocks.MockTask";

            Assert.That(mockTask.Name == expectedName, "Expected name to be {0}.", expectedName);
        }

        [Test]
        public void should_throw_if_attempt_new_InjectTasks()
        {
            Assert.Throws<ArgumentException>(() => Task.New<InjectTasks>());
        }

        [Test]
        public void should_throw_if_attempt_new_DisposeTasks()
        {
            Assert.Throws<ArgumentException>(() => Task.New<DisposeTasks>());
        }
    }
}
