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
                        response = _repository.Insert(model);
                        if (response.Status)
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Post-production RN inserted as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Post-production RN inserted successfully!');</script>";

                        }
                        else
                        {
                            BindSONumber();
                            BindDocumentNo();
                            TempData["Success"] = "<script>alert('Duplicate Post-production RN! Can not be inserted!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "PostProductionRejectionNote");

                    }
                    else
                    {
                        BindSONumber();
                        BindDocumentNo();
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                BindSONumber();
                BindDocumentNo();
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Some error occurred!');</script>";
                return RedirectToAction("Index", "PostProductionRejectionNote");
            }
        }

        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPostProdRN(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindSONumber();
                PostProductionRejectionNoteBO model = _repository.GetById(Id);

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPostProdRN(PostProductionRejectionNoteBO model)
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
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Post-production RN updated as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Post-prodcution RN updated successfully!');</script>";

                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindSONumber();
                            PostProductionRejectionNoteBO model1 = _repository.GetById(model.ID);

                            return View(model1);
                        }

                        return RedirectToAction("Index", "PostProductionRejectionNote");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindSONumber();
                        PostProductionRejectionNoteBO model1 = _repository.GetById(model.ID);

                        return View(model1);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "PostProductionRejectionNote");
            }
        }

        #endregion

        #region View Post-production RN
        /// <summary>
        /// Created By: Farheen
        /// Created Date : 27-04-2023
        /// Description: This method responsible for View of post-production RN details.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ViewPostProdRN(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindSONumber();
            PostProductionRejectionNoteBO model = _repository.GetById(ID);

            return View(model);

        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 27 Apr'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeletePostProdRN(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Post-production RN deleted successfully!');</script>";
                return RedirectToAction("Index", "PostProductionRejectionNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind item details
        public JsonResult GetItemDetails(string FGS_Id, string Stage, string Type)
        {
            int FGSID = 0, FGSStage = 0;

            if (FGS_Id != null & FGS_Id != "")
                FGSID = Convert.ToInt32(FGS_Id);
            if (Stage != null & Stage != "")
                FGSStage = Convert.ToInt32(Stage);

            var result = _repository.GetItemDetails(FGSID, FGSStage, Type);
            return Json(result);
        }
        #endregion

        #region Bind Dropdowns
        public void BindSONumber()
        {
            var result = _repository.BindWorkOrderDD();
            var resultList = new SelectList(result.ToList(), "FGSID", "WorkOrderAndBN");
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