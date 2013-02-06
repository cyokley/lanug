using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LANUG.Models;

namespace LANUG.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private LANUGEntities db = new LANUGEntities();

        //
        // GET: /Meetings/

        public ActionResult Index()
        {
            return View(db.Meetings.ToList());
        }

        [AllowAnonymous]
        public JsonResult ListMeetings()
        {
            return Json(db.Meetings.OrderBy(m => m.StartTime));
        }

        [AllowAnonymous]
        public ActionResult GetItem(string name)
        {
            return View(db.Meetings.Where(m => m.Name == name).Include(m => m.Sponsors).FirstOrDefault());
        }

        //
        // GET: /Meetings/Details/5

        public ActionResult Details(int id = 0)
        {
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        //
        // GET: /Meetings/Create

        public ActionResult Create()
        {
            ViewBag.MeetingSponsors = new SelectList(db.Sponsors, "Id", "Name");
            return View();
        }

        //
        // POST: /Meetings/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Meeting meeting, int[] SponsorList)
        {
            if (ModelState.IsValid)
            {
                meeting.CreatedById = meeting.ModifiedById = new UsersContext().UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                if (SponsorList != null)
                {
                    foreach (int sponsorId in SponsorList)
                    {
                        meeting.Sponsors.Add(db.Sponsors.Find(sponsorId));
                    }
                }
                db.Meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MeetingSponsors = new SelectList(db.Sponsors, "Id", "Name");
            //ViewBag.SponsorId = new SelectList(db.Sponsors, "Id", "Name", meeting.SponsorId);
            return View(meeting);
        }

        //
        // GET: /Meetings/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Meeting meeting = db.Meetings.Include("Sponsors").FirstOrDefault(m => m.Id == id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeetingSponsors = new SelectList(db.Sponsors, "Id", "Name");
            return View(meeting);
        }

        //
        // POST: /Meetings/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Meeting meeting, int[] SponsorList)
        {
            if (ModelState.IsValid)
            {
                var mtg = db.Meetings.Find(meeting.Id);
                mtg.Name = meeting.Name;
                mtg.StartTime = meeting.StartTime;
                mtg.EndTime = meeting.EndTime;
                mtg.Info = meeting.Info;
                mtg.Summary = meeting.Summary;
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
            return View(meeting);
        }

        //
        // GET: /Meetings/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        //
        // POST: /Meetings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            db.Meetings.Remove(meeting);
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