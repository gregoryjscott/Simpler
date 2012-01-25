using MvcExample.Resources;
using Simpler;

namespace MvcExample.Tasks.Players
{
    public class Show : InOutTask<Show.Ins, Show.Outs>
    {
        public class Ins
        {
            public int PlayerId { get; set; }
        }

        public class Outs
        {
            public Player.Data Data { get; set; }
        }

        public FetchPlayerDataById FetchPlayerData { get; set; }

        public override void Execute()
        {
            var player = FetchPlayerData
                .SetInputs(new { In.PlayerId })
                .GetOutputs()
                .PlayerData;

            Out = new Outs { Data = player };
        }
    }
}