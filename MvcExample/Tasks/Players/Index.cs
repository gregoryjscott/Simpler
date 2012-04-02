using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<Index.Out>
    {
        public class Out
        {
            public Player[] Players { get; set; }
        }

        public RunSqlAndReturn<Player> FetchPlayers { get; set; }

        public override void Execute()
        {
            FetchPlayers.ConnectionName = Config.DatabaseName;
            FetchPlayers.Sql =
                @"
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
            FetchPlayers.Execute();

            Output = new Out {Players = FetchPlayers.Models};
        }
    }
}