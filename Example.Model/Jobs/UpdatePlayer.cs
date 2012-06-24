using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class UpdatePlayer : InJob<UpdatePlayer.Input>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<UpdatePlayer>.Should(
                "update player",
                job =>
                {
                    var player =
                        new Player
                        {
                            PlayerId = 1,
                            FirstName = "Something",
                            LastName = "Different",
                            TeamId = 2
                        };

                    job.In.Player = player;
                    job.Run();

                    var fetch = New<FetchPlayer>();
                    fetch.In.PlayerId = player.PlayerId.GetValueOrDefault();
                    fetch.Run();
                    var updatedPlayer = fetch.Out.Player;

                    Check.That(updatedPlayer.LastName == "Different",
                        "Expected LastName to be Different.");
                });
        }

        public class Input
        {
            public Player Player { get; set; }
        }

        public ReturnResult Update { get; set; }

        public override void Run()
        {
            const string sql = @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";

            Update.In.ConnectionName = Config.DatabaseName;
            Update.In.Sql = sql;
            Update.In.Values = In.Player;
            Update.Run();
        }
    }
}