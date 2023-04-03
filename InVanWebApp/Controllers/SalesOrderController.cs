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
using System.IO;

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
                BindInquiryDropDown();
                BindCurrencyPrice();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(1);
                ViewData["DocumentNo"] = DocumentNumber;

                SalesOrderBO model = new SalesOrderBO();
                model.SODate = DateTime.Today;
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
        [HttpPost]
        public ActionResult AddSalesOrder(SalesOrderBO model, HttpPostedFileBase Signature)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (Signature != null)
                    {
                        UploadSignature(Signature);
                        model.Signature = Signature.FileName.ToString();
                    }
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.CreatedById = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);
                        if (response.Status)
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Sales Order inserted as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Sales Order inserted successfully!');</script>";

                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Sales Order! Can not be inserted!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindLocationName();
                            BindInquiryDropDown();
                            BindCurrencyPrice();

                            UploadSignature(Signature);
                            return View(model);
                        }

                        return RedirectToAction("Index", "SalesOrder");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        BindLocationName();
                        BindInquiryDropDown();
                        BindCurrencyPrice();

                        UploadSignature(Signature);
                       
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

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSalesOrder(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindTermsAndCondition();
                BindLocationName();

                SalesOrderBO model = _repository.GetSalesOrderById(Id);

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
        public ActionResult EditSalesOrder(SalesOrderBO model, HttpPostedFileBase Signature)
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (Signature != null && Signature.ContentLength > 1000)
                    {
                        UploadSignature(Signature);
                        model.Signature = Signature.FileName.ToString();
                    }
                    else if (Signature.ContentLength < 1000 && Signature != null)
                        model.Signature = Signature.FileName.ToString();
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedById = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        if (response.Status)
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Sales order updated as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Sales order updated successfully!');</script>";

                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindCurrencyPrice();
                            BindLocationName();
                            //BindIndentDropDown("POAmendment");
                            SalesOrderBO model1 = _repository.GetSalesOrderById(model.SalesOrderId);

                            return View(model1);
                        }

                        return RedirectToAction("Index", "SalesOrder");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        BindCurrencyPrice();
                        BindLocationName();

                        SalesOrderBO model1 = _repository.GetSalesOrderById(model.SalesOrderId);

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
                return RedirectToAction("Index", "SalesOrder");
            }
        }

        #endregion

        #region Amendment Operation
        /// <summary>
        /// Created By : Farheen
        /// Created Date : 23-03-2023
        /// Description : Get sales order Details and bind all sales order for amendment process.
        /// </summary>
        /// <param name="ID">paramenter contrains sales order Id.</param>
        /// <returns></returns>
        public ActionResult SOAmendment(int ID)
        {
            try
            {
                if (Session[ApplicationSession.USERID] == null)
                    return RedirectToAction("Index", "Login");
                else
                {
                    BindCompany();
                    BindTermsAndCondition();
                    BindLocationName();

                    SalesOrderBO model = _repository.GetSalesOrderById(ID);

                    model.Amendment = model.Amendment + 1;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while amending the SO!');</script>";
                return RedirectToAction("Index", "SalesOrder");
            }

        }

        /// <summary>
        /// Created By : Farheen
        /// Created Date : 23-03-2023
        /// Description: Insert Amendment Details of Sales order.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SOAmendment(SalesOrderBO model, HttpPostedFileBase Signature)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] == null)
                    return RedirectToAction("Index", "Login");
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (Signature != null && Signature.ContentLength > 1000)
                        {
                            UploadSignature(Signature);
                            model.Signature = Signature.FileName.ToString();
                        }
                        else if (Signature.ContentLength < 1000 && Signature != null)
                            model.Signature = Signature.FileName.ToString();
                        else
                            model.Signature = null;

                        model.CreatedById = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.IsAmendmentFlag = 1;
                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Amendment Details Added successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate entry!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindLocationName();

                            SalesOrderBO model1 = _repository.GetSalesOrderById(model.SalesOrderId);

                            return View(model1);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        
                        BindCompany();
                        BindTermsAndCondition();
                        BindLocationName();
                        
                        SalesOrderBO model1 = _repository.GetSalesOrderById(model.SalesOrderId);

                        return View(model1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while amendment of SO!');</script>";
                return RedirectToAction("Index", "SalesOrder");
            }

        }

        #endregion

        #region View Sales Order
        /// <summary>
        /// Created By: Farheen
        /// Created Date : 23-03-2023
        /// Description: This method responsible for View of sales order details.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ViewSalesOrder(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindCompany();
            BindTermsAndCondition();
            BindLocationName();


            SalesOrderBO model = _repository.GetSalesOrderById(ID);
            return View(model);

        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Mar'23
        /// Farheen: Delete the perticular record Sales Order 
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteSalesOrder(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Sales Order deleted successfully!');</script>";
                return RedirectToAction("Index", "SalesOrder");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Fetch inquiry details
        public JsonResult GetInquiryDescription(string id, string tempCurrencyId) 
        {
            int Id = Convert.ToInt32(id);
            int CurrencyId = Convert.ToInt32(tempCurrencyId);
            var result = _repository.GetInquiryFormById(Id, CurrencyId);
            return Json(result);
        }
        #endregion

        #region Function for uploading the signature
        /// <summary>
        /// Date: 21 Mar 2022
        /// Farheen: Upload Signature File items.
        /// </summary>
        /// <returns></returns>

        public void UploadSignature(HttpPostedFileBase Signature)
        {
            if (Signature != null)
            {
                string SignFilename = Signature.FileName;
                SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                Signature.SaveAs(SignFilename);

            }
        }

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
            ViewData["Terms_dd"] = resultList;
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
        public void BindInquiryDropDown()
        {
            var result = _repository.GetInquiryList();
            var resultList = new SelectList(result.ToList(), "InquiryID", "InquiryNumber");
            ViewData["InquiryDD"] = resultList;
        }
        public JsonResult GetWorkOrderNumber(string workOrderNo) 
        {
            GetDocumentNumber objDocNo = new GetDocumentNumber();
            //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetWorkOrderNo(18, workOrderNo);
            return Json(DocumentNumber);
        }

        #endregion
    }
}