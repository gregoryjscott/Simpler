using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class FetchPlayers : OutJob<FetchPlayers.Output>
    {
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

            Select._In = new ReturnMany<Player>.Input
                             {
                                 ConnectionName = Config.DatabaseName,
                                 Sql = sql
                             };
            Select.Run();
            var players = Select._Out.Models;

            _Out = new Output { Players = players };
        }
    }
}