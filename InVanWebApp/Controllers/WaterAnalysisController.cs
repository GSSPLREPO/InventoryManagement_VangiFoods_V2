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
    public class WaterAnalysisController : Controller
    {
        private IWaterAnalysisRepository _waterAnalysisRepository;
        private static ILog log = LogManager.GetLogger(typeof(WaterAnalysisController));

        #region Initializing constructor
        /// <summary>
        /// Date: 27/02/2023
        /// Yatri: Constructor without parameter
        /// </summary>
        public WaterAnalysisController()
        {
            _waterAnalysisRepository = new WaterAnalysisRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 27/02/2023
        ///Yatri: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _waterAnalysisRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri: Rendered the user to the add WaterAnalysis form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddWaterAnalysis()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                WaterAnalysisBO model = new WaterAnalysisBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Yatri: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddWaterAnalysis(WaterAnalysisBO model)
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
                        response = _waterAnalysisRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Water Analysis Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "WaterAnalysis");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "WaterAnalysis");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Yatri: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditWaterAnalysis(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                WaterAnalysisBO model = _waterAnalysisRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditWaterAnalysis(WaterAnalysisBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _waterAnalysisRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Water Analysis Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "WaterAnalysis");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "WaterAnalysis");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _waterAnalysisRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "WaterAnalysis");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}