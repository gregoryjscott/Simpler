using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Update : InTask<Update.Inputs>
    {
        public class Inputs
        {
            public PlayersResource.Data Data { get; set; }
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
            UpdatePlayer.Values = In.Data;
            UpdatePlayer.Execute();
        }
    }
}