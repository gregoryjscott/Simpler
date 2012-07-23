using Example.Model.Entities;
using Simpler;
using Simpler.Data;

namespace Example.Model.Tasks
{
    public class FetchPlayers : OutTask<FetchPlayers.Output>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<FetchPlayers>.Should(
                "return all players",
                it =>
                {
                    it.Run();
                    var players = it.Out.Players;

                    Check.That(players.Length > 0, "Expected more than zero players to be returned.");
                });
        }

        public class Output
        {
            public Player[] Players { get; set; }
        }

        public override void Run()
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