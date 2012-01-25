using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<Index.Outputs>
    {
        public class Outputs
        {
            public PlayersResource.Data[] Data { get; set; }
        }

        public RunSqlAndReturn<PlayersResource.Data> FetchPlayersData { get; set; }

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

            Out = new Outputs {Data = FetchPlayersData.Models};
        }
    }
}