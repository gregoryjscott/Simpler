using System;
using System.Data;
using Moq;
using NUnit.Framework;
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
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select ... doesnt matter");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new MockPerson {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Age"});
            });

            parameter.VerifySet(param => param.ParameterName = "@Age");
            parameter.VerifySet(param => param.Value = 21);
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_create_parameters_for_any_parameters_found_in_the_command_text_with_matching_properties_in_the_anonymous_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select ... doesnt matter");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Age"});
            });

            parameter.VerifySet(param => param.ParameterName = "@Age");
            parameter.VerifySet(param => param.Value = 21);
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_static_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select something from table where @MockObject.Age = 21");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new MockPerson {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Name"});
            });

            parameter.VerifySet(p => p.ParameterName = "@Name");
            parameter.VerifySet(p => p.Value = "John Doe");
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_the_value_found_in_the_matching_property_of_the_anonymous_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select something from table where @MockObject.Age = 21");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Name"});
            });

            parameter.VerifySet(p => p.ParameterName = "@Name");
            parameter.VerifySet(p => p.Value = "John Doe");
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_ignore_parameters_found_in_the_command_text_without_matching_properties_in_the_static_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select something from table where @MockObject.Age = 21");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new MockPerson {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Whatever"});
            });

            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Never());
        }

        [Test]
        public void should_ignore_parameters_found_in_the_command_text_without_matching_properties_in_the_anonymous_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "doesnt matter");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new {Name = "John Doe", Age = 21};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Whatever"});
            });

            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Never());
        }

        [Test]
        public void should_set_parameter_to_dbnull_if_value_found_in_the_matching_property_of_the_static_object_is_null()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "doesnt matter");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new MockPerson {Age = null};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Age"});
            });

            parameter.VerifySet(p => p.ParameterName = "@Age");
            parameter.VerifySet(p => p.Value = DBNull.Value);
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_set_parameter_to_dbnull_if_value_found_in_the_matching_property_of_the_anonymous_object_is_dbnull()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "doesnt matter");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new {Age = DBNull.Value};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@Age"});
            });

            parameter.VerifySet(p => p.ParameterName = "@Age");
            parameter.VerifySet(p => p.Value = DBNull.Value);
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_support_setting_parameter_using_a_complex_static_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select something from table where @MockPerson.Age = 21");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new MockComplexObject {MockPerson = new MockPerson {Name = "John Doe", Age = 21}};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@MockPerson.Age"});
            });

            parameter.VerifySet(param => param.ParameterName = "@MockPerson_Age");
            parameter.VerifySet(param => param.Value = 21);
            command.Verify(dbCommand => dbCommand.Parameters.Add(parameter.Object), Times.Once());
        }

        [Test]
        public void should_support_setting_parameter_using_a_complex_anonymous_object()
        {
            var parameter = new Mock<IDbDataParameter>();
            var command = SetupCommand(parameter.Object, "select something from table where @MockObject.Age = 21");

            Execute.Now<BuildParameters>(bp => {
                bp.In.Command = command.Object;
                bp.In.Values = new {MockObject = new {Name = "John Doe", Age = 21}};
                bp.FindParameters = Fake.Task<FindParameters>(j => j.Out.ParameterNames = new[] {"@MockObject.Age"});
            });

            parameter.VerifySet(param => param.ParameterName = "@MockObject_Age");
            parameter.VerifySet(param => param.Value = 21);
            command.Verify(c => c.Parameters.Add(parameter.Object), Times.Once());
        }

        #region Helpers

        public static Mock<IDbCommand> SetupCommand(IDbDataParameter parameter, string sql)
        {
            var command = new Mock<IDbCommand> {DefaultValue = DefaultValue.Mock};
            command.Setup(c => c.CreateParameter()).Returns(parameter);
            command.Setup(c => c.CommandText).Returns(sql);
            return command;
        }

        #endregion
    }
}
