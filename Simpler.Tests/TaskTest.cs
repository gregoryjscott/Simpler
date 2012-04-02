using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void should_inject_sub_tasks_before_execution_if_given_type_is_decorated_with_inject_sub_tasks_attribute()
        {
            // Arrange
            var task = Invoke<MockParentTask>.New().Task;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.SubTaskWasInjected, Is.True);
        }
    }
}
