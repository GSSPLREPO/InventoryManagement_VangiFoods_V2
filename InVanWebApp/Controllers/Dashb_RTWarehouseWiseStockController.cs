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

namespace InVanWebApp.Controllers
{
    public class Dashb_RTWarehouseWiseStockController : Controller
    {
        private IDashboardRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public Dashb_RTWarehouseWiseStockController()
        {
            _repository = new DashboardRepository();
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

        #region Bind dashboard data
        // GET: Dashb_RTWarehouseWiseStock
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDashboardData()
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                { 
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return RedirectToAction("Index", "Dashb_RTWarehouseWiseStock");
        }

        #endregion
    }
}