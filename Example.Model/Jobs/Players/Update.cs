using Example.Model.Entities;
using Simpler;
using Simpler.Data.Jobs;

namespace Example.Model.Jobs.Players
{
    public class Update : InJob<Update.In>
    {
        public class In
        {
            public Player Player { get; set; }
        }

        public RunSql UpdatePlayer { get; set; }

        public override void Execute()
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
                .Set(new RunSql.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql,
                             Values = Input.Player
                         })
                .Execute();
        }
    }
}