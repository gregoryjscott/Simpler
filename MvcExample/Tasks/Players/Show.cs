using MvcExample.Resources;
using Simpler;

namespace MvcExample.Tasks.Players
{
    public class Show : InOutTask<Show.Inputs, Show.Outputs>
    {
        public class Inputs
        {
            public int PlayerId { get; set; }
        }

        public class Outputs
        {
            public PlayersResource.Data Data { get; set; }
        }

        public FetchPlayerDataById FetchPlayerData { get; set; }

        public override void Execute()
        {
            var player = FetchPlayerData
                .SetInputs(new { In.PlayerId })
                .GetOutputs()
                .PlayerData;

            Out = new Outputs { Data = player };
        }
    }
}