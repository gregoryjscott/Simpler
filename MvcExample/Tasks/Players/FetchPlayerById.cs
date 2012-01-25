using System.Linq;
using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class FetchPlayerById : InOutTask<FetchPlayerById.Ins, FetchPlayerById.Outs>
    {
        public class Ins
        {
            public int PlayerId { get; set; }
        }

        public class Outs
        {
            public Player Player { get; set; }
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

            Out = new Outs {Player = FetchPlayer.Models.Single()};
        }
    }
}