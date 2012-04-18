using System.Web.Mvc;
using Example.Model.Jobs.Players;
using Simpler;

namespace Example.Mvc.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = Job.New<FetchPlayers>()
                .Get();

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var model = Job.New<FetchPlayer>()
                .Set(new FetchPlayer.In {PlayerId = id})
                .Get();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = Job.New<FetchPlayer>()
                .Set(new FetchPlayer.In {PlayerId = id})
                .Get();

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(UpdatePlayer.In model)
        {
            if (!ModelState.IsValid)
            {
                var editModel = Job.New<FetchPlayer>()
                    .Set(new FetchPlayer.In {PlayerId = model.Player.PlayerId.GetValueOrDefault()})
                    .Get();

                return View("Edit", editModel);
            }

            Job.New<UpdatePlayer>()
                .Set(new UpdatePlayer.In { Player = model.Player })
                .Run();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
