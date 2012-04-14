using NUnit.Framework;
using Simpler.Sql.Jobs;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class FindParametersInCommandTextTest
    {
        [Test]
        public void should_find_parameters_starting_with_an_at_symbol()
        {
            const string sql = @"
                select whatever from table where something = @something and something_else is true
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@something")));
        }

        [Test]
        public void should_find_parameters_starting_with_a_colon()
        {
            const string sql = @"
                select whatever from table where something = :something and something_else is true
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo(":something")));
        }

        [Test]
        public void should_find_parameters_that_contain_an_underscore()
        {
            const string sql = @"
                select whatever from table where something = @some_thing and something_else is true
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@some_thing")));
        }

        [Test]
        public void should_find_parameters_that_contain_a_dot()
        {
            const string sql = @"
                select whatever from table where something = @complex.object and something_else is true
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@complex.object")));
        }

        [Test]
        public void should_find_parameters_that_contain_a_number()
        {
            const string sql = @"
                select whatever from table where something = @some1thing1 and something_else is true
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@some1thing1")));
        }

        [Test]
        public void should_find_parameters_followed_by_a_comma()
        {
            const string sql = @"
                insert into table set something = @something, something_else = 'whatever'
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@something")));
        }

        [Test]
        public void should_find_parameters_followed_by_a_carriage_return()
        {
            var sql = @"select whatever from table where something = @something\n and something_else is true";

            sql +=
                @" 
                and something_more = @something_more
                and
                hopefully that covers it
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t =>
                            {
                                Assert.That(t.ParameterNames[0], Is.EqualTo("@something"));
                                Assert.That(t.ParameterNames[1], Is.EqualTo("@something_more"));
                            });
        }

        [Test]
        public void should_find_parameter_at_the_very_end_of_the_command_text()
        {
            const string sql = @"
                select whatever from table where something = @something
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t => Assert.That(t.ParameterNames[0], Is.EqualTo("@something")));
        }

        [Test]
        public void should_only_return_one_instance_of_parameter_name_even_if_parameter_exists_in_command_text_more_than_once()
        {
            const string sql = @"
                select whatever from table where something = @something and somethingelsealso = @something
                ";

            Test<FindParametersInCommandText>.New()
                .Arrange(t => t.CommandText = sql)
                .Act()
                .Assert(t =>
                            {
                                Assert.That(t.ParameterNames[0], Is.EqualTo("@something"));
                                Assert.That(t.ParameterNames.Length, Is.EqualTo(1));
                            });
        }
    }
}
