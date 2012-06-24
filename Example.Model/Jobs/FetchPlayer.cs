using System;
using Example.Model.Entities;
using Simpler;
using Simpler.Data.Jobs;

namespace Example.Model.Jobs
{
    public class FetchPlayer : InOutJob<FetchPlayer.Input, FetchPlayer.Output>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<FetchPlayer>.Should(
                "return player identified by given id",
                job =>
                {
                    job.In.PlayerId = 1;
                    job.Run();
                    var player = job.Out.Player;

                    Check.That(player.PlayerId == 1,
                               String.Format("Expect {0} to be equal to 1.", player.PlayerId));
                });
        }

        public class Input
        {
            public int PlayerId { get; set; }
        }

        public class Output
        {
            public Player Player { get; set; }
        }

        public ReturnOne<Player> Select { get; set; }

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

            Select.In.ConnectionName = Config.DatabaseName;
            Select.In.Sql = sql;
            Select.In.Values = In;
            Select.Run();
            var player = Select.Out.Model;

            Out.Player = player;
        }
    }
}