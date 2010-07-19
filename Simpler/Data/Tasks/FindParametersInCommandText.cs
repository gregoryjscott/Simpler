using System.Text;
using System.Text.RegularExpressions;

namespace Simpler.Data.Tasks
{
    public class FindParametersInCommandText : Task, IFindParametersInCommandText
    {
        public string CommandText { get; set; }
        public string[] ParameterNames { get; private set; }

        public override void Execute()
        {
            var regularExpression = new StringBuilder();

            // Name the grouping "Parameter", make it start with ":" or "@", then a letter, and followed by up to 29 letters, numbers, or underscores.  Finally, look ahead
            // and make sure the next character is not a letter, number, or underscore.
            regularExpression.Append(@"(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_]{0,29})(?=[^a-zA-Z0-9_])");

            // Or, allow the same thing as above accept look ahead and allow the string to end immediately after the parameter.
            regularExpression.Append(@"|(?<Parameter>[:@][a-zA-Z][a-zA-Z0-9_]{0,29})$");

            var regex = new Regex(regularExpression.ToString(), RegexOptions.Multiline);
            var matches = regex.Matches(CommandText);

            ParameterNames = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
            {
                ParameterNames[i] = matches[i].Groups["Parameter"].Value;
            }
        }
    }
}
