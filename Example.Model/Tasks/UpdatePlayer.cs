using Example.Model.Entities;
using Simpler;
using Simpler.Data;

namespace Example.Model.Tasks
{
    public class UpdatePlayer : InTask<UpdatePlayer.Input>
    {
        public class Input
        {
            public Player Player { get; set; }
        }

        public override void Execute()
        {
            const string sql = @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                Db.GetResult(connection, sql, In.Player);
            }
        }
    }
}