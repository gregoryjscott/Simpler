using System.Linq;
using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;
using Simpler.Web.Models;

namespace MvcExample.Tasks.Players
{
    public class Update : InOutTask<PlayerEdit, UpdateResult>
    {
        public RunSql UpdatePlayer { get; set; }

        public override void Execute()
        {
            UpdatePlayer.ConnectionName = Config.Database;
            UpdatePlayer.Sql =
                @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";
            UpdatePlayer.Values = Inputs;
            UpdatePlayer.Execute();

            Outputs = new UpdateResult
                      {
                          RowsAffected = UpdatePlayer.RowsAffected
                      };
        }
    }
}