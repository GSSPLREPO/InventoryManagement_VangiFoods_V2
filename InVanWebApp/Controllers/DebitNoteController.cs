using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class DebitNoteController : Controller
    {
        private IDebitNoteRepository _repository;
        private ICreditNoteRepository _creditNoteRepository;
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Constructor(s)

        /// <summary>
        /// Raj: Constructor without parameters
        /// </summary>
        public DebitNoteController()
        {
            _repository = new DebitNoteRepository();
            _creditNoteRepository = new CreditNoteRepository();
        }

        /// <summary>
        /// Raj: Constructor With Parameters for initalizing objects.
        /// </summary>
        /// <param name="debitNoteRepository"></param>
        public DebitNoteController(IDebitNoteRepository debitNoteRepository)
        {
            _repository = debitNoteRepository;
        }
        #endregion

        #region Bind Grid
        // GET: DebitNote
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model=_repository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add debit note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDebitNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindPONumber();
                GenerateDocumentNo();
                DebitNoteBO model = new DebitNoteBO();
                model.DebitNoteDate = DateTime.Today;                              

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
        public ActionResult AddDebitNote(DebitNoteBO model)
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
                            TempData["Success"] = "<script>alert('Debit Note created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate debit note! Can not be inserted!');</script>";
                            BindPONumber();
                            model.DebitNoteDate = DateTime.Today;

                            GenerateDocumentNo();

                            return View(model);
                        }

                        return RedirectToAction("Index", "DebitNote");

                    }
                    else
                    {
                        BindPONumber();
                        GenerateDocumentNo();
                        model.DebitNoteDate = DateTime.Today;                        

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
                GenerateDocumentNo();
                model.DebitNoteDate = DateTime.Today;

                return View(model);
            }
            //return View();
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 02 Feb'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteDebitNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Debit Note note deleted successfully!');</script>";
                return RedirectToAction("Index", "DebitNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Debit note Details
        [HttpGet]
        public ActionResult ViewDebitNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DebitNoteBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Bind dropdowns 
        public void BindPONumber()
        {
            var result = _creditNoteRepository.GetPONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberAndId"] = resultList;
        }

        public void GenerateDocumentNo() 
        {
            //==========Document number for GRN note============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=6 i.e. for generating the Debit note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(6);
            ViewData["DocumentNo"] = DocumentNumber;
        }
        #endregion

        #region Fetch PO details for debit note
        public JsonResult GetPODetails(string id)
        {
            int POId = 0;
            if (id != "" && id != null)
                POId = Convert.ToInt32(id);

            var result = _creditNoteRepository.GetPODetailsById(POId);
            return Json(result);
        }
        #endregion
    }
}