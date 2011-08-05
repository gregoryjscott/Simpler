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

        // Example usage.
        class FetchList : Task
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        class FetchCardinals : Task
        {
            // Sub-tasks
            FetchList FetchPitchers { get; set; }
            FetchList FetchCatchers { get; set; }
            FetchList FetchInfielders { get; set; }
            FetchList FetchOutfielders { get; set; }

            public override void Execute()
            {
                Outputs = new
                {
                    Team = "Cardinals",

                    Manager = "Mr. Lucky",

                    Pitchers = FetchPitchers.Execute(new
                    {
                        Sql = 
                            @"
                            select * from players where position = 1
                            "
                    }),

                    Catchers = FetchCatchers.Execute(new
                    {
                        Sql = 
                            @"
                            select * from players where position = 2
                            "
                    }),

                    Infielders = FetchInfielders.Execute(new
                    {
                        Sql = 
                            @"
                            select * from players where position in (3, 4, 5, 6)
                            "
                    }),

                    Outfielders = FetchOutfielders.Execute(new
                    {
                        Sql = 
                            @"
                            select * from players where position in (7, 8, 9)
                            "
                    })
                };
            }
        }
    }
}
