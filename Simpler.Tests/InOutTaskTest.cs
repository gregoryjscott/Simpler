using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class InOutTaskTest
    {
        [Test]
        public void should_update_Inputs_when_SetInputs_is_called()
        {
            // Arrange
            var task = TaskFactory<MockInOutTask>.Create();

            // Act
            task.SetInputs(new {Name = "something"});

            // Assert
            Assert.That(task.Inputs.Name, Is.EqualTo("something"));
        }

        [Test]
        public void should_update_Inputs_when_SetInputs_is_sent_a_primitive_type()
        {
            // Arrange
            var task = TaskFactory<MockInOutTaskUsingPrimitives>.Create();

            // Act
            task.SetInputs(4);

            // Assert
            Assert.That(task.Inputs, Is.EqualTo(4));
        }

        [Test]
        public void should_update_Inputs_property_when_Inputs_property_is_set_after_SetInputs()
        {
            // Arrange
            var task = TaskFactory<MockInOutTask>.Create();

            // Act
            task.SetInputs(new {Name = "something", Age = 10});
            task.Inputs.Age = 15;

            // Assert
            Assert.That(task.Inputs.Name, Is.EqualTo("something"));
            Assert.That(task.Inputs.Age, Is.EqualTo(15));
        }

        //[Test]
        //public void should_update_Inputs_when_Inputs_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Inputs = new MockObject {Name = "something"};

        //    // Assert
        //    Assert.That(task.Inputs.Name, Is.EqualTo("something"));
        //}

        //[Test]
        //public void should_update_Inputs_property_when_Inputs_property_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Inputs = new MockObject {Name = "something", Age = 10};
        //    task.Inputs.Age = 15;

        //    // Assert
        //    Assert.That(task.Inputs.Name, Is.EqualTo("something"));
        //    Assert.That(task.Inputs.Age, Is.EqualTo(15));
        //}

        //[Test]
        //public void should_update_OutputsModel_when_Outputs_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Outputs = new {MockObject = new {Name = "something"}};

        //    // Assert
        //    Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("something"));
        //}

        //[Test]
        //public void should_update_OutputsModel_when_Outputs_is_set_to_a_primitive_type()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTaskUsingPrimitives>.Create();

        //    // Act
        //    task.Outputs = "4";

        //    // Assert
        //    Assert.That(task.Outputs, Is.EqualTo("4"));
        //}

        //[Test]
        //public void should_update_OutputsModel_property_when_Outputs_property_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Outputs = new {MockObject = new {Name = "something", Age = 10}};
        //    task.Outputs.MockObject.Age = 15;

        //    // Assert
        //    Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("something"));
        //    Assert.That(task.Outputs.MockObject.Age, Is.EqualTo(15));
        //}

        //[Test]
        //public void should_update_Outputs_when_OutputsModel_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Outputs = new MockComplexObject {MockObject = new MockObject {Name = "something"}};

        //    // Assert
        //    Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("something"));
        //}

        //[Test]
        //public void should_update_Outputs_property_when_OutputsModel_property_is_set()
        //{
        //    // Arrange
        //    var task = TaskFactory<MockInOutTask>.Create();

        //    // Act
        //    task.Outputs = new MockComplexObject {MockObject = new MockObject {Name = "something", Age = 10}};
        //    task.Outputs.MockObject.Age = 15;

        //    // Assert
        //    Assert.That(task.Outputs.MockObject.Name, Is.EqualTo("something"));
        //    Assert.That(task.Outputs.MockObject.Age, Is.EqualTo(15));
        //}
    }
}
