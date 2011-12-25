using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskTITOTest
    {
        [Test]
        public void should_update_InputsModel_when_Inputs_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.Inputs = new {Name = "tito"};

            // Assert
            Assert.That(task.InputsModel.Name, Is.EqualTo("tito"));
        }

        [Test]
        public void should_update_InputsModel_property_when_Inputs_property_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.Inputs = new {Name = "tito", Age = 10};
            task.Inputs.Age = 15;

            // Assert
            Assert.That(task.Inputs.Name, Is.EqualTo("tito"));
            Assert.That(task.Inputs.Age, Is.EqualTo(15));
        }

        [Test]
        public void should_update_Inputs_when_InputsModel_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.InputsModel = new MockObject {Name = "tito"};

            // Assert
            Assert.That(task.Inputs.Name, Is.EqualTo("tito"));
        }

        [Test]
        public void should_update_Inputs_property_when_InputsModel_property_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.InputsModel = new MockObject {Name = "tito", Age = 10};
            task.InputsModel.Age = 15;

            // Assert
            Assert.That(task.Inputs.Name, Is.EqualTo("tito"));
            Assert.That(task.Inputs.Age, Is.EqualTo(15));
        }

        [Test]
        public void should_update_OutputsModel_when_Outputs_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.Outputs = new {MockObject = new {Name = "tito"}};

            // Assert
            Assert.That(task.OutputsModel.MockObject.Name, Is.EqualTo("tito"));
        }

        [Test]
        public void should_update_OutputsModel_property_when_Outputs_property_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.Outputs = new {MockObject = new {Name = "tito", Age = 10}};
            task.Outputs.MockObject.Age = 15;

            // Assert
            Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("tito"));
            Assert.That(task.Outputs.MockObject.Age, Is.EqualTo(15));
        }

        [Test]
        public void should_update_Outputs_when_OutputsModel_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.OutputsModel = new MockComplexObject {MockObject = new MockObject {Name = "tito"}};

            // Assert
            Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("tito"));
        }

        [Test]
        public void should_update_Outputs_property_when_OutputsModel_property_is_set()
        {
            // Arrange
            var task = TaskFactory<MockTaskTITO>.Create();

            // Act
            task.OutputsModel = new MockComplexObject {MockObject = new MockObject {Name = "tito", Age = 10}};
            task.OutputsModel.MockObject.Age = 15;

            // Assert
            Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("tito"));
            Assert.That(task.Outputs.MockObject.Age, Is.EqualTo(15));
        }
    }
}
