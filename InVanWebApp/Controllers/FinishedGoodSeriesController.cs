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
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace InVanWebApp.Controllers
{
    public class FinishedGoodSeriesController : Controller
    {
        private IFinishedGoodSeriesRepository _finishedGoodSeriesRepository;
        private static ILog log = LogManager.GetLogger(typeof(FinishedGoodSeriesController));

        #region Initializing constructor
        /// <summary>
        /// Date: 22/03/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public FinishedGoodSeriesController()
        {
            _finishedGoodSeriesRepository = new FinishedGoodSeriesRepository();
        }

        #endregion

        #region  Bind Index Page
        /// <summary>
        /// Date: 22/03/2023
        ///Snehal: Get index view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Bind Datatable
        /// <summary>
        /// Develop By Snehal on 22-03-2023
        /// Calling method for FinishedGoodSeries
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllFinishedGoodSeriesList()
        {
            var finishedGoodSeriesBOs = _finishedGoodSeriesRepository.GetAllFinishedGoodSeriesList();
            //TempData["FinishedGoodSeriesPDF"] = finishedGoodSeriesBOs;
            //TempData["FinishedGoodSeriesExcel"] = finishedGoodSeriesBOs;
            return Json(new { data = finishedGoodSeriesBOs }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Date: 22-03-2023
        /// Snehal: Rendered the user to the add FinishedGoodSeries form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddFinishedGoodSeries()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindSONumber();

                FinishedGoodSeriesBO model = new FinishedGoodSeriesBO();
                //model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.MfgDate  = DateTime.Today;
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
        public ActionResult AddFinishedGoodSeries(FinishedGoodSeriesBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        //model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        response = _finishedGoodSeriesRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Finished Good Series Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            BindSONumber();
                            return View();
                        }
                        return RedirectToAction("Index", "FinishedGoodSeries");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    BindSONumber();
                    return RedirectToAction("Index", "FinishedGoodSeries");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Snehal: Rendered the user to the edit page with details of a particular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditFinishedGoodSeries(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindSONumber();
                FinishedGoodSeriesBO model = _finishedGoodSeriesRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22-03-2023
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFinishedGoodSeries(FinishedGoodSeriesBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _finishedGoodSeriesRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Finished Good Series Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            BindSONumber();
                            return View();
                        }
                        return RedirectToAction("Index", "FinishedGoodSeries");
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    BindSONumber();
                    return RedirectToAction("Index", "FinishedGoodSeries");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22-03-23
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
                _finishedGoodSeriesRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record Deleted Successfully!');</script>";
                return RedirectToAction("Index", "FinishedGoodSeries");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region This method is for View Details of Finished Good Series
        [HttpGet]
        public ActionResult ViewFinishedGoodSeries(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindSONumber();
                FinishedGoodSeriesBO model = _finishedGoodSeriesRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Bind Dorpdown
        public void BindSONumber()
        {
            var soNumber = _finishedGoodSeriesRepository.GetSONUmberForDropDown();
            var soNumberList = new SelectList(soNumber.ToList(), "SalesOrderId", "SONo");
            ViewData["SONumbers"] = soNumberList;
        }

        #endregion

        #region Bind BindWorkOrderNo
        public JsonResult BindWorkOrderNo(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _finishedGoodSeriesRepository.GetBindWorkOrderNo(Id);
            return Json(result);
        }
        #endregion
    }
}
