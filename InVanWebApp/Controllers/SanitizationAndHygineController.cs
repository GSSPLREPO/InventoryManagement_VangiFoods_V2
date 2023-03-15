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
    public class SanitizationAndHygineController : Controller
    {
        private ISanitizationAndHygineRepository _sanitizationAndHygineRepository;
        private static ILog log = LogManager.GetLogger(typeof(SanitizationAndHygineController));

        #region Initializing constructor
        /// <summary>
        /// Date: 22/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public SanitizationAndHygineController()
        {
            _sanitizationAndHygineRepository = new SanitizationAndHygineRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        ///Maharshi: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _sanitizationAndHygineRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Rendered the user to the add SanitizationAndHygine form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSanitizationAndHygine()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                SanitizationAndHygineBO model = new SanitizationAndHygineBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Maharshi: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSanitizationAndHygine(SanitizationAndHygineBO model)
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
                        response = _sanitizationAndHygineRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Sanitization and Hygine Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "SanitizationAndHygine");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "SanitizationAndHygine");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Maharshi: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSanitizationAndHygine(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                SanitizationAndHygineBO model = _sanitizationAndHygineRepository.GetById(Id);
                model.Date= DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Maharshi:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSanitizationAndHygine(SanitizationAndHygineBO model)
         {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                   
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.Date = DateTime.Today;
                        response = _sanitizationAndHygineRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Sanitization and Hygine Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "SanitizationAndHygine");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "SanitizationAndHygine");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Feb'23
        /// Maharshi: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _sanitizationAndHygineRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "SanitizationAndHygine");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}