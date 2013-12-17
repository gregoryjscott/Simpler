﻿using System;
using System.Data;
using System.Reflection;

namespace Simpler.Data.Tasks
{
    public class BuildParameters : I<BuildParameters.Ins>
    {
        public class Ins
        {
            public IDbCommand Command { get; set; }
            public object Values { get; set; }
        }

        public FindParameters FindParameters { get; set; }

        public override void Execute()
        {
            FindParameters.In.CommandText = In.Command.CommandText;
            FindParameters.Execute();

            foreach (var parameterNameX in FindParameters.Out.ParameterNames)
            {
                var objectType = In.Values.GetType();
                var values = In.Values;

                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var propertyName = parameterNameX.Substring(1);

                // If the parameter contains a dot then the property must be a complex object, and therefore we must look inside the object to find the value.
                PropertyInfo property;
                while(propertyName.Contains("."))
                {
                    // Look for a property using the string that comes before the dot.
                    var indexOfDot = propertyName.IndexOf(".");
                    property = objectType.GetProperty(propertyName.Substring(0, indexOfDot));

                    // Apparently there isn't a property that is a complex object that matches the parameter name.
                    if (property == null) break;

                    // Reset variables using the property that was found that matched the string that came before the dot. 
                    objectType = property.PropertyType;
                    values = property.GetValue(values, null);
                    propertyName = propertyName.Substring(indexOfDot + 1);
                }

                property = objectType.GetProperty(propertyName);

                if (property != null)
                {
                    var dbDataParameter = In.Command.CreateParameter();

                    // If the property came from a complex object then it contains a dot, and dots aren't allowed in parameter names.
                    dbDataParameter.ParameterName = parameterNameX.Replace(".", "_");
                    In.Command.CommandText = In.Command.CommandText.Replace(parameterNameX, parameterNameX.Replace(".", "_"));

                    dbDataParameter.Value = property.GetValue(values, null) ?? DBNull.Value;
                    In.Command.Parameters.Add(dbDataParameter);
                }
            }
        }
     }
}
