using Example.Model.Jobs;
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
                parameters =>
                    {
                        var fetch = Job.New<FetchPlayers>();
                        fetch.Run();
                        var model = fetch.Out;

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                parameters =>
                    {
                        var fetch = Job.New<FetchPlayer>();
                        fetch.In = this.Bind<FetchPlayer.Input>();
                        var model = fetch.Out;

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                parameters =>
                    {
                        var fetch = Job.New<FetchPlayer>();
                        fetch.In = this.Bind<FetchPlayer.Input>();
                        var model = fetch.Out;

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                parameters =>
                    {
                        var input = this.Bind<UpdatePlayer.Input>();
                        
                        var update = Job.New<UpdatePlayer>();
                        update.In = input;
                        update.Run();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}