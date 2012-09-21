using Nancy;

namespace Example.Nancy.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = _ => View["Views/Home/Index.html"];
        }
    }
}