using System;
using System.Data;
using System.Reflection;

namespace Simpler.Data.Tasks
{
    public class BuildParameters : Task
    {
        public override void Specs()
        {
            // todo
        }

        // Inputs
        public virtual IDbCommand Command { get; set; }
        public virtual object Values { get; set; }

        // Sub-tasks
        public virtual FindParameters FindParameters { get; set; }

        public override void Run()
        {
            // Create the sub-tasks.
            if (FindParameters == null) FindParameters = new FindParameters();

            FindParameters.In.CommandText = Command.CommandText;
            FindParameters.Run();

            foreach (var parameterNameX in FindParameters.Out.ParameterNames)
            {
                var objectType = Values.GetType();
                var objectContainingPropertyValue = Values;

                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var nameOfPropertyContainingValue = parameterNameX.Substring(1);

                // If the parameter contains a dot then the property must be a complex object, and therefore we must look inside the object to find the value.
                PropertyInfo property;
                while(nameOfPropertyContainingValue.Contains("."))
                {
                    // Look for a property using the string that comes before the dot.
                    var indexOfDot = nameOfPropertyContainingValue.IndexOf(".");
                    property = objectType.GetProperty(nameOfPropertyContainingValue.Substring(0, indexOfDot));

                    // Apparently there isn't a property that is a complex object that matches the parameter name.
                    if (property == null) break;

                    // Reset variables using the property that was found that matched the string that came before the dot. 
                    objectType = property.PropertyType;
                    objectContainingPropertyValue = property.GetValue(objectContainingPropertyValue, null);
                    nameOfPropertyContainingValue = nameOfPropertyContainingValue.Substring(indexOfDot + 1);
                }

                property = objectType.GetProperty(nameOfPropertyContainingValue);
                
                // If the property is null and the ObjectWithValues is an anonymous type, then create the parameter and set it
                // to DBNull.  Otherwise if the ObjectWithValues is a static then just ignore any parameters that don't have
                // matching properties.
                if (((objectType.FullName != null) && objectType.FullName.Contains("AnonymousType"))
                    ||
                    (property != null))
                {
                    var dbDataParameter = Command.CreateParameter();

                    // If the property came from a complex object then it contains a dot, and dots aren't allowed in parameter names.
                    dbDataParameter.ParameterName = parameterNameX.Replace(".", "_");
                    Command.CommandText = Command.CommandText.Replace(parameterNameX, parameterNameX.Replace(".", "_"));

                    dbDataParameter.Value =
                        property != null
                        ? property.GetValue(objectContainingPropertyValue, null) ?? DBNull.Value
                        : DBNull.Value;

                    Command.Parameters.Add(dbDataParameter);
                }
            }
        }
     }
}
