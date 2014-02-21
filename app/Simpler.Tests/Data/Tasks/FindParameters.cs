using NUnit.Framework;
using Simpler.Data;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class FindParametersTest
    {
        [Test]
        public void should_find_parameters_starting_with_an_at_symbol()
        {
            const string sql = @"
                select whatever from table where something = @something and something_else is true
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_starting_with_a_colon()
        {
            const string sql = @"
                select whatever from table where something = :something and something_else is true
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo(":something"));
        }

        [Test]
        public void should_find_parameters_that_contain_an_underscore()
        {
            const string sql = @"
                select whatever from table where something = @some_thing and something_else is true
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@some_thing"));
        }

        [Test]
        public void should_find_parameters_that_contain_a_dot()
        {
            const string sql = @"
                select whatever from table where something = @complex.object and something_else is true
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@complex.object"));
        }

        [Test]
        public void should_find_parameters_that_contain_a_number()
        {
            const string sql = @"
                select whatever from table where something = @some1thing1 and something_else is true
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@some1thing1"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_comma()
        {
            const string sql = @"
                insert into table set something = @something, something_else = 'whatever'
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_find_parameters_followed_by_a_carriage_return()
        {
            const string sql = @"
                select whatever from table where something = @something\n and something_else is true
                and something_more = @something_more
                and
                hopefully that covers it
            ";

            var parameters = Db.Core.FindParameters(sql);
            Assert.That(parameters[0], Is.EqualTo("@something"));
            Assert.That(parameters[1], Is.EqualTo("@something_more"));
        }

        [Test]
        public void should_find_parameter_at_the_very_end_of_the_command_text()
        {
            const string sql = @"
                select whatever from table where something = @something
            ";

            Assert.That(Db.Core.FindParameters(sql)[0], Is.EqualTo("@something"));
        }

        [Test]
        public void should_only_return_one_instance_of_parameter_name_even_if_parameter_exists_in_command_text_more_than_once()
        {
            const string sql = @"
                select whatever from table where something = @something and somethingelsealso = @something
            ";

            var findParameters =  Db.Core.FindParameters(sql);
            Assert.That(findParameters[0], Is.EqualTo("@something"));
            Assert.That(findParameters.Length, Is.EqualTo(1));
        }
    }
}