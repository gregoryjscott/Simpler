using MvcExample.Models.Players;
using Simpler;

namespace MvcExample.Tasks.Players
{
    public class Edit : InOutTask<Edit.Inputs, Edit.Outputs>
    {
        public class Inputs
        {
            public int PlayerId { get; set; }
        }

        public class Outputs
        {
            public Player Player { get; set; }
        }

        public FetchPlayerDataById FetchPlayer { get; set; }

        public override void Execute()
        {
            var player = FetchPlayer
                .SetInputs(new {In.PlayerId})
                .GetOutputs()
                .PlayerData;

            Out = new Outputs {Player = player};
        }
    }
}