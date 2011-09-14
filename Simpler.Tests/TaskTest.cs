using Moq;
using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void should_be_able_to_test_dynamic_Inputs_and_Outputs_properties()
        {
            // Arrange
            var task = TaskFactory<MockSubTaskUsingDynamicProperties>.Create();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Outputs.SomeOutput, Is.EqualTo(9));
        }

        [Test]
        public void should_be_able_to_mock_dynamic_Inputs_and_Outputs_properties_on_subtasks()
        {
            // Arrange
            var task = TaskFactory<MockTaskUsingDynamicProperties>.Create();
            var mockSubTask = new Mock<MockSubTaskUsingDynamicProperties>();
            mockSubTask.Setup(st => st.Outputs).Returns(new { SomeOutput = 7 });
            task.MockSubTaskUsingDynamicProperties = mockSubTask.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Outputs.SubTaskOutputs.SomeOutput, Is.EqualTo(7));
        }
    }
}
