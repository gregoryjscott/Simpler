using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<Index.Outs>
    {
        public class Outs
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

            Out = new Outs {Players = FetchPlayers.Models};
        }
    }
}