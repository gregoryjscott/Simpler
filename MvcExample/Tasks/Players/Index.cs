using System.Data.SqlServerCe;
using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : Task<object, PlayerIndex>
    {
        public FetchListOf<PlayerIndexItem> FetchPlayers { get; set; }

        public override void Execute()
        {
            //var providerName = ConfigurationManager.ConnectionStrings["ExampleData"].ProviderName;
            //var provider = DbProviderFactories.GetFactory(providerName);

            using (var connection = new SqlCeConnection("Data Source=|DataDirectory|ExampleData.sdf"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText =
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

                FetchPlayers.SelectCommand = command;
                FetchPlayers.Execute();
                OutputsModel = new PlayerIndex
                               {
                                   PlayerIndexItems = FetchPlayers.ObjectsFetched
                               };
            }
        }
    }
}