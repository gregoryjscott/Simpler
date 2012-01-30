using Saber.Models.Players;
using Simpler;

namespace Saber.Tasks.Players
{
    public class Show : InOutTask<Show.Inputs, Show.Outputs>
    {
        public class Inputs
        {
            public int PlayerId { get; set; }
        }

        public class Outputs
        {
            public Player Player { get; set; }
        }

        public FetchPlayerDataById FetchPlayerData { get; set; }

        public override void Execute()
        {
            var player = FetchPlayerData
                .SetInputs(new { In.PlayerId })
                .GetOutputs()
                .PlayerData;

            Out = new Outputs { Player = player };
        }
    }
}