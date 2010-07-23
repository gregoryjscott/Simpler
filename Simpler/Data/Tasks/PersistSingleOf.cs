using System;
using System.Data;
using Simpler.Data.Exceptions;
using Simpler.Injection;

namespace Simpler.Data.Tasks
{
    [InjectSubTasks]
    public class PersistSingleOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand PersistCommand { get; set; }
        public virtual T ObjectToPersist { get; set; }

        // Sub-tasks
        public virtual BuildParametersUsing<T> BuildParameters { get; set; }

        public override void Execute()
        {
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
