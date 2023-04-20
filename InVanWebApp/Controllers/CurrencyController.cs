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
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace InVanWebApp.Controllers
{
    public class CurrencyController : Controller
    {
        private ICurrencyRepository _currencyRepository;
        private static ILog log = LogManager.GetLogger(typeof(CurrencyController));

        #region Initializing constructor
        /// <summary>
        /// Date: 19/04/2023
        /// Yatri: Constructor without parameter
        /// </summary>
        public CurrencyController()
        {
            _currencyRepository = new CurrencyRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 19/04/2023
        ///Yatri: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _currencyRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 19 April'23
        /// Yatri: Rendered the user to the add  Currency form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCurrency()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CurrencyBO model = new CurrencyBO();
                //model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                //model.Date = DateTime.Today;
                //model.Time= DateTime.Now.ToString("HH:mm:ss tt");
                //model.fromDate = DateTime.Now;
                //model.toDate = DateTime.Now;
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
        public ActionResult AddCurrency(CurrencyBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                       // model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        response = _currencyRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Currency Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplication Currency Name! Cannot be inserted!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "Currency");
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
        public ActionResult EditCurrency(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CurrencyBO model = _currencyRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 19 April'23
        /// Yatri:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCurrency(CurrencyBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _currencyRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Currency Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplication Currency Name! Cannot perform update!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "Currency");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "Currency");
                }
            }
            return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 19 April'23
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
                _currencyRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "Currency");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

    }
}