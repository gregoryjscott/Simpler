using Example.Model.Entities;
using Simpler;

namespace Example.Model.Jobs.Players
{
    public class Show : InOutJob<Show.In, Show.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player Player { get; set; }
        }

        public FetchPlayer FetchPlayerData { get; set; }

        public override void Execute()
        {
            var player = FetchPlayerData
                .Set(new FetchPlayer.In {PlayerId = Input.PlayerId})
                .Get().Player;

            Output = new Out { Player = player };
        }
    }
}