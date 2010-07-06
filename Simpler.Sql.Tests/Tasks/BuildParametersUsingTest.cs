using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Simpler.Sql.Tests.Mocks;
using Simpler.Sql.Tasks;
using System.Data;

namespace Simpler.Sql.Tests.Tasks
{
    [TestFixture]
    public class BuildParametersUsingTest
    {
        [Test]
        public void should_create_parameters_for_all_parameters_found_in_the_command_text()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(new Mock<IDbDataParameter>().Object);
            task.DbCommand = mockDbCommand.Object;

            var mockObject = new MockObject {Name = "John Doe", Age = 21};
            task.Object = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] {"@Name", "@Age"});
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockFindParameters.VerifySet(findParams => findParams.CommandText, It.IsAny<string>());
            mockFindParameters.Verify(findParams => findParams.Execute(), Times.Once());
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(It.IsAny<IDbDataParameter>()), Times.Exactly(2));
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_object()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            task.DbCommand = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.Object = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] { "@Name"});
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Name");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = "John Doe");
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_dbnull_if_value_found_in_the_matching_property_of_the_object_is_null()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            task.DbCommand = mockDbCommand.Object;

            var mockObject = new MockObject { Age = null };
            task.Object = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] { "@Age" });
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Age");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = DBNull.Value);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_not_set_parameter_if_no_matching_property_is_found_in_the_object()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            task.DbCommand = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.Object = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] { "@Whatever" });
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Whatever");
            mockDbDataParameter.VerifySet(parameter => parameter.Value, Times.Never());
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }
    }
}
