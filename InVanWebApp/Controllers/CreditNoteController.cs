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
        private IInwardNoteRepository _InwardRepository;
        private IGRNRepository _GRNrepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 07 Jan'23
        /// </summary>
        public CreditNoteController()
        {
            _repository = new CreditNoteRepository();
            _GRNrepository = new GRNRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _InwardRepository = new InwardNoteRepository();
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
                BindPONumber();
                BindLocationName();
                BindCurrencyPrice();
                BindCompany();
                CreditNoteBO model = new CreditNoteBO();
                model.CreditNoteDate = DateTime.Today;
                //==========Document number for GRN note============//
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
                           BindPONumber();
                            BindLocationName();
                            BindCurrencyPrice();
                            model.CreditNoteDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(7);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "GRN");

                    }
                    else
                    {
                        BindPONumber();
                        BindLocationName();
                        BindCurrencyPrice();
                        model.CreditNoteDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(7);
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
                BindLocationName();
                BindCurrencyPrice();
                model.CreditNoteDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(7);
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

        public JsonResult BindCreditNoteDetails(string id)
        {
            int CreditNoteId = 0;
            if (id != "" && id != null)
                CreditNoteId = Convert.ToInt32(id);

            var result = _repository.GetCreditNoteDetails(CreditNoteId);
            return Json(result);
        }
        #endregion

        #region Bind dropdowns 
        public void BindPONumber()
        {
            var result = _repository.GetPONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberAndId"] = resultList;
        }
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        public void BindCurrencyPrice()
        {
            var result = _purchaseOrderRepository.GetCurrencyPriceList();
            var resultList = new SelectList(result.ToList(), "CurrencyID", "CurrencyName");
            ViewData["CurrencyList"] = resultList;
        }
        public void BindCompany()
        {
            var result = _purchaseOrderRepository.GetCompanyList();
            var resultList = new SelectList(result.ToList(), "VendorsID", "CompanyName");
            ViewData["CompanyName"] = resultList;
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
    }
}