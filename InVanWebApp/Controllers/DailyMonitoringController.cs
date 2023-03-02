using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using log4net;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using System.IO;

namespace InVanWebApp.Controllers
{
    public class DailyMonitoringController : Controller
    {
        private IDailyMonitoringRepository _dailyMonitoringRepository;
        private static ILog log = LogManager.GetLogger(typeof(DailyMonitoringController));

        #region Initializing constructor
        /// <summary>
        /// Date: 22/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public DailyMonitoringController()
        {
            _dailyMonitoringRepository = new DailyMonitoringRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 22/02/2023
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _dailyMonitoringRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Rendered the user to the add DailyMonitoring form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDailyMonitoring()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DailyMonitoringBO model = new DailyMonitoringBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Snehal: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDailyMonitoring(DailyMonitoringBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        response = _dailyMonitoringRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Daily Monitoring Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "DailyMonitoring");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "DailyMonitoring");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Snehal: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditDailyMonitoring(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DailyMonitoringBO model = _dailyMonitoringRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDailyMonitoring(DailyMonitoringBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _dailyMonitoringRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Daily Monitoring Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "DailyMonitoring");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "DailyMonitoring");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _dailyMonitoringRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "DailyMonitoring");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}