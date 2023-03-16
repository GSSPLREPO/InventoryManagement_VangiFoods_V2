using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;

namespace InVanWebApp.Controllers
{
    public class SalesOrderController : Controller
    {
        private ISalesOrderRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(SalesOrderController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public SalesOrderController()
        {
            _repository = new SalesOrderRepository();
          
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="salesOrderRepository"></param>
        public SalesOrderController(ISalesOrderRepository salesOrderRepository)
        {
            _repository = salesOrderRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _repository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

    }
}