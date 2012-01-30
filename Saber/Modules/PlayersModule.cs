using Nancy;
using Saber.Tasks.Players;
using Simpler;
using Nancy.ModelBinding;

namespace Saber.Modules
{
    public class PlayersModule : NancyModule
    {
        public PlayersModule()
        {
            Get["/players"] =
                parameters => View["Views/Players/Index.html",
                                   Task.Create<Index>()
                                       .GetOutputs()];

            Get["/players/{PlayerId}"] =
                parameters => View["Views/Players/Show.html",
                                   Task.Create<Show>()
                                       .SetInputs(new {this.Bind<Show.Inputs>().PlayerId})
                                       .GetOutputs()];

            Get["/players/{PlayerId}/edit"] =
                parameters => View["Views/Players/Edit.html",
                                   Task.Create<Edit>()
                                       .SetInputs(new {this.Bind<Edit.Inputs>().PlayerId})
                                       .GetOutputs()];

            //Put["/players/{Player.PlayerId}"] =
            //    parameters =>
            //    {
            //        var inputs = this.Bind<Update.Inputs>();
            //        Task.Create<Update>()
            //            .SetInputs(new { inputs.Player })
            //            .Execute();
            //        return Response.AsRedirect(string.Format("/players/{0}", inputs.Player.PlayerId));
            //    };
        }
    }
}