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
            var model = Job.New<Index>()
                .Get();

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var model = Job.New<Show>()
                .Set(new Show.In {PlayerId = id})
                .Get();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = Job.New<Edit>()
                .Set(new Edit.In {PlayerId = id})
                .Get();

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Update.In model)
        {
            if (!ModelState.IsValid)
            {
                var editModel = Job.New<Edit>()
                    .Set(new Edit.In {PlayerId = model.Player.PlayerId.GetValueOrDefault()})
                    .Get();

                return View("Edit", editModel);
            }

            Job.New<Update>()
                .Set(new Update.In { Player = model.Player })
                .Run();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
