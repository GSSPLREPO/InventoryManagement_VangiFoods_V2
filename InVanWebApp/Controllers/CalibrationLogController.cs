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
    public class CalibrationLogController : Controller
    {
        private ICalibrationLogRepository _calibrationLogRepository;
        private static ILog log = LogManager.GetLogger(typeof(CalibrationLogController));

        #region Initializing constructor
        /// <summary>
        /// Date: 20/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public CalibrationLogController()
        {
            _calibrationLogRepository = new CalibrationLogRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 20/02/2023
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _calibrationLogRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Rendered the user to the add CalibrationLog form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCalibrationLog()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CalibrationLogBO model = new CalibrationLogBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.CalibrationDoneDate = DateTime.Today;
                model.CalibrationDueDate = DateTime.Today;
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
        public ActionResult AddCalibrationLog(CalibrationLogBO model)
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
                        response = _calibrationLogRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Calibration Log Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "CalibrationLog");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "CalibrationLog");
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
        public ActionResult EditCalibrationLog(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CalibrationLogBO model = _calibrationLogRepository.GetById(Id);
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
        public ActionResult EditCalibrationLog(CalibrationLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _calibrationLogRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Calibration Log details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "CalibrationLog");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "CalibrationLog");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 20 Feb'23
        /// Snehal: Delete the perticular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _calibrationLogRepository.Delete(Id, userID);

                //_calibrationLogRepository.Delete(Id);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "CalibrationLog");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}