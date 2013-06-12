using NUnit.Framework;
using Simpler.Core.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Core.Tasks
{
    [TestFixture]
    public class CreateTaskTest
    {
        [Test]
        public void should_create_task()
        {
            // Arrange
            var task = Task.New<CreateTask>();
            task.In.TaskType = typeof (MockTask);

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.TaskInstance, Is.InstanceOf<MockTask>());
        }
    }
}