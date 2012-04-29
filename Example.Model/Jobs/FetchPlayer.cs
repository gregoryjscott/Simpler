using System.Linq;
using Example.Model.Entities;
using NUnit.Framework;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class FetchPlayer : InOutJob<FetchPlayer.Input, FetchPlayer.Output>
    {
        public class Input
        {
            public int PlayerId { get; set; }
        }

        public class Output
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

            _Out = new Output {Player = player};
        }

        public override void Test()
        {
            Config.SetDataDirectory();

            It<FetchPlayer>.Should(
                "return player identified by given id",
                job =>
                {
                    //var player = job
                    //    .Set(new In {PlayerId = 1})
                    //    .Get().Player;

                    job._In.PlayerId = 1;
                    job.Run();
                    var player = job._Out.Player;

                    Assert.True(player.PlayerId == 1);
                });
        }
    }
}