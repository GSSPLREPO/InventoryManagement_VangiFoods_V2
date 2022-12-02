using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SalesOrder()
        {
            return View();
        }
    }
}