using NUnit.Framework;
using Simpler;
using Simpler.Core.Tasks;
using Simpler.Mocks;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class FakeTaskTest
    {
        [Test]
        public void should_create_instance_of_given_type()
        {
            var fakeTask = Task.New<FakeTask>();
            fakeTask.In.TaskType = typeof(MockTask);
            fakeTask.Execute();

            Assert.That(fakeTask.Out.TaskInstance, Is.InstanceOf<MockTask>());
        }

        [Test]
        public void should_override_execute_with_given_action()
        {
            var overrideCalled = false;
            Action<Task> action = t => overrideCalled = true;
            var fakeTask = Task.New<FakeTask>();
            fakeTask.In.TaskType = typeof(MockTask);
            fakeTask.In.ExecuteOverride = action;
            fakeTask.Execute();
            (fakeTask.Out.TaskInstance as MockTask).Execute();

            Assert.That(overrideCalled, Is.True);
        }
    }
}
