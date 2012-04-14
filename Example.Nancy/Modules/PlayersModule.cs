using Example.Model.Jobs.Players;
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
                        var model = Job.Create<Index>()
                            .Get();

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                parameters =>
                    {
                        var model = Job.Create<Show>()
                            .Set(this.Bind<Show.In>())
                            .Get();

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                parameters =>
                    {
                        var model = Job.Create<Edit>()
                            .Set(this.Bind<Edit.In>())
                            .Get();

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                parameters =>
                    {
                        var input = this.Bind<Update.In>();
                        
                        Job.Create<Update>()
                            .Set(input)
                            .Execute();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}