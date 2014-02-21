using System;
using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class DisposeTasksTest
    {
        [Test]
        public void should_dispose_sub_task_property_that_is_included_in_list_of_injected_property_names()
        {
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask {MockSubClass = mockSubTask};

            Execute.Now<DisposeTasks>(dt => {
                dt.In.Owner = mockParentTask;
                dt.In.InjectedTaskNames = new[] {typeof (MockSubTask<DateTime>).FullName};
            });

            Assert.That(mockSubTask.DisposeWasCalled, Is.True);
        }

        [Test]
        public void should_not_dispose_sub_task_property_that_is_not_included_in_list_of_injected_property_names()
        {
            var mockSubTask = new MockSubTask<DateTime>();
            var mockParentTask = new MockParentTask {MockSubClass = mockSubTask};

            Execute.Now<DisposeTasks>(dt => {
                dt.In.Owner = mockParentTask;
                dt.In.InjectedTaskNames = new[] {"bogus"};
            });

            Assert.That(mockSubTask.DisposeWasCalled, Is.False);
        }

        [Test]
        public void should_set_sub_task_property_to_null_that_is_included_in_list_of_injected_property_names()
        {
            var mockParentTask = new MockParentTask {MockSubClass = new MockSubTask<DateTime>()};

            Execute.Now<DisposeTasks>(dt => {
                dt.In.Owner = mockParentTask;
                dt.In.InjectedTaskNames = new[] {typeof (MockSubTask<DateTime>).FullName};
            });

            Assert.That(mockParentTask.MockSubClass, Is.Null);
        }

        [Test]
        public void should_not_set_sub_task_property_to_null_that_is_not_included_in_list_of_injected_property_names()
        {
            var mockParentTask = new MockParentTask {MockSubClass = new MockSubTask<DateTime>()};

            Execute.Now<DisposeTasks>(dt => {
                dt.In.Owner = mockParentTask;
                dt.In.InjectedTaskNames = new[] {"bogus"};
            });

            Assert.That(mockParentTask.MockSubClass, Is.Not.Null);
        }
    }
}
