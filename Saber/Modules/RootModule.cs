using Nancy;

namespace Saber.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters => View["Views/Home/Index.html"];
        }
    }
}