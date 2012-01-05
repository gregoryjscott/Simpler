using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : Task<object, PlayerIndex>
    {
        public RunSqlAndReturn<PlayerIndexItem> FetchPlayers { get; set; }

        public override void Execute()
        {
            FetchPlayers.ConnectionName = "ExampleData";
            FetchPlayers.Sql =
                @"
                select 
                    Player.FirstName + ' ' + Player.LastName as Name,
                    Team.Mascot as Team
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                ";
            FetchPlayers.Execute();

            OutputsModel = new PlayerIndex
                           {
                               PlayerIndexItems = FetchPlayers.Models
                           };
        }
    }
}