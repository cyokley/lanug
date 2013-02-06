using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LANUG.Filters;
using LANUG.Models;
using WebMatrix.WebData;

namespace LANUG.Controllers
{
    [Authorize(Roles = "Admin")]
    [InitializeSimpleMembership]
    public class UsersController : Controller
    {
        UsersContext db = new UsersContext();

        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View(db.UserProfiles.OrderBy(u => u.Disabled).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserProfile model, string Password, string IsAdmin)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.UserName, Password, new { Email = model.Email });
                if (!string.IsNullOrEmpty(IsAdmin)) Roles.AddUserToRole(model.UserName, "Admin");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            UserProfile user = db.UserProfiles.Find(id);
            ViewBag.IsAdmin = Roles.IsUserInRole(user.UserName, "Admin");
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(UserProfile model, string IsAdmin)
        {
            if (ModelState.IsValid)
            {
                var u = db.UserProfiles.Find(model.UserId);
                u.UserName = model.UserName;
                u.Email = model.Email;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(IsAdmin)) Roles.AddUserToRole(u.UserName, "Admin");
                else Roles.RemoveUserFromRole(u.UserName, "Admin");

                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Disable(int id = 0)
        {
            UserProfile user = db.UserProfiles.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Disable")]
        public ActionResult DisableConfirmed(int id)
        {
            var u = db.UserProfiles.Find(id);
            u.Disabled = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
