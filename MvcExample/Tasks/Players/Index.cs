using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;
using Simpler.Web.Models;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<IndexResult<PlayerIndex>>
    {
        public RunSqlAndReturn<PlayerIndexItem> FetchPlayers { get; set; }

        public override void Execute()
        {
            FetchPlayers.ConnectionName = Config.Database;
            FetchPlayers.Sql =
                @"
                select 
                    PlayerId,
                    Player.FirstName + ' ' + Player.LastName as Name,
                    Team.Mascot as Team
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                ";
            FetchPlayers.Execute();

            Outputs = new IndexResult<PlayerIndex>
                      {
                          Model = new PlayerIndex
                                  {
                                      PlayerIndexItems = FetchPlayers.Models
                                  }
                      };
        }
    }
}