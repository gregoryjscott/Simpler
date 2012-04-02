using Saber.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace Saber.Tasks.Players
{
    public class Update : InTask<Update.In>
    {
        public class In
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
            UpdatePlayer.Values = Input.Player;
            UpdatePlayer.Execute();
        }
    }
}