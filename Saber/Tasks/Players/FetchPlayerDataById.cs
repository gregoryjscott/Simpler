using System.Linq;
using Saber.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace Saber.Tasks.Players
{
    public class FetchPlayerDataById : InOutTask<FetchPlayerDataById.Inputs, FetchPlayerDataById.Outputs>
    {
        public class Inputs
        {
            public int PlayerId { get; set; }
        }

        public class Outputs
        {
            public Player PlayerData { get; set; }
        }

        public RunSqlAndReturn<Player> FetchPlayer { get; set; }

        public override void Execute()
        {
            FetchPlayer.ConnectionName = Config.DatabaseName;
            FetchPlayer.Sql =
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
            FetchPlayer.Values = In;
            FetchPlayer.Execute();

            Out = new Outputs {PlayerData = FetchPlayer.Models.Single()};
        }
    }
}