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
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            else
            {
                BindSONumber();
                BindDocumentNo();
                PostProductionRejectionNoteBO model = new PostProductionRejectionNoteBO();
                model.PostProdRejectionNoteDate = DateTime.Now;
                return View(model);
            }
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

        #region Bind item details
        public JsonResult GetItemDetails(string FGS_Id,string Stage, string Type)
        {
            int FGSID = 0,FGSStage=0;

            if (FGS_Id != null & FGS_Id != "")
                FGSID = Convert.ToInt32(FGS_Id);
            if (Stage != null & Stage != "")
                FGSStage = Convert.ToInt32(Stage);

            var result = _repository.GetItemDetails(FGSID,FGSStage,Type);
            return Json(result);
        }
        #endregion

        #region Bind Dropdowns
        public void BindSONumber()
        {
            var result = _repository.BindWorkOrderDD();
            var resultList = new SelectList(result.ToList(), "FGSID", "WorkOrderNo");
            ViewData["WorkOrderDD"] = resultList;
        }

        public void BindDocumentNo()
        {
            GetDocumentNumber objDocNo = new GetDocumentNumber();
            var DocumentNumber = objDocNo.GetDocumentNo(21);
            ViewData["DocumentNo"] = DocumentNumber;
        }
        #endregion
    }
}