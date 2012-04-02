using System.Linq;
using Saber.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace Saber.Tasks.Players
{
    public class FetchPlayer : InOutTask<FetchPlayer.In, FetchPlayer.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player PlayerData { get; set; }
        }

        public RunSqlAndReturn<Player> SelectPlayer { get; set; }

        public override void Execute()
        {
            SelectPlayer.ConnectionName = Config.DatabaseName;
            SelectPlayer.Sql =
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
            SelectPlayer.Values = Input;
            SelectPlayer.Execute();

            Output = new Out {PlayerData = SelectPlayer.Models.Single()};
        }
    }
}