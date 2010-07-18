using System;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class BuildParametersUsing<T> : Task, IBuildParametersUsing<T>
    {
        // Inputs
        public IDbCommand CommandWithParameters { get; set; }
        public T ObjectWithValues { get; set; }

        // Sub-tasks
        public IFindParametersInCommandText FindParametersInCommandText { private get; set; }

        public override void Execute()
        {
            // Create the sub-tasks if null (this won't be necessary after dependency injection is implemented).
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
