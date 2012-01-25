using System.Web.Mvc;
using MvcExample.Resources;
using MvcExample.Tasks.Players;
using Simpler.Web;

namespace MvcExample.Controllers
{
    public class PlayersController : ResourceController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Index(outputs => View(outputs));
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return Show(inputs => new {PlayerId = id},
                        outputs => View(outputs));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return Edit(inputs => new {PlayerId = id},
                        outputs => View(outputs));
        }

        [HttpPost]
        public ActionResult Update(PostData<Player.Data> model)
        {
            return !ModelState.IsValid

                       ? Edit(inputs => new {model.Data.PlayerId},
                              outputs => View(outputs))

                       : Update(inputs => new {model.Data},
                                outputs => RedirectToShow(new {id = model.Data.PlayerId}));
        }
    }
}
