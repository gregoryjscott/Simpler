using System.Web.Mvc;
using Example.Model.Jobs;
using Simpler;

namespace Example.Mvc.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var fetch = Job.New<FetchPlayers>();
            fetch.Run();
            var model = fetch._Out;

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var fetch = Job.New<FetchPlayer>();
            fetch._In.PlayerId = id;
            fetch.Run();
            var model = fetch._Out.Player;

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fetch = Job.New<FetchPlayer>();
            fetch._In.PlayerId = id;
            fetch.Run();
            var model = fetch._Out.Player;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(UpdatePlayer.Input model)
        {
            if (!ModelState.IsValid)
            {
                var fetch = Job.New<FetchPlayer>();
                fetch._In.PlayerId = model.Player.PlayerId.GetValueOrDefault();
                fetch.Run();
                var editModel = fetch._Out;

                return View("Edit", editModel);
            }

            var update = Job.New<UpdatePlayer>();
            update._In.Player = model.Player;
            update.Run();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
