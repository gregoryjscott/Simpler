using Saber.Models.Players;
using Simpler;

namespace Saber.Tasks.Players
{
    public class Edit : InOutTask<Edit.In, Edit.Out>
    {
        public class In
        {
            public int PlayerId { get; set; }
        }

        public class Out
        {
            public Player Player { get; set; }
        }

        public Invoke<FetchPlayer> FetchPlayer { get; set; }

        public override void Execute()
        {
            var player = FetchPlayer
                .Set(t => t.Input = new FetchPlayer.In {PlayerId = Input.PlayerId})
                .Get().Output.PlayerData;

            Output = new Out {Player = player};
        }
    }
}