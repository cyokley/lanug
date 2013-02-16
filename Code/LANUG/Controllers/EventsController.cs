using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LANUG.Models;

namespace LANUG.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private LANUGEntities db = new LANUGEntities();

        //
        // GET: /Events/

        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        [AllowAnonymous]
        public JsonResult ListEvents()
        {
            return Json(db.Events.OrderBy(m => m.StartTime));
        }

        [AllowAnonymous]
        public ActionResult GetItem(string name)
        {
            return View(db.Events.Where(m => SqlFunctions.PatIndex(name.Replace("-", "_"), m.Name) > 0).Include(m => m.Sponsors).FirstOrDefault());
        }

        //
        // GET: /Events/Details/5

        public ActionResult Details(int id = 0)
        {
            Event Event = db.Events.Find(id);
            if (Event == null)
            {
                return HttpNotFound();
            }
            return View(Event);
        }

        //
        // GET: /Events/Create

        public ActionResult Create()
        {
            ViewBag.EventSponsors = new SelectList(db.Sponsors, "Id", "Name");
            return View();
        }

        //
        // POST: /Events/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Event Event, int[] SponsorList)
        {
            if (ModelState.IsValid)
            {
                Event.CreatedById = Event.ModifiedById = new UsersContext().UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                if (SponsorList != null)
                {
                    foreach (int sponsorId in SponsorList)
                    {
                        Event.Sponsors.Add(db.Sponsors.Find(sponsorId));
                    }
                }
                db.Events.Add(Event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventSponsors = new SelectList(db.Sponsors, "Id", "Name");
            //ViewBag.SponsorId = new SelectList(db.Sponsors, "Id", "Name", Event.SponsorId);
            return View(Event);
        }

        //
        // GET: /Events/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Event Event = db.Events.Include("Sponsors").FirstOrDefault(m => m.Id == id);
            if (Event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventSponsors = new SelectList(db.Sponsors, "Id", "Name");
            return View(Event);
        }

        //
        // POST: /Events/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Event Event, int[] SponsorList)
        {
            if (ModelState.IsValid)
            {
                var mtg = db.Events.Find(Event.Id);
                mtg.Name = Event.Name;
                mtg.StartTime = Event.StartTime;
                mtg.EndTime = Event.EndTime;
                mtg.Info = Event.Info;
                mtg.Summary = Event.Summary;
                mtg.Sponsors.Clear();
                if (SponsorList != null)
                {
                    foreach (int sponsorId in SponsorList)
                    {
                        mtg.Sponsors.Add(db.Sponsors.Find(sponsorId));
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Event);
        }

        //
        // GET: /Events/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Event Event = db.Events.Find(id);
            if (Event == null)
            {
                return HttpNotFound();
            }
            return View(Event);
        }

        //
        // POST: /Events/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event Event = db.Events.Find(id);
            db.Events.Remove(Event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}