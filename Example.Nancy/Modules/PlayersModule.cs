using Example.Model.Tasks;
using Nancy;
using Simpler;
using Nancy.ModelBinding;

namespace Example.Nancy.Modules
{
    public class PlayersModule : NancyModule
    {
        public PlayersModule()
        {
            Get["/players"] =
                _ =>
                    {
                        var fetch = Task.New<FetchPlayers>();
                        fetch.Run();
                        var model = fetch.Out;

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                _ =>
                    {
                        var fetch = Task.New<FetchPlayer>();
                        fetch.In = this.Bind<FetchPlayer.Input>();
                        var model = fetch.Out;

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                _ =>
                    {
                        var fetch = Task.New<FetchPlayer>();
                        fetch.In = this.Bind<FetchPlayer.Input>();
                        var model = fetch.Out;

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                _ =>
                    {
                        var input = this.Bind<UpdatePlayer.Input>();

                        var update = Task.New<UpdatePlayer>();
                        update.In = input;
                        update.Run();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}