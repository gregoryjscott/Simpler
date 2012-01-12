﻿using System.Web.Mvc;
using Simpler.Web;

namespace MvcExample.Controllers
{
    public class PlayersController : ResourceController
    {
        public ActionResult Index()
        {
            return Index(inputs => null, outputs => View(outputs.Model));
        }

        public ActionResult Show(int id)
        {
            return Show(inputs => new {PlayerId = id}, outputs => View(outputs.Model));
        }
    }
}
