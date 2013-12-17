using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class FakeTest
    {
        [Test]
        public void should_fake_subtasks()
        {
            // Arrange
            var mockParentTask = Task.New<MockParentTask>();

            // Act
            Fake.Subtasks(mockParentTask);
            mockParentTask.Execute();

            // Assert
            Assert.That(mockParentTask.SubtaskWasExecuted, Is.False);
        }
    }
}
