﻿using NUnit.Framework;
using System;
using Simpler.Core.Tasks;
using Simpler.Mocks;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class DisposeTasksTest
    {
        [Test]
        public void should_dispose_sub_task_property_that_is_included_in_list_of_injected_property_names()
        {
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubTask = mockSubTask };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubTask<DateTime>).FullName } } };
            task.Execute();

            Assert.That(mockSubTask.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_task_property_that_is_not_included_in_list_of_injected_property_names()
        {
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask { MockSubTask = mockSubTask };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };
            task.Execute();

            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_task_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            var mockParentTask = new MockParentTask { MockSubTask = new MockSubTask<DateTime>() };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { typeof(MockSubTask<DateTime>).FullName } } };
            task.Execute();

            Assert.That(mockParentTask.MockSubTask, Is.Null);
        }

        [Test]
        public void should_not_set_sub_task_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            var mockParentTask = new MockParentTask { MockSubTask = new MockSubTask<DateTime>() };
            var task = new DisposeTasks { In = { Owner = mockParentTask, InjectedTaskNames = new[] { "bogus" } } };
            task.Execute();

            Assert.That(mockParentTask.MockSubTask, Is.Not.Null);
        }
    }
}
