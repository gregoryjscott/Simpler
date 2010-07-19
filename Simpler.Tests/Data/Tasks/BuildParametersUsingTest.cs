using System;
using Moq;
using NUnit.Framework;
using Simpler.Data.Interfaces;
using Simpler.Data.Tasks;
using System.Data;
using Simpler.Tests.Data.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildParametersUsingTest
    {
        [Test]
        public void should_create_parameters_for_any_parameters_found_in_the_command_text_with_matching_properties_in_the_object()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(new Mock<IDbDataParameter>().Object);
            task.CommandWithParameters = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.ObjectWithValues = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] { "@Name", "@Age" });
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockFindParameters.VerifySet(findParams => findParams.CommandText, It.IsAny<string>());
            mockFindParameters.Verify(findParams => findParams.Execute(), Times.Once());
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(It.IsAny<IDbDataParameter>()), Times.Exactly(2));
        }

        [Test]
        public void should_not_create_parameters_for_parameters_found_in_the_command_text_without_matching_properties_in_the_object()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(new Mock<IDbDataParameter>().Object);
            task.CommandWithParameters = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.ObjectWithValues = mockObject;

            var mockFindParameters = new Mock<IFindParametersInCommandText>();
            mockFindParameters.Setup(findParams => findParams.ParameterNames).Returns(new string[] { "@Whatever" });
            task.FindParametersInCommandText = mockFindParameters.Object;

            // Act
            task.Execute();

            // Assert
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(It.IsAny<IDbDataParameter>()), Times.Never());
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_object()
        {
            // Arrange
            var task = new BuildParametersUsing<MockObject>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            task.CommandWithParameters = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.ObjectWithValues = mockObject;

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
            task.CommandWithParameters = mockDbCommand.Object;

            var mockObject = new MockObject { Age = null };
            task.ObjectWithValues = mockObject;

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
    }
}
