using System.IO;

namespace Simpler.Data
{
    public static class TaskExtensions
    {
        public static string Sql(this Task task)
        {
            var assembly = task.GetType().BaseType.Assembly;
            var resourceName = task.Name.Replace(".Tasks.", ".Sql.") + ".sql";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}