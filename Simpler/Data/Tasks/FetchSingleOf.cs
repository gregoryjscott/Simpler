using System;
using System.Data;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Tasks
{
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
                if (!dataReader.Read())
                    throw new SingleNotFoundException("No records were found.");

                if(dataReader.Read())
                    throw new SingleNotFoundException("More than one record was found.");

                UseDataRecordToBuild.DataRecord = dataReader;
                UseDataRecordToBuild.Execute();
                ObjectFetched = UseDataRecordToBuild.Object;
            }
        }
    }
}
