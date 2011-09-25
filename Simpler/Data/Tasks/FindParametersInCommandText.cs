using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NUnit.Framework;
using Simpler.Testing;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will search the given command text for parameter placeholders.  Placeholders can begin with a ":" or a "@".
    /// </summary>
    public class FindParametersInCommandText : Task
    {
        // Inputs
        public virtual string CommandText { get; set; }

        // Outputs
        public virtual string[] ParameterNames { get; private set; }

        public override void Execute()
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

        /// <summary>
        /// Provides tests for FindParametersInCommandText."/>
        /// </summary>
        public Test[] Tests
        {
            get
            {
                return new[]
                   {
                       new TestFor<FindParametersInCommandText>
                       {
                           Expectation = "should find parameters starting with an @",

                           Setup =
                               () =>
                               {
                                   var task = TaskFactory<FindParametersInCommandText>.Create();
                                   task.CommandText = @"select ... where something = @something and something_else is true";
                                   return task;
                               },

                           Verify =
                               (task) =>
                               {
                                   Assert.That(task.ParameterNames.Length, Is.EqualTo(1));
                                   Assert.That(task.ParameterNames[0], Is.EqualTo("@something"));
                               }
                       },
                       new TestFor<FindParametersInCommandText>
                       {
                           Expectation = "should find parameters starting with a :",

                           Setup =
                               () =>
                               {
                                   var task = TaskFactory<FindParametersInCommandText>.Create();
                                   task.CommandText = @"select ... where something = :something and something_else is true";
                                   return task;
                               },

                           Verify =
                               (task) =>
                               {
                                   Assert.That(task.ParameterNames.Length, Is.EqualTo(1));
                                   Assert.That(task.ParameterNames[0], Is.EqualTo(":something"));
                               }
                       }
                   };
            }
        }
    }
}
