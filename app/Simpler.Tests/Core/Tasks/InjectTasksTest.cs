﻿using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class InjectTasksTest
    {
        [Test]
        public void should_inject_sub_task_if_null()
        {
            var mockParentTask = new MockParentTask();

            Execute.Now<InjectTasks>(it => {
                it.In.TaskContainingSubTasks = mockParentTask;
            });

            Assert.That(mockParentTask.MockSubClass, Is.Not.Null);
        }

        [Test]
        public void should_not_inject_sub_task_if_not_null()
        {
            var mockDifferentSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubClass = mockDifferentSubTask };

            var injectTasks = Execute.Now<InjectTasks>(it => {
                it.In.TaskContainingSubTasks = mockParentTask;
            });

            Assert.That(injectTasks.Out.InjectedSubTaskPropertyNames.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_return_a_list_of_all_sub_tasks_that_were_injected()
        {
            var mockParentTask = new MockParentTask();

            var injectTasks = Execute.Now<InjectTasks>(it => {
                it.In.TaskContainingSubTasks = mockParentTask;
            });

            Assert.That(injectTasks.Out.InjectedSubTaskPropertyNames[0], Is.EqualTo(typeof(MockSubTask<DateTime>).FullName));
        }
    }
}
