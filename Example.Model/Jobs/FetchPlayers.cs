using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class FetchPlayers : OutJob<FetchPlayers.Output>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<FetchPlayers>.Should(
                "return all players",
                job =>
                {
                    job.Run();
                    var players = job.Out.Players;

                    Check.That(players.Length > 0, "Expected more than zero players to be returned.");
                });
        }

        public class Output
        {
            public Player[] Players { get; set; }
        }

        public ReturnMany<Player> Select { get; set; }

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

            Select.In.ConnectionName = Config.DatabaseName;
            Select.In.Sql = sql;
            Select.Run();
            var players = Select.Out.Models;

            Out.Players = players;
        }
    }
}