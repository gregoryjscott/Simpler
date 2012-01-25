using System.Linq;
using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class FetchPlayerDataById : InOutTask<FetchPlayerDataById.Inputs, FetchPlayerDataById.Outputs>
    {
        public class Inputs
        {
            public int PlayerId { get; set; }
        }

        public class Outputs
        {
            public PlayersResource.Data PlayerData { get; set; }
        }

        public RunSqlAndReturn<PlayersResource.Data> FetchPlayerData { get; set; }

        public override void Execute()
        {
            FetchPlayerData.ConnectionName = Config.DatabaseName;
            FetchPlayerData.Sql =
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
                where
                    PlayerId = @PlayerId
                ";
            FetchPlayerData.Values = In;
            FetchPlayerData.Execute();

            Out = new Outputs {PlayerData = FetchPlayerData.Models.Single()};
        }
    }
}