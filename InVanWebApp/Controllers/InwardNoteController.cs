using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class InwardNoteController : Controller
    {
        private IInwardNoteRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public InwardNoteController()
        {
            _repository = new InwardNoteRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public InwardNoteController(IInwardNoteRepository inwardNoteRepository)
        {
            _repository = inwardNoteRepository;
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
                var model = _repository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add organisation master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInwardNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindOrganisationGroup();
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
        public ActionResult AddInwardNote(InwardNoteBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                       // model.UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        //response = _organisationRepository.Insert(model);
                        //if (response.Status)
                        //    TempData["Success"] = "<script>alert('Organisation inserted successfully!');</script>";
                        //else
                        //{
                        //    TempData["Success"] = "<script>alert('Duplicate organisation! Can not be inserted!');</script>";
                        //    BindOrganisationGroup();
                        //    return View(model);
                        //}

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

    }
}