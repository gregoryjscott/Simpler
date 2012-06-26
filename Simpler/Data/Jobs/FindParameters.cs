using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Simpler.Data.Jobs
{
    public class FindParameters : InOutJob<FindParameters.Input, FindParameters.Output> 
    {
        public override void Specs()
        {
            It<FindParameters>.Should(
                "find parameters starting with @",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = @something and something_else is true
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@something",
                               "Parameter name was not @something as expected.");
                });

            It<FindParameters>.Should(
                "find parameters starting with a :",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = :something and something_else is true
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == ":something",
                               "Parameter name was not :something as expected.");
                });

            It<FindParameters>.Should(
                "should find parameters that contain an _",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = @some_thing and something_else is true
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@some_thing",
                               "Parameter name was not @some_thing as expected.");
                });

            It<FindParameters>.Should(
                "find parameters that contain a .",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = @complex.object and something_else is true
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@complex.object",
                               "Parameter name was not @complex.object as expected.");
                });

            It<FindParameters>.Should(
                "find parameters that contain a number",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = @some1thing1 and something_else is true
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@some1thing1",
                               "Parameter name was not @some1thing1 as expected.");
                });

            It<FindParameters>.Should(
                "find parameters followed by a comma",
                it =>
                {
                    it.In.CommandText =
                        @"
                        insert into table set something = @something, something_else = 'whatever'
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@something",
                               "Parameter name was not @something as expected");
                });

            It<FindParameters>.Should(
                "find parameters followed by a carriage return",
                it =>
                {
                    var sql =
                        @"select whatever from table where something = @something\n and something_else is true";

                    sql +=
                        @" 
                        and something_more = @something_more
                        and
                        hopefully that covers it
                        ";

                    it.In.CommandText = sql;
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@something",
                               "First parameter should be @something.");
                    Check.That(it.Out.ParameterNames[1] == "@something_more",
                               "Second paraemter should be @something_more.");
                });

            It<FindParameters>.Should(
                "find parameters at the very end of the command text",
                it =>
                {
                    it.In.CommandText = @"select whatever from table where something = @something";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@something",
                               "Parameter name was not @something as expected.");
                });

            It<FindParameters>.Should(
                "return one instance of a parameter even if the parameter exists multiple times",
                it =>
                {
                    it.In.CommandText =
                        @"
                        select whatever from table where something = @something and somethingelsealso = @something
                        ";
                    it.Run();

                    Check.That(it.Out.ParameterNames[0] == "@something",
                               "Parameter name was not @something as expected.");
                    Check.That(it.Out.ParameterNames.Length == 1,
                               "Only one parameter should be found.");
                });
        }

        public class Input
        {
            public string CommandText { get; set; }
        }

        public class Output
        {
            public string[] ParameterNames { get; set; }
        }

        public override void Run()
        {
            var regularExpression = new StringBuilder();

            // Name the grouping "Parameter", make it start with ":" or "@", then a letter, and followed by up to 29 letters, numbers, or underscores.  Finally, look ahead
            // and make sure the next character is not a letter, number, or underscore.
            regularExpression.Append(@"(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_\.]{0,127})(?=[^a-zA-Z0-9_\.])");

            // Or, allow the same thing as above accept look ahead and allow the string to end immediately after the parameter.
            regularExpression.Append(@"|(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_\.]{0,127})$");

            var regex = new Regex(regularExpression.ToString(), RegexOptions.Multiline);
            var matches = regex.Matches(In.CommandText);

            var parameterNameSet = new HashSet<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                parameterNameSet.Add(matches[i].Groups["Parameter"].Value);
            }

            Out.ParameterNames = new string[parameterNameSet.Count];
            parameterNameSet.CopyTo(Out.ParameterNames);
        }
    }
}
