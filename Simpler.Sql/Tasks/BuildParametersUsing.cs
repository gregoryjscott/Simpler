using System;
using System.Data;

namespace Simpler.Sql.Tasks
{
    public class BuildParametersUsing<T> : Task, IBuildParametersUsing<T>
    {
        // Inputs
        public IDbCommand DbCommand { get; set; }
        public T Object { get; set; }

        // Sub-tasks
        public IFindParametersInCommandText FindParametersInCommandText { private get; set; }

        public override void Execute()
        {
            // Create the sub-tasks if null (this won't be necessary after dependency injection is implemented).
            if (FindParametersInCommandText == null) FindParametersInCommandText = new FindParametersInCommandText();
            FindParametersInCommandText.CommandText = DbCommand.CommandText;
            FindParametersInCommandText.Execute();

            var objectType = typeof(T);

            foreach (var parameterNameX in FindParametersInCommandText.ParameterNames)
            {
                IDbDataParameter dbDataParameter = DbCommand.CreateParameter();
                dbDataParameter.ParameterName = parameterNameX;

                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var propertyNameToFind = parameterNameX.Substring(1);

                var property = objectType.GetProperty(propertyNameToFind);
                if (property != null)
                {
                    dbDataParameter.Value = property.GetValue(Object, null) ?? DBNull.Value;
                }

                DbCommand.Parameters.Add(dbDataParameter);
            }

        }
     }
}
