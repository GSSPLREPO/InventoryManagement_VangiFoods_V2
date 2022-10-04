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
using System.IO;

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
        /// Farheen: Rendered the user to the add inward master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInwardNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindPONumber();

                InwardNoteBO model = new InwardNoteBO();
                model.InwardDate = DateTime.Today;
                //==========Document number for Inward note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(3);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
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
        public ActionResult AddInwardNote(InwardNoteBO model, HttpPostedFileBase Signature)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        if (Signature != null)
                        {
                            string SignFilename = Signature.FileName;
                            SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                            Signature.SaveAs(SignFilename);
                            //string path = Server.MapPath("~/Signatures/");

                            //if (!Directory.Exists(path))
                            //{
                            //    Directory.CreateDirectory(path);
                            //}

                            //Signature.SaveAs(path + Path.GetFileName(Signature.FileName));
                            model.Signature = Signature.FileName.ToString();
                        }

                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Inward note created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Inward note! Can not be inserted!');</script>";
                            BindPONumber();
                            model.InwardDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(3);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "InwardNote");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindPONumber();
                        model.InwardDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(3);
                        ViewData["DocumentNo"] = DocumentNumber;

                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

                BindPONumber();
                model.InwardDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(3);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region  Update function
        /// <summary>28 Sep'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {

                BindPONumber();
                InwardNoteBO model = _repository.GetById(ID);
                string SignFilename = model.Signature;
                SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                ViewData["Signature"] = SignFilename;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditInwardNote(InwardNoteBO model, HttpPostedFileBase Signature)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (Signature != null)
                        {
                            string SignFilename = model.Signature;
                            SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                            Signature.SaveAs(SignFilename);
                            //string path = Server.MapPath("~/Signatures/");

                            //if (!Directory.Exists(path))
                            //{
                            //    Directory.CreateDirectory(path);
                            //}

                            //Signature.SaveAs(path + Path.GetFileName(Signature.FileName));
                            model.Signature = Signature.FileName.ToString();
                        }

                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Inward note updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate organisation! Can not be updated!');</script>";
                            BindPONumber();
                            return View(model);
                        }
                        return RedirectToAction("Index", "InwardNote");
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
                return RedirectToAction("Index", "InwardNote");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Sep'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Inward note deleted successfully!');</script>";
                return RedirectToAction("Index", "InwardNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dropdown of PO Number
        public void BindPONumber()
        {
            var result = _repository.GetPONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberAndId"] = resultList;
        }
        #endregion

        #region Bind all PO details 
        public JsonResult BindPODetails(string id,string InwId=null)
        {
            int POId = 0;
            int InwdId = 0;
            if (id != "" && id != null)
                POId= Convert.ToInt32(id);
            if (InwId != "" && InwId != null)
                InwdId = Convert.ToInt32(InwId);
            
            var result = _repository.GetPODetailsById(POId,InwdId);
            return Json(result);
        }
        #endregion

        #region This method is for View the Inward note
        [HttpGet]
        public ActionResult ViewInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                InwardNoteBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

    }
}