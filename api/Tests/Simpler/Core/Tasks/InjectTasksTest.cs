using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Mocks;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class InjectTasksTest
    {
        [Test]
        public void should_inject_sub_task_if_null()
        {
            var mockParentTask = new MockParentTask();
            var task = new InjectTasks { In = { TaskContainingSubTasks = mockParentTask } };
            task.Execute();

            Assert.That(mockParentTask.MockSubTask, Is.Not.Null);
        }

        [Test]
        public void should_not_inject_sub_task_if_not_null()
        {
            var mockDifferentSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubTask = mockDifferentSubTask };
            var task = new InjectTasks { In = { TaskContainingSubTasks = mockParentTask } };
            task.Execute();

            Assert.That(task.Out.InjectedSubTaskPropertyNames.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_return_a_list_of_all_sub_tasks_that_were_injected()
        {
            var mockParentTask = new MockParentTask();
            var task = new InjectTasks { In = { TaskContainingSubTasks = mockParentTask } };
            task.Execute();

            Assert.That(task.Out.InjectedSubTaskPropertyNames[0], Is.EqualTo(typeof(MockSubTask<DateTime>).FullName));
        }
    }
}
