using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class PostProductionRejectionNoteController : Controller
    {
        // GET: PostProductionRejectionNote
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPostProductionRejectionNote()
        {
            return View();
        }
    }
}