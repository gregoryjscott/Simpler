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
    }
}
