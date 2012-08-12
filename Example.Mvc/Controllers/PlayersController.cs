using System.Web.Mvc;
using Example.Model.Tasks;
using Simpler;

namespace Example.Mvc.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var fetch = Task.New<FetchPlayers>();
            fetch.Execute();
            var model = fetch.Out;

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var fetch = Task.New<FetchPlayer>();
            fetch.In.PlayerId = id;
            fetch.Execute();
            var model = fetch.Out.Player;

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fetch = Task.New<FetchPlayer>();
            fetch.In.PlayerId = id;
            fetch.Execute();
            var model = fetch.Out.Player;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(UpdatePlayer.Input model)
        {
            if (!ModelState.IsValid)
            {
                var fetch = Task.New<FetchPlayer>();
                fetch.In.PlayerId = model.Player.PlayerId.GetValueOrDefault();
                fetch.Execute();
                var editModel = fetch.Out;

                return View("Edit", editModel);
            }

            var update = Task.New<UpdatePlayer>();
            update.In.Player = model.Player;
            update.Execute();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
