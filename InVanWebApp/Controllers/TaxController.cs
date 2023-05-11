using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Controllers
{
    public class TaxController : Controller
    {
        private ITaxRepository _taxRepository;
        private static ILog log = LogManager.GetLogger(typeof(TaxController));

        #region Initializing constructor
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public TaxController()
        {
            _taxRepository = new TaxRepository();
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="taxRepository"></param>
        public TaxController(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 19 Aug'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _taxRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Rendered the user to the add location form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddTax()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTax(TaxBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _taxRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Tax inserted successfully!');</script>";
                        else
                        { 
                            TempData["Success"] = "<script>alert('Duplicate tax name! Cannot perform insert!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "Tax");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return View();
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                //TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                //return RedirectToAction("Index", "Tax");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 19 Aug'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTax(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                TaxBO model = _taxRepository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTax(TaxBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _taxRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Tax updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate tax name! Cannot perform update!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "Tax");
                    }
                    else
                        return View(model);
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "Tax");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTax(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                _taxRepository.Delete(ID);
                //_unitRepository.Save();
                TempData["Success"] = "<script>alert('Tax deleted successfully!');</script>";
                return RedirectToAction("Index", "Tax");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}