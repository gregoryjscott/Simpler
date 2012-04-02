using Saber.Models.Players;
using Simpler;

namespace Saber.Tasks.Players
{
    public class Show : InOutTask<Show.In, Show.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player Player { get; set; }
        }

        public Invoke<FetchPlayer> FetchPlayerData { get; set; }

        public override void Execute()
        {
            var player = FetchPlayerData
                .Set(t => t.Input = new FetchPlayer.In {PlayerId = Input.PlayerId})
                .Get().Output.PlayerData;

            Output = new Out { Player = player };
        }
    }
}