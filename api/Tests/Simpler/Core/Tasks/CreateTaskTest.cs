using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Mocks;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_create_proxy_to_task()
        {
            var createTask = Task.New<CreateTask>();
            createTask.In.TaskType = typeof(MockTask);
            createTask.Execute();

            Assert.That(createTask.Out.TaskInstance, Is.InstanceOf<MockTask>());
            Assert.That(createTask.Out.TaskInstance.GetType().Name, Is.EqualTo("MockTaskProxy"));
        }
    }
}