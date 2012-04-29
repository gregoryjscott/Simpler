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
                        var model = Job.New<FetchPlayers>().Get();

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                parameters =>
                    {
                        var model = Job.New<FetchPlayer>()
                            .Set(this.Bind<FetchPlayer.Input>())
                            .Get();

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                parameters =>
                    {
                        var model = Job.New<FetchPlayer>()
                            .Set(this.Bind<FetchPlayer.Input>())
                            .Get();

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                parameters =>
                    {
                        var input = this.Bind<UpdatePlayer.In>();
                        
                        Job.New<UpdatePlayer>().Set(input).Run();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}