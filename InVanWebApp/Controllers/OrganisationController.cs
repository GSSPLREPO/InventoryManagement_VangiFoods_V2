using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class OrganisationController : Controller
    {
        private IOrganisationRepository _organisationRepository;
        private static ILog log = LogManager.GetLogger(typeof(OrganisationController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public OrganisationController()
        {
            _organisationRepository = new OrganisationRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public OrganisationController(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _organisationRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dropdowns organisation group

        public void BindOrganisationGroup()
        {
            var result = _organisationRepository.GetOrganisationGroupList();
            var resultList = new SelectList(result.ToList(), "OrganisationGroupId", "Name");
            ViewData["OrganisationGroup"] = resultList;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add organisation master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrganisation()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindOrganisationGroup();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrganisation(OrganisationsBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        response = _organisationRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Organisation inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate organisation! Can not be inserted!');</script>";
                            BindOrganisationGroup();
                            return View(model);
                        }

                        return RedirectToAction("Index", "Organisation");

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
        /// Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditOrganisation(int OrganisationId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindOrganisationGroup();
                OrganisationsBO model = _organisationRepository.GetById(OrganisationId);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrganisation(OrganisationsBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _organisationRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Organisation updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate organisation! Can not be updated!');</script>";
                            BindOrganisationGroup();
                            return View(model);
                        }



                        return RedirectToAction("Index", "Organisation");
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
                return RedirectToAction("Index", "Organisation");
            }
        }

        #endregion

        #region Delete function

        [HttpGet]
        public ActionResult DeleteOrganisation(int OrganisationId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _organisationRepository.Delete(OrganisationId, userID);
                TempData["Success"] = "<script>alert('Organisation deleted successfully!');</script>";
                return RedirectToAction("Index", "Organisation");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}