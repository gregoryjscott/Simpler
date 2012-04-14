using Example.Model.Entities;
using Simpler;

namespace Example.Model.Jobs.Players
{
    public class Edit : InOutJob<Edit.In, Edit.Out>
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

        public override void Run()
        {
            var player = FetchPlayer
                .Set(new FetchPlayer.In {PlayerId = _In.PlayerId})
                .Get().Player;

            _Out = new Out {Player = player};
        }
    }
}