using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;
using Newtonsoft.Json;

namespace InVanWebApp.Controllers
{
    public class Dashb_RTWarehouseWiseStockController : Controller
    {
        private IDashboardRepository _repository;
        private ILocationRepository _repositoryLocation;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public Dashb_RTWarehouseWiseStockController()
        {
            _repository = new DashboardRepository();
            _repositoryLocation = new LocationRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public Dashb_RTWarehouseWiseStockController(IDashboardRepository dashboardRepository)
        {
            _repository = dashboardRepository;
        }
        #endregion

        #region Bind real time warehouse wise dashboard
       
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindLocationDropdown();
            return View();
        }

        public JsonResult GetDashboardData(string id)
        {
            int LocationId = Convert.ToInt32(id);
            string jsonstring = string.Empty;

            var result = _repository.GetDashboardData(LocationId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind data reorder point of available total stock

        public ActionResult ReorderPointOfStocks()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public JsonResult GetDashboardDataReorderPointOfStocks()
        {
            string jsonstring = string.Empty;

            var result = _repository.GetReorderPointDashboardData();
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind location dropdown
        public void BindLocationDropdown()
        {
            var ItemType = _repositoryLocation.GetAll();
            var dd = new SelectList(ItemType.ToList(), "ID", "LocationName");
            ViewData["LocationName"] = dd;
        }

        #endregion
    }
}