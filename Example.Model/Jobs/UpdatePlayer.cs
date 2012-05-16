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

            Update._In.ConnectionName = Config.DatabaseName;
            Update._In.Sql = sql;
            Update._In.Values = _In.Player;
            Update.Run();
        }
    }
}