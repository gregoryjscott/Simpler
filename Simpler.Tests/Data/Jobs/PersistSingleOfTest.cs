using NUnit.Framework;
using Moq;
using System.Data;
using Simpler.Sql.Exceptions;
using Simpler.Sql.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    class PersistSingleOfTest
    {
        [Test]
        public void should_call_command_to_persist_the_object()
        {
            // Arrange
            var job = Job.New<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            job.BuildParameters = mockBuildParameters.Object;

            var mockObject = new MockObject();
            job.ObjectToPersist = mockObject;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(1);
            job.PersistCommand = mockPersistCommand.Object;

            // Act
            job.Run();
            
            // Assert
            mockBuildParameters.VerifySet(buildParams => buildParams.CommandWithParameters = mockPersistCommand.Object);
            mockBuildParameters.VerifySet(buildParams => buildParams.ObjectWithValues = mockObject);
            mockPersistCommand.Verify(buildParams => buildParams.ExecuteNonQuery(), Times.Once());
        }

        [Test]
        public void should_throw_exception_if_no_database_rows_are_affected()
        {
            // Arrange
            var job = Job.New<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            job.BuildParameters = mockBuildParameters.Object;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(0);
            job.PersistCommand = mockPersistCommand.Object;

            // Act & Assert
            Assert.Throws(typeof(ObjectPersistanceException), job.Run);
        }

        [Test]
        public void should_throw_exception_if_more_than_one_database_row_is_affected()
        {
            // Arrange
            var job = Job.New<PersistSingleOf<MockObject>>();

            var mockBuildParameters = new Mock<BuildParametersUsing<MockObject>>();
            job.BuildParameters = mockBuildParameters.Object;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(2);
            job.PersistCommand = mockPersistCommand.Object;

            // Act & Assert
            Assert.Throws(typeof(ObjectPersistanceException), job.Run);
        }
    }
}
