using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Mocks;
using System;

namespace Simpler
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void should_create_new_task()
        {
            var mockTask = Task.New<MockTask>();

            Assert.That(mockTask, Is.Not.Null);
        }

        [Test]
        public void should_provide_underlying_task_name()
        {
            var mockTask = Task.New<MockTask>();

            Assert.That(mockTask.Name, Is.EqualTo(typeof(MockTask).FullName));
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
