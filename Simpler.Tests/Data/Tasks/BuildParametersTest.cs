using System;
using Moq;
using NUnit.Framework;
using System.Data;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildParametersTest
    {
        [Test]
        public void should_create_parameters_for_any_parameters_found_in_the_command_text_with_matching_properties_in_the_static_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.Values = mockObject;

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Age" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Age" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(param => param.ParameterName = "@Age");
            mockDbDataParameter.VerifySet(param => param.Value = 21);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_create_parameters_for_any_parameters_found_in_the_command_text_with_matching_properties_in_the_anonymous_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            task.Values = new { Name = "John Doe", Age = 21 };

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.In).Returns(new FindParameters.Input());
            //mockFindParameters.Setup(findParams => findParams.Out).Returns(new FindParameters.Output { ParameterNames = new[] { "@Age" } });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Age"});

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(param => param.ParameterName = "@Age");
            mockDbDataParameter.VerifySet(param => param.Value = 21);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_static_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.Values = mockObject;

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Name" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Name" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Name");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = "John Doe");
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_anonymous_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            task.Values = new { Name = "John Doe", Age = 21 };

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Name" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Name" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Name");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = "John Doe");
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_ignore_parameters_found_in_the_command_text_without_matching_properties_in_the_static_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            var mockObject = new MockObject { Name = "John Doe", Age = 21 };
            task.Values = mockObject;

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Whatever" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Whatever" });

            // Act & Assert
            task.Execute();

            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Never());

        }

        [Test]
        public void should_set_parameter_to_dbnull_if_value_found_in_the_matching_property_of_the_static_object_is_null()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            var mockObject = new MockObject {Age = null};
            task.Values = mockObject;

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Age" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Age" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Age");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = DBNull.Value);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_dbnull_if_a_matching_property_is_not_found_in_the_anonymous_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("doesnt matter");
            task.Command = mockDbCommand.Object;

            task.Values = new { NotAge = "A" };

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@Age" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@Age" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(parameter => parameter.ParameterName = "@Age");
            mockDbDataParameter.VerifySet(parameter => parameter.Value = DBNull.Value);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
        }

        [Test]
        public void should_support_setting_parameter_using_a_complex_static_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("select something from table where @MockObject.Age = 21");
            task.Command = mockDbCommand.Object;

            var mockComplexObject = new MockComplexObject { MockObject = new MockObject { Name = "John Doe", Age = 21 } };
            task.Values = mockComplexObject;

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new string[] { "@MockObject.Age" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@MockObject.Age" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(param => param.ParameterName = "@MockObject_Age");
            mockDbDataParameter.VerifySet(param => param.Value = 21);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
            mockDbCommand.VerifySet(dbCommand => dbCommand.CommandText = "select something from table where @MockObject_Age = 21");
        }

        [Test]
        public void should_support_setting_parameter_using_a_complex_anonymous_object()
        {
            // Arrange
            var task = Task.New<BuildParameters>();

            var mockDbCommand = new Mock<IDbCommand> { DefaultValue = DefaultValue.Mock };
            var mockDbDataParameter = new Mock<IDbDataParameter>();
            mockDbCommand.Setup(dbCommand => dbCommand.CreateParameter()).Returns(mockDbDataParameter.Object);
            mockDbCommand.Setup(dbCommand => dbCommand.CommandText).Returns("select something from table where @MockObject.Age = 21");
            task.Command = mockDbCommand.Object;

            task.Values = new { MockObject = new { Name = "John Doe", Age = 21 } };

            //var mockFindParameters = new Mock<FindParameters>();
            //mockFindParameters.Setup(findParams => findParams.Out.ParameterNames).Returns(new[] { "@MockObject.Age" });
            //task.FindParameters = mockFindParameters.Object;
            task.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] { "@MockObject.Age" });

            // Act
            task.Execute();

            // Assert
            mockDbDataParameter.VerifySet(param => param.ParameterName = "@MockObject_Age");
            mockDbDataParameter.VerifySet(param => param.Value = 21);
            mockDbCommand.Verify(dbCommand => dbCommand.Parameters.Add(mockDbDataParameter.Object), Times.Once());
            mockDbCommand.VerifySet(dbCommand => dbCommand.CommandText = "select something from table where @MockObject_Age = 21");
        }
    }
}
