using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using InVanWebApp.Common;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Controllers
{
    public class CreditNoteController : Controller
    {
        private ICreditNoteRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(CreditNoteController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 07 Jan'23
        /// </summary>
        public CreditNoteController()
        {
            _repository = new CreditNoteRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 07 Jan'23
        /// </summary>
        /// <param name="creditNoteRepository"></param>
        public CreditNoteController(ICreditNoteRepository creditNoteRepository)
        {
            _repository = creditNoteRepository;
        }
        #endregion

        #region Bind Grid
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _repository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add Credit note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCreditNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindPONumber();
                BindSONumber();
                CreditNoteBO model = new CreditNoteBO();
                model.CreditNoteDate = DateTime.Today;
                //==========Document number for Credit note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=11 i.e. for generating the Credit note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(11);
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
        public ActionResult AddCreditNote(CreditNoteBO model)
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
                            TempData["Success"] = "<script>alert('Credit Note created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Credit note! Can not be inserted!');</script>";
                            // BindPONumber();
                            BindSONumber();
                            model.CreditNoteDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(11);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "CreditNote");

                    }
                    else
                    {
                        //BindPONumber();
                        BindSONumber();
                        model.CreditNoteDate = DateTime.Today;
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(11);
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

                //BindPONumber();
                BindSONumber();
                model.CreditNoteDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(11);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Nov'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteCreditNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Credit note deleted successfully!');</script>";
                return RedirectToAction("Index", "CreditNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Credit note Details
        [HttpGet]
        public ActionResult ViewCreditNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CreditNoteBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Bind dropdowns 
        public void BindPONumber()
        {
            var result = _repository.GetPONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberAndId"] = resultList;
        }
        public void BindSONumber()
        {
            var result = _repository.GetSONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONumber");
            ViewData["SONumberAndId"] = resultList;
        }

        #endregion

        #region Fetch PO details for creditNote
        public JsonResult GetPODetails(string id)
        {
            int POId = 0;
            if (id != "" && id != null)
                POId = Convert.ToInt32(id);

            var result = _repository.GetPODetailsById(POId);
            return Json(result);
        }
        #endregion

        #region Fetch SO details for creditNote
        public JsonResult GetSODetails(string id)
        {
            int SOId = 0;
            if (id != "" && id != null)
                SOId = Convert.ToInt32(id);

            var result = _repository.GetSODetailsById(SOId);
            return Json(result);
        }
        #endregion

    }
}