using System.Linq;
using Example.Model.Entities;
using NUnit.Framework;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class FetchPlayer : InOutJob<FetchPlayer.In, FetchPlayer.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player Player { get; set; }
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
                where
                    PlayerId = @PlayerId
                ";

            var player = Select
                .Set(new ReturnMany<Player>.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql,
                             Values = _In
                         })
                .Get().Models.Single();

            _Out = new Out {Player = player};
        }

        public override void Test()
        {
            Config.SetDataDirectory();

            Verify.Job<FetchPlayer>.Can(
                "return player identified by given id",
                job =>
                {
                    var player = job
                        .Set(new In {PlayerId = 1})
                        .Get().Player;

                    Assert.True(player.PlayerId == 1);
                });
        }
    }
}