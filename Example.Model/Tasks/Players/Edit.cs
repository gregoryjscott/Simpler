using Example.Model.Entities;
using Simpler;

namespace Example.Model.Tasks.Players
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

        public FetchPlayer FetchPlayer { get; set; }

        public override void Execute()
        {
            var player = FetchPlayer
                .Set(new FetchPlayer.In {PlayerId = Input.PlayerId})
                .Get().Player;

            Output = new Out {Player = player};
        }
    }
}