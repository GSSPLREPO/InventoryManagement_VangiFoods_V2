using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class InventoryControlController : Controller
    {
        IInventoryControlRepository _InventoryControlRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockReportController));

        public InventoryControlController()
        {
            _InventoryControlRepository = new InventoryControlRepository();
        }

        // GET: InventoryControl
        public ActionResult Index()
        {
            return View();
        }

        #region Bind the grid
        /// <summary>
        /// 23/11/2022 Bhandavi
        /// Bind grid by fetching the stock details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInventoryControl()
        {
            var InventoryDetails = _InventoryControlRepository.GetAllInventoryControl();
            TempData["InventoryControl"] = InventoryDetails;
            return Json(new { data = InventoryDetails }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}