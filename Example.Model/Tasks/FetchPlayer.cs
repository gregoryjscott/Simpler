using Example.Model.Entities;
using Simpler;
using Simpler.Data;

namespace Example.Model.Tasks
{
    public class FetchPlayer : InOutTask<FetchPlayer.Input, FetchPlayer.Output>
    {
        public class Input
        {
            public int PlayerId { get; set; }
        }

        public class Output
        {
            public Player Player { get; set; }
        }

        public override void Execute()
        {
            const string sql = @"
                select
                    PlayerId,
                    Player.FirstName,
                    Player.LastName,
                    Player.TeamId,
                    Player.FirstName + ' ' + Player.LastName as FullName,
                    Team.Mascot as Team
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                where
                    PlayerId = @PlayerId
                ";

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                Out.Player = Db.GetOne<Player>(connection, sql, In);
            }
        }
    }
}