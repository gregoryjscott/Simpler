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
            var task = new FindParametersInCommandText();
            task.CommandText =
                @"
                select whatever from table where something = @something and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_starting_with_a_colon()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                @"
                select whatever from table where something = :something and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo(":something"));
        }

        [Test]
        public void should_find_parameters_that_contain_an_underscore()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                @"
                select whatever from table where something = @some_thing and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@some_thing"));
        }

        [Test]
        public void should_find_parameters_that_contain_a_number()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                @"
                select whatever from table where something = @some1thing1 and something_else is true
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@some1thing1"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_comma()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                @"
                insert into table set something = @something, something_else = 'whatever'
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_carriage_return()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                "select whatever from table where something = @something\n and something_else is true";

            task.CommandText +=
                @" 
                and something_more = @something_more
                and
                hopefully that covers it
                ";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@something"));
            Assert.That(task.ParameterNames[1], Is.EqualTo("@something_more"));
        }

        [Test]
        public void should_find_parameter_at_the_very_end_of_the_command_text()
        {
            // Arrange
            var task = new FindParametersInCommandText();
            task.CommandText =
                "select whatever from table where something = @something";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ParameterNames[0], Is.EqualTo("@something"));
        }
    }
}
