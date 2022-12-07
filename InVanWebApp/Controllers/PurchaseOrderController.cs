using InVanWebApp.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using System.IO;
using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private ITermsConditionRepository _termsConditionRepository;
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public PurchaseOrderController()
        {
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _termsConditionRepository = new TermsConditionRepository();

            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            BindCurrencyPrice();
        }
        /// <summary>
        /// Rahul: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            //var model = _purchaseOrderRepository.GetAll();
            //return View(model);

            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _purchaseOrderRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dropdowns Company 
        public void BindCompany()
        {
            var result = _purchaseOrderRepository.GetCompanyList();
            var resultList = new SelectList(result.ToList(), "VendorsID", "CompanyName");
            ViewData["CompanyName"] = resultList;
        }
        #endregion

        #region Bind textarea Company Address 
        public JsonResult BindCompanyAddress(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _purchaseOrderRepository.GetCompanyAddressList(Id);
            //var resultList = new SelectList(result.ToList(), "VendorsID", "SupplierAddress");
            //var resultList = new SelectList(result.ToList(),"@ID", "SupplierAddress");
            //ViewData["SupplierAddress"] = resultList;
            return Json(result);
        }
        #endregion

        #region Bind dropdowns Terms And Condition 
        public void BindTermsAndCondition()
        {
            var result = _purchaseOrderRepository.GetTermsAndConditionList();
            var resultList = new SelectList(result.ToList(), "TermsAndConditionID", "Terms");
            ViewData["TermsAndConditionID"] = resultList;
        }
        #endregion

        #region Bind dropdowns Currency Price
        public void BindCurrencyPrice()
        {
            var result = _purchaseOrderRepository.GetCurrencyPriceList(); 
            var resultList = new SelectList(result.ToList(), "CurrencyID", "CurrencyName", "IndianCurrencyValue");
            ViewData["CurrencyName"] = resultList;
        }
        #endregion

        public JsonResult GetTermsDescription(string id)
        {
            int taxId = Convert.ToInt32(id);
            var result=_termsConditionRepository.GetById(taxId);
            return Json(result);
        }

        #region Bind dropdowns Location Name 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        #endregion

        #region Bind dropdowns Location Master  
        //public void BindOrganisations() 
        public JsonResult BindLocationMaster(string id)
        {
            //var result = _purchaseOrderRepository.GetOrganisationsList();            
            //var resultList = new SelectList(result.ToList(), "OrganisationId", "DeliveryAddress");

            //var resultList = new SelectList(result.ToList(), "LocationId", "LocationName", "DeliveryAddress");
            //ViewData["LocationName"] = resultList;
            //ViewData["DeliveryAddress"] = resultList;

            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _purchaseOrderRepository.GetLocationMasterList(Id);
            return Json(result);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Rendered the user to the add purchase order transaction form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPurchaseOrder()
        {
            //return View();

            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                //BindCompanyAddress();
                BindTermsAndCondition();
                //BindOrganisations();
                BindLocationName();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(2);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                PurchaseOrderBO model = new PurchaseOrderBO();
                model.PODate = DateTime.Today;
                model.DeliveryDate = DateTime.Today;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPurchaseOrder(PurchaseOrderBO model, HttpPostedFileBase Signature) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    //if (Signature != null || Attachment != null)
                    if (Signature != null)
                    {
                        UploadSignature(Signature);
                        //FileAttachment(Attachment); 
                        model.Signature = Signature.FileName.ToString();
                        //model.Attachment = Attachment.FileName.ToString();
                    }
                    else
                        model.Signature = null;
                        model.Attachment = null;

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _purchaseOrderRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Purchase Order inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Purchase Order! Can not be inserted!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            //BindOrganisations();
                            BindLocationName();
                            UploadSignature(Signature);
                            //FileAttachment(Attachment); 
                            return View(model);
                        }

                        return RedirectToAction("Index", "PurchaseOrder");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        //BindOrganisations();
                        BindLocationName();
                        UploadSignature(Signature);
                       // FileAttachment(Attachment); 
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
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

        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelpoItemsDetail"></param> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDraft(PurchaseOrderBO model, HttpPostedFileBase Signature)
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
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _purchaseOrderRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Purchase Order details saved as draft.!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Purchase Order as draft.! Can not be saved as draft.!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            //BindOrganisations();
                            BindLocationName();
                            UploadSignature(Signature);
                            return RedirectToAction("AddPurchaseOrder", "PurchaseOrder", model);
                        }

                        return RedirectToAction("Index", "PurchaseOrder");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        //BindOrganisations();
                        BindLocationName();
                        UploadSignature(Signature);
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
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

        #region Function for uploading the signature
        /// <summary>
        /// Date: 04 Oct 2022
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

        #region Function for Attachment File 
        /// <summary>
        /// Date: 06 Dce 2022 
        /// Farheen: Attachment File.
        /// </summary>
        /// <returns></returns>

        public void FileAttachment(HttpPostedFileBase Attachment)
        {
            if (Attachment != null)
            {
                string AttachFilename = Attachment.FileName;
                AttachFilename = Path.Combine(Server.MapPath("~/FileAttachment/"), AttachFilename);
                Attachment.SaveAs(AttachFilename); 

            }
        }

        #endregion

        #region  Update function
        /// <summary>
        ///Rahul: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPurchaseOrder(int PurchaseOrderId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindTermsAndCondition();
                BindLocationName();

                PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);

                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                string itemListForDD = "itemListForDD";

                if (model != null)
                {
                    var ItemCount = model.itemDetails.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        itemListForDD = "itemListForDD";
                        itemListForDD = itemListForDD + i;
                        dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.itemDetails[i].Item_ID);
                        ViewData[itemListForDD] = dd;
                        i++;
                    }

                }

                ViewData[itemListForDD] = dd;

                return View(model);
                //  return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        /// <summary>
        /// Rahul:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPurchaseOrder(PurchaseOrderBO model, HttpPostedFileBase Signature)
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
                    else if (Signature.ContentLength < 1000 && Signature!=null)
                        model.Signature = Signature.FileName.ToString();
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _purchaseOrderRepository.Update(model);
                        if (response.Status)
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Purchase order updated as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Purchase order updated successfully!');</script>";

                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindLocationName();
                            PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);
                            //Binding item grid with sell type item.
                            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model1 != null)
                            {
                                var ItemCount = model1.itemDetails.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.itemDetails[i].Item_ID);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }

                        return RedirectToAction("Index", "PurchaseOrder");
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
                return RedirectToAction("Index", "PurchaseOrder");
            }
        }

        #endregion

        #region Function for get item details
        public JsonResult GetitemDetails(string id, string currencyId)
        {
            var itemId = Convert.ToInt32(id);
            var currencyID = Convert.ToInt32(currencyId);
            var itemDetails = _purchaseOrderRepository.GetItemDetails(itemId, currencyID);
            //var finalDetials = itemDetails.Item_Name +"#"+ itemDetails.UnitName +"#"+ itemDetails.Price+"#"+itemDetails.Tax;
            //return Json(finalDetials);
            return Json(itemDetails);
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 07 Nov'22
        /// Rahul: Delete the perticular record Purchase Order 
        /// </summary>
        /// <param name="PurchaseOrderId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeletePurchaseOrder(int PurchaseOrderId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _purchaseOrderRepository.Delete(PurchaseOrderId, userID);
                TempData["Success"] = "<script>alert('Purchase Order deleted successfully!');</script>";
                return RedirectToAction("Index", "PurchaseOrder");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Amendment Operation
        /// <summary>
        /// Created By : Raj
        /// Created Date : 11-11-2022
        /// Edited by: Farheen (17 Nov'22)
        /// Description : Get Purchase Order Details and bind all Purchase Order for Amendment process.
        /// </summary>
        /// <param name="PurchaseOrderId">paramenter contrains purchase order Id.</param>
        /// <returns></returns>
        public ActionResult POAmendment(int PurchaseOrderId)
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

                    PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);
                    //Binding item grid with sell type item.
                    var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                    var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                    string itemListForDD = "itemListForDD";

                    if (model != null)
                    {
                        var ItemCount = model.itemDetails.Count;
                        var i = 0;
                        while (i < ItemCount)
                        {
                            itemListForDD = "itemListForDD";
                            itemListForDD = itemListForDD + i;
                            dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.itemDetails[i].Item_ID);
                            ViewData[itemListForDD] = dd;
                            i++;
                        }

                    }

                    ViewData[itemListForDD] = dd;

                    model.Amendment = model.Amendment + 1;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while amending the PO!');</script>";
                return RedirectToAction("Index", "PurchaseOrder");
            }
           
        }

        /// <summary>
        /// Created By: Raj
        /// Created Date: 11-11-2022
        /// Edited by: Farheen (17 Nov'22)
        /// Description: Insert Amendment Details of Purchase Order.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult POAmendment(PurchaseOrderBO model,HttpPostedFileBase Signature)
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

                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                       // response = _purchaseOrderRepository.SaveAmendment(model);
                        response = _purchaseOrderRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Amendment Details Added successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate category!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindLocationName();
                            PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);
                            //Binding item grid with sell type item.
                            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model1 != null)
                            {
                                var ItemCount = model1.itemDetails.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.itemDetails[i].Item_ID);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while amendment of PO!');</script>";
                return RedirectToAction("Index", "PurchaseOrder");
            }
            
        }

        #endregion

        #region View Purchase Order
        /// <summary>
        /// Created By: Raj
        /// Created Date : 12-11-2022
        /// Description: This method responsible for View of Purchase Order details.
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        public ActionResult ViewPurchaseOrder(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindCompany();
            BindTermsAndCondition();
            BindLocationName();

            //Binding item grid with sell type item.
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(ID);
            return View(model);

        }
        #endregion

    }
}