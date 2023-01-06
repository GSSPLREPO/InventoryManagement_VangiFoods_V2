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
    public class InquiryFormController : Controller
    {
        private IInquiryFormRepository _inquiryFormRepository; 
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(InquiryFormController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public InquiryFormController()
        {
            _inquiryFormRepository = new InquiryFormRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1); //ItemType='Sell' 
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
        }
        /// <summary>
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="unitRepository"></param>
        public InquiryFormController(IInquiryFormRepository inquiryFormRepository)
        {
            _inquiryFormRepository = inquiryFormRepository;
        }
        #endregion


        #region  Bind grid
        /// <summary>
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: InquiryForm
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _inquiryFormRepository.GetAll();
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
            return Json(result);
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
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _purchaseOrderRepository.GetLocationMasterList(Id);
            return Json(result);
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

        #region Function for get item details
        public JsonResult GetitemDetails(string id, string currencyId)
        {
            var itemId = Convert.ToInt32(id);
            var currencyID = Convert.ToInt32(currencyId);
            var itemDetails = _purchaseOrderRepository.GetItemDetails(itemId, currencyID);            
            return Json(itemDetails);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Rendered the user to the add Request For Quotation form. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInquiryForm()
        {
            //return View();

            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindLocationName();
                BindCurrencyPrice(); 
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=10 i.e. for generating the Inquiry Form (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(10);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                InquiryFormBO model = new InquiryFormBO();
                model.DateOfInquiry = DateTime.Today;                
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
        public ActionResult AddInquiryForm(InquiryFormBO model) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();                                          

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _inquiryFormRepository.Insert(model); 
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Inquiry Form inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Inquiry Form! Can not be inserted!');</script>";
                            BindCompany();
                            BindCurrencyPrice();
                            BindLocationName();                                                      
                            return View(model);
                        }
                        return RedirectToAction("Index", "InquiryForm");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindCurrencyPrice();
                        BindLocationName();                       
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
        ///Rahul: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditInquiryForm(int InquiryID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();                
                BindCurrencyPrice();
                BindLocationName();
                
                InquiryFormBO model = _inquiryFormRepository.GetInquiryFormById(InquiryID);

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
        public ActionResult EditInquiryForm(InquiryFormBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {           

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _inquiryFormRepository.Update(model); 
                        if (response.Status)
                        {
                            TempData["Success"] = "<script>alert('Inquiry Form updated successfully!');</script>";
                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindCompany();                            
                            BindCurrencyPrice();
                            BindLocationName();                            
                            InquiryFormBO model1 = _inquiryFormRepository.GetInquiryFormById(model.InquiryID);
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
                        return RedirectToAction("Index", "InquiryForm");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();                        
                        BindCurrencyPrice();
                        BindLocationName();                        
                        InquiryFormBO model1 = _inquiryFormRepository.GetInquiryFormById(model.InquiryID);
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
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "InquiryForm");
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 03 Dec'23
        /// Rahul: Delete the perticular record InquiryForm Details. 
        /// </summary>
        /// <param name="InquiryID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteInquiryForm(int InquiryID) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                    _inquiryFormRepository.Delete(InquiryID, userID);
                    TempData["Success"] = "<script>alert('InquiryForm deleted successfully!');</script>";
                    return RedirectToAction("Index", "InquiryForm");
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

        #region View Inquiry Form 
        /// <summary>
        /// Created By: Rahul
        /// Created Date : 05-01-2023. 
        /// Description: This method responsible for View of Inquiry Form details. 
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        public ActionResult ViewInquiryForm(int InquiryID)  
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindCompany();            
            BindCurrencyPrice();
            BindLocationName();            

            //Binding item grid with sell type item.
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            InquiryFormBO model = _inquiryFormRepository.GetInquiryFormById(InquiryID); 
            return View(model);
        }
        #endregion

    }
}