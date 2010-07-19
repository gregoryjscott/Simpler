using System;
using System.Data;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Tasks
{
    public class PersistSingleOf<T> : Task
    {
        // Inputs
        public IDbCommand PersistCommand { get; set; }
        public T ObjectToPersist { get; set; }

        // Sub-tasks
        public IBuildParametersUsing<T> BuildParameters { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks if null (this won't be necessary after dependency injection is implemented).
            if (BuildParameters == null) BuildParameters = new BuildParametersUsing<T>();
            BuildParameters.CommandWithParameters = PersistCommand;
            BuildParameters.ObjectWithValues = ObjectToPersist;
            BuildParameters.Execute();

            var rowsPersisted = PersistCommand.ExecuteNonQuery();

            if (rowsPersisted != 1)
            {
                throw new ObjectPersistanceException(String.Format("Expected 1 row to be persisted, but actual count was {0}.", rowsPersisted));
            }
        }
    }
}
