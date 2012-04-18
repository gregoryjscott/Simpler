using Example.Model.Entities;
using Simpler;
using Simpler.Sql.Jobs;

namespace Example.Model.Jobs
{
    public class UpdatePlayer : InJob<UpdatePlayer.In>
    {
        public class In
        {
            public Player Player { get; set; }
        }

        public ReturnResult Update { get; set; }

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

            Update
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