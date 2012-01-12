using System.Linq;
using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;
using Simpler.Web.Models;

namespace MvcExample.Tasks.Players
{
    public class Show : InOutTask<Show.In, ShowResult<PlayerShow>>
    {
        public class In
        {
            public int PlayerId;
        }

        public RunSqlAndReturn<PlayerShow> FetchPlayers { get; set; }

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

            Outputs = new ShowResult<PlayerShow>
                      {
                          Model = FetchPlayers.Models.Single()
                      };
        }
    }
}