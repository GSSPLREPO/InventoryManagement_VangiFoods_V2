using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp_BO;

namespace InVanWebApp.Controllers
{
    public class SalesOrderController : Controller
    {
        private ISalesOrderRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(SalesOrderController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public SalesOrderController()
        {
            _repository = new SalesOrderRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
          
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="salesOrderRepository"></param>
        public SalesOrderController(ISalesOrderRepository salesOrderRepository)
        {
            _repository = salesOrderRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: PurchaseOrder
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
        public ActionResult AddSalesOrder()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindTermsAndCondition();
                BindLocationName();
                //BindInquiryDropDown();
                BindCurrencyPrice();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(2);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                //var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                //var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                //ViewData["itemListForDD"] = dd;

                PurchaseOrderBO model = new PurchaseOrderBO();
                model.PODate = DateTime.Today;
                model.DeliveryDate = DateTime.Today;

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
        //[HttpPost]
        //public ActionResult AddSalesOrder(SalesOrderBO model, HttpPostedFileBase Signature)
        //{
        //    try
        //    {
        //        if (Session[ApplicationSession.USERID] != null)
        //        {
        //            ResponseMessageBO response = new ResponseMessageBO();

        //            if (Signature != null)
        //            {
        //                UploadSignature(Signature);
        //                model.Signature = Signature.FileName.ToString();
        //            }
        //            else
        //                model.Signature = null;

        //            if (ModelState.IsValid)
        //            {
        //                model.CreatedById = Convert.ToInt32(Session[ApplicationSession.USERID]);
        //                response = _purchaseOrderRepository.Insert(model);
        //                if (response.Status)
        //                {
        //                    if (model.DraftFlag == true)
        //                        TempData["Success"] = "<script>alert('Purchase order inserted as draft successfully!');</script>";
        //                    else
        //                        TempData["Success"] = "<script>alert('Purchase Order inserted successfully!');</script>";

        //                }
        //                else
        //                {
        //                    TempData["Success"] = "<script>alert('Duplicate Purchase Order! Can not be inserted!');</script>";
        //                    BindCompany();
        //                    BindTermsAndCondition();
        //                    BindCurrencyPrice();
        //                    BindLocationName();
        //                    BindIndentDropDown();
        //                    UploadSignature(Signature);
        //                    return View(model);
        //                }

        //                return RedirectToAction("Index", "PurchaseOrder");

        //            }
        //            else
        //            {
        //                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
        //                BindCompany();
        //                BindIndentDropDown();
        //                BindTermsAndCondition();
        //                BindCurrencyPrice();
        //                //BindOrganisations();
        //                BindLocationName();
        //                UploadSignature(Signature);
        //                //var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
        //                //var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
        //                //ViewData["itemListForDD"] = dd;
        //                return View(model);
        //            }
        //        }
        //        else
        //            return RedirectToAction("Index", "Login");

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error", ex);
        //    }
        //    return View();
        //}

        #endregion

        #region Dropdowns binding functions
        public void BindCompany()
        {
            var result = _purchaseOrderRepository.GetCompanyList();
            var resultList = new SelectList(result.ToList(), "VendorsID", "CompanyName");
            ViewData["CompanyName"] = resultList;
        }
        public void BindTermsAndCondition()
        {
            var result = _purchaseOrderRepository.GetTermsAndConditionList();
            var resultList = new SelectList(result.ToList(), "TermsAndConditionID", "Terms");
            ViewData["TermsAndConditionID"] = resultList;
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
        #endregion
    }
}