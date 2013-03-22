using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LANUG.Models;
using System.Net.Mail;
using System.Configuration;

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
            Random rand = new Random();
            int v1 = rand.Next(1, 15);
            int v2 = rand.Next(1, 15);
            ViewBag.Captcha = string.Format("{0} + {1}", v1, v2);
            ViewBag.CV = v1 + v2 + 108;
            return View();
        }

        [HttpPost]
        public JsonResult Contact(string Name, string Email, string Subject, string Message, string Captcha, int CV)
        {
            string rval = string.Empty;
            try
            {
                int cval = 0;
                int.TryParse(Captcha, out cval);
                if (cval == CV - 108)
                {
                    MailMessage msg = new MailMessage(Email, ConfigurationManager.AppSettings["ContactEmail"]);
                    msg.Subject = "LANUG Contact Submission: " + Subject;
                    msg.IsBodyHtml = true;
                    msg.Body = "New Message From " + Name + "; Email: " + Email + "<br/><br/>" + Message;

                    SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]);
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"], ConfigurationManager.AppSettings["SmtpPassword"]);
                    smtp.Send(msg);
                }
                else rval = "The captcha answer is incorrect.";
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(rval, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sponsors()
        {
            LANUGEntities db = new LANUGEntities();
            return View(db.Sponsors.OrderBy(s => s.Name).ToList());
        }

        public ActionResult Calendar()
        {
            return View(db.Events);
        }
    }
}
