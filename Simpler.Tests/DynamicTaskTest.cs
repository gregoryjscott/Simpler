using Moq;
using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class DynamicTaskTest
    {
        [Test]
        public void should_be_able_to_test_dynamic_Inputs_and_Outputs_properties()
        {
            // Arrange
            var task = TaskFactory<MockDynamicSubTask>.Create();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Outputs.SomeOutput, Is.EqualTo(9));
        }

        [Test]
        public void should_be_able_to_mock_dynamic_Inputs_and_Outputs_properties_on_subtasks()
        {
            // Arrange
            var task = TaskFactory<MockDynamicTask>.Create();
            var mockSubTask = new Mock<MockDynamicSubTask>();
            mockSubTask.Setup(st => st.Outputs).Returns(new { SomeOutput = 7 });
            task.MockDynamicSubTask = mockSubTask.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Outputs.SubTaskOutputs.SomeOutput, Is.EqualTo(7));
        }

        [Test]
        public void should_be_execute_using_shorthand_syntax()
        {
            // Arrange
            var task = TaskFactory<MockDynamicTask>.Create();
            
            // Act
            var outputs = task
                .SetInputs(new {SendSomething = "something"})
                .GetOutputs();

            // Assert
            Assert.That(outputs.InputsReceived.SendSomething, Is.EqualTo("something"));
        }
    }
}
