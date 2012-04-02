using Example.Model.Tasks.Players;
using Nancy;
using Simpler;
using Nancy.ModelBinding;

namespace Saber.Modules
{
    public class PlayersModule : NancyModule
    {
        public PlayersModule()
        {
            Get["/players"] =
                parameters =>
                    {
                        var model = Task.Create<Index>()
                            .Get();

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                parameters =>
                    {
                        var model = Task.Create<Show>()
                            .Set(this.Bind<Show.In>())
                            .Get();

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                parameters =>
                    {
                        var model = Task.Create<Edit>()
                            .Set(this.Bind<Edit.In>())
                            .Get();

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                parameters =>
                    {
                        var input = this.Bind<Update.In>();
                        
                        Task.Create<Update>()
                            .Set(input)
                            .Execute();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}