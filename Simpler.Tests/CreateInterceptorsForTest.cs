using System;
using NUnit.Framework;
using SimpleTask.Construction;
using SimpleTask.Testing;
using SimpleTask.Tests.Mocks;

namespace SimpleTask.Tests
{
    [TestFixture(TypeArgs = new Type[] { typeof(MockTask) })]
    public class CreateInterceptorsForTest<T> : TaskTest<CreateInterceptorsFor<T>> where T : Task
    {
        [Test]
        public void should_provide_interceptor_that_will_inject_subtasks_before_the_task_is_executed()
        {
            Test.Act = () =>
            {
                TaskUnderTest.Execute();
            };

            Test.Assert = () =>
            {
                var interceptor = TaskUnderTest.Interceptors[0] as TaskExecutionInterceptor<T>;
                Assert.That(interceptor.TaskToExecuteDuringInterception, Is.InstanceOf<InjectSubTasksBeforeExecutionOf<T>>());
            };
        }

        [Test]
        public void should_provide_interceptor_that_will_dispose_subtasks_after_the_task_is_executed()
        {
            Test.Act = () =>
            {
                TaskUnderTest.Execute();
            };

            Test.Assert = () =>
            {
                var interceptor = TaskUnderTest.Interceptors[1] as TaskExecutionInterceptor<T>;
                Assert.That(interceptor.TaskToExecuteDuringInterception, Is.InstanceOf<DisposeSubTasksAfterExecutionOf<T>>());
            };
        }
    }

    [TestFixture(TypeArgs = new Type[] { typeof(MockTaskWithSubTaskInjectionDisabled) })]
    public class CreateInterceptorsForTest2<T> : TaskTest<CreateInterceptorsFor<T>> where T : Task
    {
        [Test]
        public void should_not_provide_any_interceptors()
        {
            Test.Act = () =>
            {
                TaskUnderTest.Execute();
            };

            Test.Assert = () =>
            {
                Assert.That(TaskUnderTest.Interceptors.Length == 0);
            };
        }
    }
}
