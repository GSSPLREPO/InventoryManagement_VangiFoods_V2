using InVanWebApp.Common;
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
    public class BatchPlanningController : Controller
    {
        private IBatchPlanningRepository _batchPlanningRepository;
        private static ILog log = LogManager.GetLogger(typeof(BatchPlanningController));

        #region Initializing constructor
        /// <summary>
        /// Date: 23 March'23
        /// Rahul:  Constructor without parameter 
        /// </summary>
        public BatchPlanningController()
        {
            _batchPlanningRepository = new BatchPlanningRepository();
        }
        /// <summary>
        /// Date: 23 March'23
        /// Rahul:  Constructor with parameters for initializing the interface object.         
        /// </summary>
        ///<param name="itemRepository"></param>
        public BatchPlanningController(IBatchPlanningRepository batchPlanningRepository)
        {
            _batchPlanningRepository = batchPlanningRepository;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// Date: 23 March'23
        /// Rahul:  Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: BatchPlanning
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _batchPlanningRepository.GetAll();
                return View(model);                
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion
    }
}