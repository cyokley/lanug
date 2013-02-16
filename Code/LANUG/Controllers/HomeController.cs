using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LANUG.Models;

namespace LANUG.Controllers
{
    public class HomeController : Controller
    {
        private LANUGEntities db = new LANUGEntities();

        public ActionResult Index()
        {
            return View(db.Events.Where(e => e.StartTime > DateTime.Now).OrderBy(e => e.StartTime).FirstOrDefault());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Sponsors()
        {
            LANUGEntities db = new LANUGEntities();
            return View(db.Sponsors.OrderBy(s => s.Name).ToList());
        }

        public ActionResult Calendar()
        {
            return View();
        }
    }
}
