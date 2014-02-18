using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_just_provide_instance_if_given_type_is_not_decorated_with_execution_callbacks_attribute()
        {
            var createTask = Execute.Now<CreateTask>(ct => {
                ct.In.TaskType = typeof (MockTask);
            });

            Assert.That(createTask.Out.TaskInstance, Is.InstanceOf<MockTask>());
            Assert.That(createTask.Out.TaskInstance.GetType().Name, Is.Not.EqualTo("MockTaskWithAttributesProxy"));
        }

        [Test]
        public void should_provide_proxy_instance_if_given_type_is_decorated_with_execution_callbacks_attribute()
        {
            var createTask = Execute.Now<CreateTask>(ct => {
                ct.In.TaskType = typeof (MockTaskWithAttributes);
            });

            Assert.That(createTask.Out.TaskInstance, Is.InstanceOf<MockTaskWithAttributes>());
            Assert.That(createTask.Out.TaskInstance.GetType().Name, Is.EqualTo("MockTaskWithAttributesProxy"));
        }
    }
}
