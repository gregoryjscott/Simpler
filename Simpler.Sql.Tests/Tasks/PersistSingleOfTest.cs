using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Simpler.Sql.Tasks;
using Simpler.Sql.Tests.Mocks;
using Moq;
using System.Data;
using Simpler.Sql.Exceptions;

namespace Simpler.Sql.Tests.Tasks
{
    [TestFixture]
    class PersistSingleOfTest
    {
        [Test]
        public void should_call_command_to_persist_the_object()
        {
            // Arrange
            var task = new PersistSingleOf<MockObject>();

            var mockBuildParameters = new Mock<IBuildParametersUsing<MockObject>>();
            task.BuildParameters = mockBuildParameters.Object;

            var mockObject = new MockObject();
            task.ObjectToPersist = mockObject;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(1);
            task.PersistCommand = mockPersistCommand.Object;

            // Act
            task.Execute();
            
            // Assert
            mockBuildParameters.VerifySet(buildParams => buildParams.DbCommand = mockPersistCommand.Object);
            mockBuildParameters.VerifySet(buildParams => buildParams.Object = mockObject);
            mockPersistCommand.Verify(buildParams => buildParams.ExecuteNonQuery(), Times.Once());
        }

        [Test]
        public void should_throw_exception_if_no_database_rows_are_affected()
        {
            // Arrange
            var task = new PersistSingleOf<MockObject>();

            var mockBuildParameters = new Mock<IBuildParametersUsing<MockObject>>();
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
            var task = new PersistSingleOf<MockObject>();

            var mockBuildParameters = new Mock<IBuildParametersUsing<MockObject>>();
            task.BuildParameters = mockBuildParameters.Object;

            var mockPersistCommand = new Mock<IDbCommand>();
            mockPersistCommand.Setup(command => command.ExecuteNonQuery()).Returns(2);
            task.PersistCommand = mockPersistCommand.Object;

            // Act & Assert
            Assert.Throws(typeof(ObjectPersistanceException), task.Execute);
        }
    }
}
