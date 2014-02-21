using System;
using System.Data;

namespace Simpler.Data.Tasks
{
    public class BuildParameters: InTask<BuildParameters.Input>
    {
        public class Input
        {
            public IDbCommand Command { get; set; }
            public object Values { get; set; }
        }

        public FindParameters FindParameters { get; set; }

        public override void Execute()
        {
            FindParameters.In.CommandText = In.Command.CommandText;
            FindParameters.Execute();
            var parameterNames = FindParameters.Out.ParameterNames;

            var parameterValues = In.Values;
            var objectType = In.Values.GetType();

            foreach (var parameterName in parameterNames) 
            {
                var propertyName = RemoveParameterNotation(parameterName);

                while (PropertyIsComplex(propertyName)) 
                {
                    var indexOfDot = propertyName.IndexOf(".", StringComparison.Ordinal);
                    var nameBeforeDot = propertyName.Substring(0, indexOfDot);
                    var propertyBeforeDot = objectType.GetProperty(nameBeforeDot);
                    if (propertyBeforeDot == null) break;

                    objectType = propertyBeforeDot.PropertyType;
                    parameterValues = propertyBeforeDot.GetValue(parameterValues, null);
                    propertyName = RemoveDotAndEverythingBeforeIt(propertyName, indexOfDot);
                }

                var property = objectType.GetProperty(propertyName);
                if (property == null) continue;

                var newParameterName = ReplaceDots(parameterName);
                In.Command.CommandText = In.Command.CommandText.Replace(parameterName, newParameterName);

                var parameter = In.Command.CreateParameter();
                parameter.ParameterName = newParameterName;
                parameter.Value = property.GetValue(parameterValues, null) ?? DBNull.Value;
                In.Command.Parameters.Add(parameter);
            }
        }

        #region Helpers

        static bool PropertyIsComplex(string propertyName) { return propertyName.Contains("."); }

        static string RemoveParameterNotation(string parameterName) { return parameterName.Substring(1); }

        static string RemoveDotAndEverythingBeforeIt(string propertyName, int indexOfDot) { return propertyName.Substring(indexOfDot + 1); }

        static string ReplaceDots(string stringWithDots) { return stringWithDots.Replace(".", "_"); }

        #endregion
    }
}
