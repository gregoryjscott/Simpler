using NUnit.Framework;
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
            Assert.That(mockTask, Is.Not.Null);
        }

        [Test]
        public void should_provide_underlying_task_name()
        {
            var mockTask = Task.New<MockTask>();
            Assert.That(mockTask.Name, Is.EqualTo("Simpler.Tests.Core.Mocks.MockTask"));
        }
    }
}
