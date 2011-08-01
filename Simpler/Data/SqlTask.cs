using System;

namespace Simpler.Data
{
    /// <summary>
    /// todo - currently just a prototype of the desired syntax...
    /// 
    /// default db?  default command?  
    /// </summary>
    public abstract class SqlTask : Task
    {
        protected string Sql { get; set; }

        protected dynamic FetchList()
        {
            throw new NotImplementedException();
        }

        // Example usage.
        class FetchEverything : SqlTask
        {
            public override void Execute()
            {
                Sql = @"
                select * from alltables
                ";

                Outputs = FetchList();
            }
        }
    }
}
