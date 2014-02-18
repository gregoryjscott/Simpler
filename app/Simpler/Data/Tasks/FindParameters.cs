using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Simpler.Data.Tasks
{
    public class FindParameters: InOutTask<FindParameters.Input, FindParameters.Output>
    {
        public class Input
        {
            public string CommandText { get; set; }
        }

        public class Output
        {
            public string[] ParameterNames { get; set; }
        }

        public override void Execute()
        {
            var regularExpression = new StringBuilder();

            // Name the grouping "Parameter", make it start with ":" or "@", then a letter, and followed by up to 128 letters, numbers, or underscores.  Finally, look ahead
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
