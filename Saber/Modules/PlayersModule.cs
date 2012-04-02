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
                parameters =>
                    {
                        var model = Invoke<Index>.New()
                            .Get().Output;

                        return View["Views/Players/Index.html", model];
                    };

            Get["/players/{PlayerId}"] =
                parameters =>
                    {
                        var model = Invoke<Show>.New()
                            .Set(t => t.Input = this.Bind<Show.In>())
                            .Get().Output;

                        return View["Views/Players/Show.html", model];
                    };

            Get["/players/{PlayerId}/edit"] =
                parameters =>
                    {
                        var model = Invoke<Edit>.New()
                            .Set(t => t.Input = this.Bind<Edit.In>())
                            .Get().Output;

                        return View["Views/Players/Edit.html", model];
                    };

            Put["/players/{PlayerId}"] =
                parameters =>
                    {
                        var input = this.Bind<Update.In>();
                        
                        Invoke<Update>.New()
                            .Set(t => t.Input = input)
                            .Execute();

                        return Response.AsRedirect(string.Format("/players/{0}", input.Player.PlayerId));
                    };
        }
    }
}