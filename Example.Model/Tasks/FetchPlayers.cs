using Example.Model.Entities;
using Simpler;
using Simpler.Data;

namespace Example.Model.Tasks
{
    public class FetchPlayers : OutTask<FetchPlayers.Output>
    {
        public class Output
        {
            public Player[] Players { get; set; }
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
                ";

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                Out.Players = Db.GetMany<Player>(connection, sql);
            }
        }
    }
}