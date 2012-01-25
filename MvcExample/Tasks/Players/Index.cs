using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<Index.Outs>
    {
        public class Outs
        {
            public Player.Data[] Data { get; set; }
        }

        public RunSqlAndReturn<Player.Data> FetchPlayersData { get; set; }

        public override void Execute()
        {
            FetchPlayersData.ConnectionName = Config.DatabaseName;
            FetchPlayersData.Sql =
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
            FetchPlayersData.Execute();

            Out = new Outs {Data = FetchPlayersData.Models};
        }
    }
}