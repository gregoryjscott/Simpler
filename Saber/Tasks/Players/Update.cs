using Saber.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace Saber.Tasks.Players
{
    public class Update : InTask<Update.Inputs>
    {
        public class Inputs
        {
            public string _method { get; set; }
            public Player Player { get; set; }
        }

        public RunSql UpdatePlayer { get; set; }

        public override void Execute()
        {
            UpdatePlayer.ConnectionName = Config.DatabaseName;
            UpdatePlayer.Sql =
                @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";
            UpdatePlayer.Values = In.Player;
            UpdatePlayer.Execute();
        }
    }
}