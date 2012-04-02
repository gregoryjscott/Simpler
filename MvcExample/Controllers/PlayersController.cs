using System.Web.Mvc;
using MvcExample.Tasks.Players;
using Simpler;
using Simpler.Web;
using Simpler.Web.Tasks;

namespace MvcExample.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = Invoke<Index>.New()
                .Get().Output;

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var model = Invoke<Show>.New()
                .Set(t => t.Input = new Show.In {PlayerId = id})
                .Get().Output;

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = Invoke<Edit>.New()
                .Set(t => t.Input = new Edit.In {PlayerId = id})
                .Get().Output;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Update.In model)
        {
            if (!ModelState.IsValid)
            {
                var editModel = Invoke<Edit>.New()
                    .Set(t => t.Input = new Edit.In {PlayerId = model.Player.PlayerId.GetValueOrDefault()})
                    .Get().Output;

                return View("Edit", editModel);
            }

            Invoke<Update>.New()
                .Set(t => t.Input = new Update.In { Player = model.Player })
                .Execute();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
