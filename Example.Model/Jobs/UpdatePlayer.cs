using Example.Model.Entities;
using Simpler;

namespace Example.Model.Jobs
{
    public class UpdatePlayer : InJob<UpdatePlayer.Input>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<UpdatePlayer>.Should(
                "update player",
                it =>
                {
                    var player =
                        new Player
                        {
                            PlayerId = 1,
                            FirstName = "Something",
                            LastName = "Different",
                            TeamId = 2
                        };

                    it.In.Player = player;
                    it.Run();

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

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                Db.GetResult(connection, sql, In.Player);
            }
        }
    }
}