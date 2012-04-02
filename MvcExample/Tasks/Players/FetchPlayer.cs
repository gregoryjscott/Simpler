using System.Linq;
using MvcExample.Entities;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class FetchPlayer : InOutTask<FetchPlayer.In, FetchPlayer.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player Player { get; set; }
        }

        public RunSqlAndReturn<Player> SelectPlayer { get; set; }

        public override void Execute()
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
                where
                    PlayerId = @PlayerId
                ";

            var player = SelectPlayer
                .Set(new RunSqlAndReturn<Player>.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql,
                             Values = Input
                         })
                .Get().Models.Single();

            Output = new Out {Player = player};
        }
    }
}