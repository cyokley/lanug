using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LANUG.Models;

namespace LANUG.Controllers
{
    [Authorize]
    public class SponsorsController : Controller
    {
        private LANUGEntities db = new LANUGEntities();

        //
        // GET: /Sponsor/

        public ActionResult Index()
        {
            return View(db.Sponsors.ToList());
        }

        //
        // GET: /Sponsor/Details/5

        public ActionResult Details(int id = 0)
        {
            Sponsor sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        //
        // GET: /Sponsor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sponsor/Create

        [HttpPost]
        public ActionResult Create(Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    string logoPath = "~/Media/sponsor_" + Request.Files[0].FileName;
                    if (db.Sponsors.Any(s => s.LogoURL == logoPath)) throw new ApplicationException("An image with the specified name already exists. Please rename the logo and try again.");
                    Request.Files[0].SaveAs(Server.MapPath(logoPath));
                    sponsor.LogoURL = logoPath;
                }
                sponsor.CreatedById = sponsor.ModifiedById = new UsersContext().UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                db.Sponsors.Add(sponsor);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string errors = string.Empty;
                    foreach (var error in db.GetValidationErrors())
                    {
                        errors += error.ValidationErrors.First().ErrorMessage + "; ";
                    }
                    throw new ApplicationException(errors);
                }
                return RedirectToAction("Index");
                //db.Sponsors.Add(sponsor);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(sponsor);
        }

        //
        // GET: /Sponsor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sponsor sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        //
        // POST: /Sponsor/Edit/5

        [HttpPost]
        public ActionResult Edit(Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                var oldSponsor = db.Sponsors.FirstOrDefault(s => s.Id == sponsor.Id);
                oldSponsor.Name = sponsor.Name;
                oldSponsor.Website = sponsor.Website;
                oldSponsor.Description = sponsor.Description;
                if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    string logoPath = "~/Media/sponsor_" + Request.Files[0].FileName;
                    if (logoPath.ToLower() != oldSponsor.LogoURL.ToLower())
                    {
                        // if old logo name is different then new logo name make sure the name doesn't already exist
                        if (db.Sponsors.Any(s => s.LogoURL == logoPath)) throw new ApplicationException("An image with the specified name already exists. Please rename the logo and try again.");
                        // delete old logo
                        if (!string.IsNullOrWhiteSpace(oldSponsor.LogoURL)) new FileInfo(Server.MapPath(oldSponsor.LogoURL)).Delete();
                    }
                    Request.Files[0].SaveAs(Server.MapPath(logoPath));
                    oldSponsor.LogoURL = logoPath;
                }
                oldSponsor.Modified = DateTime.Now;
                oldSponsor.ModifiedById = new UsersContext().UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                db.Entry(oldSponsor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sponsor);
        }

        //
        // GET: /Sponsor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sponsor sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        //
        // POST: /Sponsor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sponsor sponsor = db.Sponsors.Find(id);
            db.Sponsors.Remove(sponsor);
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