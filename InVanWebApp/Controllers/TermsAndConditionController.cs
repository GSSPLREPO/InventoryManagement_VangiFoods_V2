using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class TermsAndConditionController : Controller
    {
        private ITermsConditionRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(TaxController));

        #region Initializing constructor
        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public TermsAndConditionController()
        {
            _repository = new TermsConditionRepository();
        }

        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="taxRepository"></param>
        public TermsAndConditionController(ITermsConditionRepository termsConditionRepository)
        {
            _repository = termsConditionRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 27 Nov'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
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

        #region Insert function
        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Rendered the user to the add location form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddTerms()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTerms(TermsAndConditionMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Terms & Condition inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate insertion!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "TermsAndCondition");
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
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 27 Nov'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTerms(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                TermsAndConditionMasterBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTerms(TermsAndConditionMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Terms & conditions updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate term name!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "TermsAndCondition");
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
                return RedirectToAction("Index", "TermsAndCondition");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTerms(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                int LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID,LastModifiedBy);
                //_unitRepository.Save();
                TempData["Success"] = "<script>alert('Terms & condition deleted successfully!');</script>";
                return RedirectToAction("Index", "TermsAndCondition");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}