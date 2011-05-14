using System.Data;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will create an object of the given type T using the result of the given command.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    public class FetchSingleOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T ObjectFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuild<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                // If no rows, throw an error.
                if (!dataReader.Read()) throw new SingleNotFoundException("No records were found.");

                // Use the first row to build the instance of T.
                UseDataRecordToBuild.DataRecord = dataReader;
                UseDataRecordToBuild.Execute();
                ObjectFetched = UseDataRecordToBuild.Object;

                // We're done with the reader, but make sure there isn't another row.
                if (dataReader.Read()) throw new SingleNotFoundException("More than one record was found.");
            }
        }
    }
}
