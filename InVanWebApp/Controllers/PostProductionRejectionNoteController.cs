using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using log4net;
using InVanWebApp.Common;
using InVanWebApp_BO;

namespace InVanWebApp.Controllers
{
    public class PostProductionRejectionNoteController : Controller
    {
        private IPostProductionRN_Repository _repository;
        private static ILog log = LogManager.GetLogger(typeof(FinishedGoodSeriesController));

        #region Initializing constructor
        /// <summary>
        /// Date: 25 Apr 2023
        /// Farheen: Constructor without parameter
        /// </summary>
        public PostProductionRejectionNoteController()
        {
            _repository = new PostProductionRN_Repository();
        }

        #endregion

        #region  Bind Index Page
        /// <summary>
        /// Date: 25/04/2023
        ///Farheen: Get index view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
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
        /// Farheen: Rendered the user to the add purchase order transaction form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPostProductionRejectionNote()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPostProductionRejectionNote(PostProductionRejectionNoteBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        //response = _repository.Insert(model);
                        if (response.Status)
                        {
                            TempData["Success"] = "<script>alert('Post-production RN inserted successfully!');</script>";

                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Post-production RN! Can not be inserted!');</script>";
                           
                            return View(model);
                        }

                        return RedirectToAction("Index", "PostProductionRejectionNote");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                       
                        return View(model);
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