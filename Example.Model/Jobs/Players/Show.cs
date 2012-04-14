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

        public override void Run()
        {
            var player = FetchPlayerData
                .Set(new FetchPlayer.In {PlayerId = _In.PlayerId})
                .Get().Player;

            _Out = new Out { Player = player };
        }
    }
}