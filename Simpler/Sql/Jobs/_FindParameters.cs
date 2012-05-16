using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Simpler.Sql.Jobs
{
    public class _FindParameters : Job
    {
        // Inputs
        public virtual string CommandText { get; set; }

        // Outputs
        public virtual string[] ParameterNames { get; private set; }

        public override void Run()
        {
            var regularExpression = new StringBuilder();

            // Name the grouping "Parameter", make it start with ":" or "@", then a letter, and followed by up to 29 letters, numbers, or underscores.  Finally, look ahead
            // and make sure the next character is not a letter, number, or underscore.
            regularExpression.Append(@"(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_\.]{0,127})(?=[^a-zA-Z0-9_\.])");

            // Or, allow the same thing as above accept look ahead and allow the string to end immediately after the parameter.
            regularExpression.Append(@"|(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_\.]{0,127})$");

            var regex = new Regex(regularExpression.ToString(), RegexOptions.Multiline);
            var matches = regex.Matches(CommandText);

            var parameterNameSet = new HashSet<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                parameterNameSet.Add(matches[i].Groups["Parameter"].Value);
            }
            ParameterNames = new string[parameterNameSet.Count];
            parameterNameSet.CopyTo(ParameterNames);
        }

        public override void Test()
        {
            It<_FindParameters>.Should(
                "find parameters starting with @",
                job =>
                    {
                        job.CommandText =
                            @"
                            select whatever from table where something = @something and something_else is true
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@something");
                    });

            It<_FindParameters>.Should(
                "find parameters starting with a :",
                job =>
                    {
                        job.CommandText =
                            @"
                            select whatever from table where something = :something and something_else is true
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == ":something");
                    });

            It<_FindParameters>.Should(
                "should find parameters that contain an _",
                job =>
                    {
                        job.CommandText =
                            @"
                              select whatever from table where something = @some_thing and something_else is true
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@some_thing");
                    });

            It<_FindParameters>.Should(
                "find parameters that contain a .",
                job =>
                    {
                        job.CommandText =
                            @"
                            select whatever from table where something = @complex.object and something_else is true
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@complex.object");
                    });

            It<_FindParameters>.Should(
                "find parameters that contain a number",
                job =>
                    {
                        job.CommandText =
                            @"
                            select whatever from table where something = @some1thing1 and something_else is true
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@some1thing1");
                    });

            It<_FindParameters>.Should(
                "find parameters followed by a comma",
                job =>
                    {
                        job.CommandText =
                            @"
                            insert into table set something = @something, something_else = 'whatever'
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@something");
                    });

            It<_FindParameters>.Should(
                "find parameters followed by a carriage return",
                job =>
                    {
                        var sql =
                            @"select whatever from table where something = @something\n and something_else is true";

                        sql +=
                            @" 
                            and something_more = @something_more
                            and
                            hopefully that covers it
                            ";

                        job.CommandText = sql;
                        job.Run();

                        Check(job.ParameterNames[0] == "@something");
                        Check(job.ParameterNames[1] == "@something_more");
                    });

            It<_FindParameters>.Should(
                "find parameters at the very end of the command text",
                job =>
                    {
                        job.CommandText = @"select whatever from table where something = @something";
                        job.Run();

                        Check(job.ParameterNames[0] == "@something");
                    });

            It<_FindParameters>.Should(
                "return one instance of a parameter even if the parameter exists multiple times",
                job =>
                    {
                        job.CommandText =
                            @"
                            select whatever from table where something = @something and somethingelsealso = @something
                            ";
                        job.Run();

                        Check(job.ParameterNames[0] == "@something");
                        Check(job.ParameterNames.Length == 1);
                    });
        }
    }
}
