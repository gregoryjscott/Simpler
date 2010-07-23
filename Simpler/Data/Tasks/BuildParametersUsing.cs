using System;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class BuildParametersUsing<T> : Task
    {
        // Inputs
        public virtual IDbCommand CommandWithParameters { get; set; }
        public virtual T ObjectWithValues { get; set; }

        // Sub-tasks
        public virtual FindParametersInCommandText FindParametersInCommandText { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (FindParametersInCommandText == null) FindParametersInCommandText = new FindParametersInCommandText();

            FindParametersInCommandText.CommandText = CommandWithParameters.CommandText;
            FindParametersInCommandText.Execute();

            var objectType = typeof(T);

            foreach (var parameterNameX in FindParametersInCommandText.ParameterNames)
            {
                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var propertyNameToFind = parameterNameX.Substring(1);

                var property = objectType.GetProperty(propertyNameToFind);
                if (property != null)
                {
                    IDbDataParameter dbDataParameter = CommandWithParameters.CreateParameter();
                    dbDataParameter.ParameterName = parameterNameX;

                    dbDataParameter.Value = property.GetValue(ObjectWithValues, null) ?? DBNull.Value;

                    CommandWithParameters.Parameters.Add(dbDataParameter);
                }
            }

        }
     }
}
