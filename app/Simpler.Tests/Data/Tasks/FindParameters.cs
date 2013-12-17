using NUnit.Framework;
using Simpler.Data.Tasks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class FindParametersInCommandTextTest
    {
        [Test]
        public void should_find_parameters_starting_with_an_at_symbol()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = @something and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_starting_with_a_colon()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = :something and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo(":something"));
        }

        [Test]
        public void should_find_parameters_that_contain_an_underscore()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = @some_thing and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@some_thing"));
        }

        [Test]
        public void should_find_parameters_that_contain_a_dot()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = @complex.object and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@complex.object"));
        }

        [Test]
        public void should_find_parameters_that_contain_a_number()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = @some1thing1 and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@some1thing1"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_comma()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                insert into table set something = @something, something_else = 'whatever'
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_carriage_return()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                "select whatever from table where something = @something\n and something_else is true";

            task.In.CommandText +=
                @" 
                and something_more = @something_more
                and
                hopefully that covers it
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@something"));
            Assert.That(task.Out.ParameterNames[1], Is.EqualTo("@something_more"));
        }

        [Test]
        public void should_find_parameter_at_the_very_end_of_the_command_text()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                "select whatever from table where something = @something";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_only_return_one_instance_of_parameter_name_even_if_parameter_exists_in_command_text_more_than_once()
        {
            // Arrange
            var task = T.New<FindParameters>();
            task.In.CommandText =
                @"
                select whatever from table where something = @something and somethingelsealso = @something
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ParameterNames[0], Is.EqualTo("@something"));
            Assert.That(task.Out.ParameterNames.Length, Is.EqualTo(1));
        }
    }
}