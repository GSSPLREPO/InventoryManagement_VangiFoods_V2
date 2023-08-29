using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using Newtonsoft.Json;

namespace InVanWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private IDashboardRepository _repository;

        #region Initializing constructor
        /// <summary>
        /// Snehal: Constructor without parameter
        /// </summary>
        public DashboardController()
        {
            _repository = new DashboardRepository();
        }
        #endregion

        #region Old dashboard
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
        #endregion

        #region Bind Utility Consumption V/S Production

        public ActionResult UtilityConsumptionProduction()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            ByWorkOrderNumberDropDown();
            return View();
        }

        public JsonResult GetDashboardUtilityConsumptionProduction(string SOID = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            var SO_ID = 0;
            if (SOID != "")
                SO_ID = Convert.ToInt32(SOID);
            string jsonstring = string.Empty;
            var result = _repository.GetDashboardUtilityConsumptionProduction(SO_ID, fromDate, toDate);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind Utility Consumption By Work Order dropdown
        public void ByWorkOrderNumberDropDown()
        {
            var model = _repository.GetAllWorkOrderNumber();
            var UtilityConsumptionByWorkOrder = new SelectList(model.ToList(), "ID", "WorkOrderNumber");
            ViewData["WorkOrderDD"] = UtilityConsumptionByWorkOrder;
        }
        #endregion

        #region Home Dashboard
        /// <summary>
        /// Shweta: fatching data for Main Dashboard
        /// </summary>
        public ActionResult HomeDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _repository.DashboardDataCount();

            return View(model);
        }

        #endregion
    }
}