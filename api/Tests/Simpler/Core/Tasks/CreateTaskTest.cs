using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Mocks;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_create_instance_of_given_type()
        {
            var createTask = Task.New<CreateTask>();
            createTask.In.TaskType = typeof(MockTask);
            createTask.Execute();

            Assert.That(createTask.Out.TaskInstance, Is.InstanceOf<MockTask>());
        }

        [Test]
        public void should_create_class_with_same_name_plus_Proxy()
        {
            var createTask = Task.New<CreateTask>();
            createTask.In.TaskType = typeof(MockTask);
            createTask.Execute();

            var expected = typeof(MockTask).FullName.Replace("MockTask", "MockTaskProxy");
            Assert.That(createTask.Out.TaskInstance.GetType().FullName, Is.EqualTo(expected));
        }
    }
}