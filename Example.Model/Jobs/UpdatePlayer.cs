using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class UpdatePlayer : InJob<UpdatePlayer.Input>
    {
        public class Input
        {
            public Player Player { get; set; }
        }

        public ReturnResult Update { get; set; }

        public override void Run()
        {
            const string sql = @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";

            Update.In.ConnectionName = Config.DatabaseName;
            Update.In.Sql = sql;
            Update.In.Values = In.Player;
            Update.Run();
        }
    }
}