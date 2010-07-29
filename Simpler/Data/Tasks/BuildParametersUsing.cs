using System;
using System.Data;
using System.Reflection;

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

            foreach (var parameterNameX in FindParametersInCommandText.ParameterNames)
            {
                var objectType = typeof(T);
                object objectContainingPropertyValue = ObjectWithValues;

                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var nameOfpropertyContainingValue = parameterNameX.Substring(1);

                // If the parameter contains a dot then the property must be a complex object, and therefore we must look inside the object to find the value.
                PropertyInfo property;
                while(nameOfpropertyContainingValue.Contains("."))
                {
                    // Look for a property using the string that comes before the dot.
                    var indexOfDot = nameOfpropertyContainingValue.IndexOf(".");
                    property = objectType.GetProperty(nameOfpropertyContainingValue.Substring(0, indexOfDot));

                    // Apparently there isn't a property that is a complex object that matches the parameter name.
                    if (property == null) break;

                    // Reset variables using the property that was found that matched the string that came before the dot. 
                    objectType = property.PropertyType;
                    objectContainingPropertyValue = property.GetValue(objectContainingPropertyValue, null);
                    nameOfpropertyContainingValue = nameOfpropertyContainingValue.Substring(indexOfDot + 1);
                }

                property = objectType.GetProperty(nameOfpropertyContainingValue);
                if (property != null)
                {
                    IDbDataParameter dbDataParameter = CommandWithParameters.CreateParameter();
                    dbDataParameter.ParameterName = parameterNameX.Replace(".", "_");

                    dbDataParameter.Value = property.GetValue(objectContainingPropertyValue, null) ?? DBNull.Value;

                    CommandWithParameters.Parameters.Add(dbDataParameter);
                }
            }

        }
     }
}
