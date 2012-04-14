using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs.Players
{
    public class Update : InJob<Update.In>
    {
        public class In
        {
            public Player Player { get; set; }
        }

        public ReturnResult UpdatePlayer { get; set; }

        public override void Run()
        {
            const string sql =
                @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";

            UpdatePlayer
                .Set(new ReturnResult.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql,
                             Values = _In.Player
                         })
                .Run();
        }
    }
}