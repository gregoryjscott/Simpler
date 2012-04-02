using NUnit.Framework;
using Simpler.Data.Tasks;
using Moq;
using System.Data;
using Simpler.Data.Exceptions;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    class PersistSingleOfTest
    {
        [Test]
        public void should_call_command_to_persist_the_object()
        {
            // Arrange
            var task = Task.Create<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            task.BuildParameters = mockBuildParameters.Object;

            var mockObject = new MockObject();
            task.ObjectToPersist = mockObject;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(1);
            task.PersistCommand = mockPersistCommand.Object;

            // Act
            task.Execute();
            
            // Assert
            mockBuildParameters.VerifySet(buildParams => buildParams.CommandWithParameters = mockPersistCommand.Object);
            mockBuildParameters.VerifySet(buildParams => buildParams.ObjectWithValues = mockObject);
            mockPersistCommand.Verify(buildParams => buildParams.ExecuteNonQuery(), Times.Once());
        }

        [Test]
        public void should_throw_exception_if_no_database_rows_are_affected()
        {
            // Arrange
            var task = Task.Create<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            task.BuildParameters = mockBuildParameters.Object;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(0);
            task.PersistCommand = mockPersistCommand.Object;

            // Act & Assert
            Assert.Throws(typeof(ObjectPersistanceException), task.Execute);
        }

        [Test]
        public void should_throw_exception_if_more_than_one_database_row_is_affected()
        {
            // Arrange
            var task = Task.Create<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            task.BuildParameters = mockBuildParameters.Object;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(2);
            task.PersistCommand = mockPersistCommand.Object;

            // Act & Assert
            Assert.Throws(typeof(ObjectPersistanceException), task.Execute);
        }
    }
}
