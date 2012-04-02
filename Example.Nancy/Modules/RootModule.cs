using Nancy;

namespace Example.Nancy.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters => View["Views/Home/Index.html"];
        }
    }
}