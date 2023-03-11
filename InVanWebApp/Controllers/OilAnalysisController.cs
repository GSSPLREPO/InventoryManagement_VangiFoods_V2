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
    public class OilAnalysisController : Controller
    {
        private IOilAnalysisRepository _oilAnalysisRepository;
        private static ILog log = LogManager.GetLogger(typeof(OilAnalysisController));

        #region Initializing constructor
        /// <summary>
        /// Date: 9/03/2023
        /// Yatri: Constructor without parameter
        /// </summary>
        public OilAnalysisController()
        {
            _oilAnalysisRepository = new OilAnalysisRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 9/03/2023
        ///Yatri: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _oilAnalysisRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 9 March'23
        /// Yatri: Rendered the user to the add  OilAnalysis form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOilAnalysis()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                OilAnalysisBO model = new OilAnalysisBO();
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
        public ActionResult AddOilAnalysis(OilAnalysisBO model)
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
                        response = _oilAnalysisRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Oil Analysis Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "OilAnalysis");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "OilAnalysis");
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
        public ActionResult EditOilAnalysis(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                OilAnalysisBO model = _oilAnalysisRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 9 March'23
        /// Yatri:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOilAnalysis(OilAnalysisBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _oilAnalysisRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Oil Analysis Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "OilAnalysis");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                { }


            }


                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 9 March'23
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
                _oilAnalysisRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "OilAnalysis");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}