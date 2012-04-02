using System.Web.Mvc;
using MvcExample.Tasks.Players;
using Simpler;

namespace MvcExample.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = Task.Create<Index>()
                .Get().Output;

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var model = Task.Create<Show>()
                .Set(new Show.In {PlayerId = id})
                .Get().Output;

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = Task.Create<Edit>()
                .Set(new Edit.In {PlayerId = id})
                .Get().Output;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Update.In model)
        {
            if (!ModelState.IsValid)
            {
                var editModel = Task.Create<Edit>()
                    .Set(new Edit.In {PlayerId = model.Player.PlayerId.GetValueOrDefault()})
                    .Get().Output;

                return View("Edit", editModel);
            }

            Task.Create<Update>()
                .Set(new Update.In { Player = model.Player })
                .Execute();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
