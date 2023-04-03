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
    public class SOPaymentController : Controller
    {
        private ISOPaymentRepository _SOPaymentRepository;
        private ICompanyRepository _CompanyRepository;
        private static ILog log = LogManager.GetLogger(typeof(SOPaymentController));

        #region Initializing Cunstructor(s)
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public SOPaymentController()
        {
            _SOPaymentRepository = new SOPaymentRepository();
            _CompanyRepository = new CompanyRepository();
        }
        /// <summary>
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="sOPaymentRepository"></param>
        /// <param name="CompanyRepository"></param>
        public SOPaymentController(ISOPaymentRepository sOPaymentRepository, ICompanyRepository CompanyRepository)
        {
            _SOPaymentRepository = sOPaymentRepository;
            _CompanyRepository = CompanyRepository;
        }
        #endregion

        #region Bind Grid
        /// <summary>
        /// Rahul: Get the SO Payment Data and render to View.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _SOPaymentRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Add Payment

        /// <summary>
        /// Rahul:  This function renders the view of Add Sales Order Payment form
        /// /// </summary>
        /// <returns></returns>
        public ActionResult AddSOPayment()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            SOPaymentBO model = new SOPaymentBO();

            BindSONumbers();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSOPayment(SOPaymentBO model)
         {
            try
            {
                if (Session[ApplicationSession.USERID] == null)
                    return RedirectToAction("Index", "Login");

                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                    response = _SOPaymentRepository.Insert(model);
                    if (response.Status)
                    {
                        BindSONumbers();
                        TempData["Success"] = "<script>alert('SO payment details inserted successfully!');</script>";
                        return RedirectToAction("Index", "SOPayment");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Something went wrong! Please try again later.');</script>";
                        BindSONumbers();
                        return View(model);
                    }
                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

                    TempData["Success"] = "<script>alert('Please fill all mandatory fields!');</script>";
                    BindSONumbers();
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View();
        }
        #endregion

        #region Edit SO Payment Details
        [HttpGet]
        public ActionResult EditSOPayment(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            var result = _SOPaymentRepository.GetByID(ID);
            var salesOrderItems = _SOPaymentRepository.GetSOItemsBySalesOrderId((int)result.SalesOrderId);
            List<SalesOrderItemsDetailBO> items = new List<SalesOrderItemsDetailBO>();
            foreach (var item in salesOrderItems)
            {
                SalesOrderItemsDetailBO salesOrderItem = new SalesOrderItemsDetailBO
                {
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ID = item.ID,
                    IsDeleted = item.IsDeleted,
                    ItemName = item.ItemName,
                    ItemQuantity = item.ItemQuantity,
                    ItemTaxValue = item.ItemTaxValue.ToString(),
                    ItemUnit = item.ItemUnit,
                    ItemUnitPrice = item.ItemUnitPrice,
                    Item_Code = item.Item_Code,
                    Item_ID = item.Item_ID,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedDate = item.LastModifiedDate,
                    SalesOrderId = item.SalesOrderId,
                    TotalItemCost = item.TotalItemCost
                };
                items.Add(salesOrderItem);
            }
            result.SalesOrderItems = items;
            
            return View(result);
        }

        [HttpPost]
        public ActionResult EditSOPayment(SOPaymentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _SOPaymentRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Sales Order Payment updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            var result = _SOPaymentRepository.GetByID(model.ID);
                            var salesOrderItems = _SOPaymentRepository.GetSOItemsBySalesOrderId((int)result.SalesOrderId);
                            List<SalesOrderItemsDetailBO> items = new List<SalesOrderItemsDetailBO>();
                            foreach (var item in salesOrderItems)
                            {
                                SalesOrderItemsDetailBO salesOrderItem = new SalesOrderItemsDetailBO
                                {
                                    CreatedBy = item.CreatedBy,
                                    CreatedDate = item.CreatedDate,
                                    ID = item.ID,
                                    IsDeleted = item.IsDeleted,
                                    ItemName = item.ItemName,
                                    ItemQuantity = item.ItemQuantity,
                                    ItemTaxValue = item.ItemTaxValue.ToString(),
                                    ItemUnit = item.ItemUnit,
                                    ItemUnitPrice = item.ItemUnitPrice,
                                    Item_Code = item.Item_Code,
                                    Item_ID = item.Item_ID,
                                    LastModifiedBy = item.LastModifiedBy,
                                    LastModifiedDate = item.LastModifiedDate,
                                    SalesOrderId = item.SalesOrderId,
                                    TotalItemCost = item.TotalItemCost
                                };
                                items.Add(salesOrderItem);
                            }
                            result.SalesOrderItems = items;
                            return View(result);
                        }
                        return RedirectToAction("Index", "SOPayment");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Error while updating!');</script>";
                        var result = _SOPaymentRepository.GetByID(model.ID);
                        var salesOrderItems = _SOPaymentRepository.GetSOItemsBySalesOrderId((int)result.SalesOrderId);
                        List<SalesOrderItemsDetailBO> items = new List<SalesOrderItemsDetailBO>();
                        foreach (var item in salesOrderItems)
                        {
                            SalesOrderItemsDetailBO salesOrderItem = new SalesOrderItemsDetailBO
                            {
                                CreatedBy = item.CreatedBy,
                                CreatedDate = item.CreatedDate,
                                ID = item.ID,
                                IsDeleted = item.IsDeleted,
                                ItemName = item.ItemName,
                                ItemQuantity = item.ItemQuantity,
                                ItemTaxValue = item.ItemTaxValue.ToString(),
                                ItemUnit = item.ItemUnit,
                                ItemUnitPrice = item.ItemUnitPrice,
                                Item_Code = item.Item_Code,
                                Item_ID = item.Item_ID,
                                LastModifiedBy = item.LastModifiedBy,
                                LastModifiedDate = item.LastModifiedDate,
                                SalesOrderId = item.SalesOrderId,
                                TotalItemCost = item.TotalItemCost
                            };
                            items.Add(salesOrderItem);
                        }
                        result.SalesOrderItems = items;
                        return View(result);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "SOPayment");
            }
        }
        #endregion

        #region This method is for View the SO payment
        [HttpGet]
        public ActionResult ViewSOPayment(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var result = _SOPaymentRepository.GetByID(ID);
                var salesOrderItems = _SOPaymentRepository.GetSOItemsBySalesOrderId((int)result.SalesOrderId);
                List<SalesOrderItemsDetailBO> items = new List<SalesOrderItemsDetailBO>();
                foreach (var item in salesOrderItems)
                {
                    SalesOrderItemsDetailBO salesOrderItem = new SalesOrderItemsDetailBO
                    {
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        ID = item.ID,
                        IsDeleted = item.IsDeleted,
                        ItemName = item.ItemName,
                        ItemQuantity = item.ItemQuantity,
                        ItemTaxValue = item.ItemTaxValue.ToString(),
                        ItemUnit = item.ItemUnit,
                        ItemUnitPrice = item.ItemUnitPrice,
                        Item_Code = item.Item_Code,
                        Item_ID = item.Item_ID,
                        LastModifiedBy = item.LastModifiedBy,
                        LastModifiedDate = item.LastModifiedDate,
                        SalesOrderId = item.SalesOrderId,
                        TotalItemCost = item.TotalItemCost
                    };
                    items.Add(salesOrderItem); 
                }
                result.SalesOrderItems = items;

                return View(result);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Remove SO Payment Details
        public ActionResult DeleteSOPayment(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
            _SOPaymentRepository.Delete(ID, userID);
            TempData["Success"] = "<script>alert('SO payment details deleted successfully!');</script>";
            return RedirectToAction("Index", "SOPayment");
        }
        #endregion

        #region Get SalesOrderDetails
        /// <summary>
        /// Rahul: This function used to filter the purchase Order using drop down selection of SO Numbers from view and retrun the json data.
        /// </summary>
        /// <param name="salesOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesOrderDetails(int salesOrderId) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {

                SOPaymentBO model = new SOPaymentBO();

                var salesDetails = _SOPaymentRepository.GetSalesOrderById(salesOrderId); 

                var salesOrderItems = _SOPaymentRepository.GetSOItemsBySalesOrderId(salesOrderId);
                List<SalesOrderItemsDetailBO> items = new List<SalesOrderItemsDetailBO>();
                foreach (var item in salesOrderItems)
                {
                    SalesOrderItemsDetailBO salesOrderItem = new SalesOrderItemsDetailBO 
                    {
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        ID = item.ID,
                        IsDeleted = item.IsDeleted,
                        ItemName = item.ItemName,
                        ItemQuantity = item.ItemQuantity,
                        ItemTaxValue = Math.Round((Convert.ToDouble(item.ItemTaxValue)), 2).ToString(),
                        ItemUnit = item.ItemUnit,
                        ItemUnitPrice = item.ItemUnitPrice,
                        Item_Code = item.Item_Code,
                        Item_ID = item.Item_ID,
                        LastModifiedBy = item.LastModifiedBy,
                        LastModifiedDate = item.LastModifiedDate,
                        SalesOrderId = item.SalesOrderId,
                        TotalItemCost = item.TotalItemCost
                    };
                    items.Add(salesOrderItem);
                }


                model.SalesOrderId = salesOrderId;
                model.SONumber = salesDetails.SONumber; 
                model.TotalPOAmount = Convert.ToDecimal(salesDetails.GrandTotal);
                model.AdvancedPayment = Convert.ToDecimal(salesDetails.AdvancedPayment);
                model.VendorID = (int)salesDetails.VendorsID;
                model.AmountPaid = salesDetails.AmountPaid;
                model.VendorName = _CompanyRepository.GetById(model.VendorID).CompanyName;
                model.SalesOrderItems = items;
                model.PaymentDueDate = DateTime.Today;
                model.PaymentDate = DateTime.Today;

                BindSONumbers(); 
                return PartialView("_SOPaymentDetails", model);
            }
            else
                return RedirectToAction("Index", "Login");

        }
        #endregion

        #region Bind SONumbers DropDown 
        private void BindSONumbers()
        {
            var result = _SOPaymentRepository.GetSONumbers(); 
            var resultList = new SelectList(result.ToList(), "Key", "Value");
            ViewData["SONo"] = resultList; 
        }
        #endregion
    }
}