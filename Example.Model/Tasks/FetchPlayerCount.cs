using Simpler;
using Simpler.Data;

namespace Example.Model.Tasks
{
    public class FetchPlayerCount : OutTask<int>
    {
        public override void Specs()
        {
            Config.SetDataDirectory();

            It<FetchPlayerCount>.Should(
                "return player count",
                it =>
                {
                    it.Execute();
                    var count = it.Out;

                    Check.That(count > 0,
                               "Expected player count to be greater than zero.");
                });
        }

        public class Output
        {
            public int Count { get; set; }
        }

        public override void Execute()
        {
            const string sql = @"
                select
                    count(1) as Count
                from 
                    Player
                ";

            using(var connection = Db.Connect(Config.DatabaseName))
            {
                var output = Db.GetOne<Output>(connection, sql);
                Out = output.Count;
            }
        }
    }
}