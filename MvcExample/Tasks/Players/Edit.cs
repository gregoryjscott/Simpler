using System.Linq;
using MvcExample.Resources;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Edit : InOutTask<Edit.Ins, Edit.Outs>
    {
        public class Ins
        {
            public int PlayerId { get; set; }
        }

        public class Outs
        {
            public Player Player { get; set; }
        }

        public FetchPlayerById FetchPlayer { get; set; }

        public override void Execute()
        {
            var player = FetchPlayer
                .SetInputs(new {In.PlayerId})
                .GetOutputs()
                .Player;

            Out = new Outs {Player = player};
        }
    }
}