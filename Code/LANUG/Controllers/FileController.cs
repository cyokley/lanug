using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LANUG.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        [HttpPost]
        public string UploadFile()
        {
            if (Request.Files.Count > 0)
            {
                if (System.IO.File.Exists("~/Media/" + Request.Files[0].FileName)) throw new ApplicationException("A file with that name already exists. Please rename the file and try again.");
                string url = "~/Media/" + Request.Files[0].FileName;
                Request.Files[0].SaveAs(Server.MapPath(url));
                return string.Format("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}');</script>", Request["CKEditorFuncNum"], Url.Content(url), string.Empty);
            }
            return "Error: No file selected.";
        }
    }
}
