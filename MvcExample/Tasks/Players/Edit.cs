using System.Linq;
using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;
using Simpler.Web.Models;

namespace MvcExample.Tasks.Players
{
    public class Edit : InOutTask<PlayerKey, EditResult<PlayerEdit>>
    {
        public RunSqlAndReturn<PlayerEdit> FetchPlayer { get; set; }

        public override void Execute()
        {
            FetchPlayer.ConnectionName = Config.Database;
            FetchPlayer.Sql =
                @"
                select
                    PlayerId,
                    Player.FirstName,
                    Player.LastName,
                    Player.TeamId
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                where
                    PlayerId = @PlayerId
                ";
            FetchPlayer.Values = Inputs;
            FetchPlayer.Execute();

            Outputs = new EditResult<PlayerEdit>
                      {
                          Model = FetchPlayer.Models.Single()
                      };
        }
    }
}