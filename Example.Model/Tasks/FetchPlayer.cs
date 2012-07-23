using Example.Model.Entities;
using Simpler;
using Simpler.Data;

namespace Example.Model.Jobs
{
    public class FetchPlayer : InOutTask<FetchPlayer.Input, FetchPlayer.Output>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<FetchPlayer>.Should(
                "return player identified by given id",
                it =>
                {
                    it.In.PlayerId = 1;
                    it.Run();
                    var player = it.Out.Player;

                    Check.That(player.PlayerId == 1, "Expect {0} to be equal to 1.", player.PlayerId);
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

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                Out.Player = Db.GetOne<Player>(connection, sql, In);
            }
        }
    }
}