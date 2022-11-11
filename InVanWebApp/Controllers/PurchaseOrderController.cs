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
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public PurchaseOrderController()
        {
            _purchaseOrderRepository = new PurchaseOrderRepository();

            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
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
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                  ViewData["itemListForDD"] = dd;

                return View();
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

                    UploadSignature(Signature);
                    model.Signature = Signature.FileName.ToString();
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
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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

                    UploadSignature(Signature);
                    model.Signature = Signature.FileName.ToString();
                    if (ModelState.IsValid)
                    {

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
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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
                //BindCompanyAddress();
                BindTermsAndCondition();
                //BindOrganisations();
                BindLocationName();

                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                //PurchaseOrderBO model = _purchaseOrderRepository.GetById(PurchaseOrderId);
                PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId); 
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
        public ActionResult EditPurchaseOrder(PurchaseOrderBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _purchaseOrderRepository.Update(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('User updated successfully!');</script>";
                    else
                    {
                        TempData["Success"] = "<script>alert('Duplicate category!');</script>";
                        return View();
                    }

                    return RedirectToAction("Index", "PurchaseOrder");
                }
                else
                    return View(model);

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
        public JsonResult GetitemDetails(string id)
        {
            var itemId = Convert.ToInt32(id);
            var itemDetails = _purchaseOrderRepository.GetItemDetails(itemId);
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

    }
}