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
    public class POPaymentController : Controller
    {
        private IPOPaymentRepository _POPaymentRepository;
        private ICompanyRepository _CompanyRepository;
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Cunstructor(s)
        /// <summary>
        /// Raj: Constructor without parameter
        /// </summary>
        public POPaymentController()
        {
            _POPaymentRepository = new POPaymentRepository();
            _CompanyRepository = new CompanyRepository();
        }
        /// <summary>
        /// Raj: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="pOPaymentRepository"></param>
        /// <param name="CompanyRepository"></param>
        public POPaymentController(IPOPaymentRepository pOPaymentRepository, ICompanyRepository CompanyRepository)
        {
            _POPaymentRepository = pOPaymentRepository;
            _CompanyRepository = CompanyRepository;
        }
        #endregion

        #region Bind Grid
        /// <summary>
        /// Raj: Get the PO Payment Data and render to View.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _POPaymentRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Add Payment

        /// <summary>
        /// Raj:  This function renders the viewe of Add Purchas Order Payment form
        /// /// </summary>
        /// <returns></returns>
        public ActionResult AddPOPayment()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            POPaymentBO model = new POPaymentBO();

            BindPONumbers();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPOPayment(POPaymentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] == null)
                    return RedirectToAction("Index", "Login");

                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                    response = _POPaymentRepository.Insert(model);
                    if (response.Status)
                    {
                        BindPONumbers();
                        TempData["Success"] = "<script>alert('PO payment details inserted successfully!');</script>";
                        return RedirectToAction("Index", "POPayment");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Something went wrong! Please try again later.');</script>";
                        BindPONumbers();
                        return View(model);
                    }
                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

                    TempData["Success"] = "<script>alert('Please fill all mandatory fields!');</script>";
                    BindPONumbers();
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

        #region Edit PO Payment Details
        [HttpGet]
        public ActionResult EditPOPayment(int ID)
        {
            var paymentDetails = _POPaymentRepository.GetPOPaymentDetailsById(ID);
            var purchaseOrderDetail = _POPaymentRepository.GetPurchaseOrderById((int)paymentDetails.PurchaseOrderId);
            var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)paymentDetails.PurchaseOrderId);
            List<PurchaseOrderItemsDetailBO> items = new List<PurchaseOrderItemsDetailBO>();
            foreach (var item in purchaseOrderItems)
            {
                PurchaseOrderItemsDetailBO purchaseOrderItem = new PurchaseOrderItemsDetailBO
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
                    PurchaseOrderId = item.PurchaseOrderId,
                    TotalItemCost = item.TotalItemCost
                };
                items.Add(purchaseOrderItem);
            }

            POPaymentBO model = new POPaymentBO()
            {
                AccountNumber = paymentDetails.AccountNo,
                BalanceAmount = (decimal)paymentDetails.BalancePay,
                BankName = paymentDetails.BankName,
                ChequeNumber = paymentDetails.ChequeNo,
                ID = paymentDetails.ID,
                InvoiceNumber = paymentDetails.InvoiceNo,
                IsPaid = paymentDetails.PaymentStatus,
                PaymentAmount = (decimal)paymentDetails.InvoiceAmount,
                PaymentDate = Convert.ToDateTime(paymentDetails.PaymentDate),
                PaymentMode = paymentDetails.PaymentMode,
                PONumber = purchaseOrderDetail.PONumber,
                PurchaseOrderId = (int)paymentDetails.PurchaseOrderId,
                PurchaseOrderItems = items,
                TotalPaybleAmount = (int)paymentDetails.AmountPaid,
                TotalPOAmount = Convert.ToDecimal(purchaseOrderDetail.GrandTotal),
                VendorID = (int)purchaseOrderDetail.VendorsID,
                VendorName = purchaseOrderDetail.CompanyName,
                PaymentDueDate = paymentDetails.PaymentDueDate,
                Remarks = paymentDetails.Remarks
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPOPayment(POPaymentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _POPaymentRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Purchase Order Payment updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate organisation! Can not be updated!');</script>";
                            return View(model);
                        }
                        return RedirectToAction("Index", "POPayment");
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
                return RedirectToAction("Index", "POPayment");
            }
        }
        #endregion

        #region Remove PO Payment Details
        public ActionResult DeletePOPayment(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
            _POPaymentRepository.Delete(ID, userID);
            TempData["Success"] = "<script>alert('PO payment details deleted successfully!');</script>";
            return RedirectToAction("Index", "POPayment");
        }
        #endregion

        #region Get PurchaseOrderDetails
        /// <summary>
        /// Raj: This function used to filter the purchase Order using drop down selection of PO Numbers from view and retrun the json data.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PurchaseOrderDetails(int purchaseOrderId)
        {
            POPaymentBO model = new POPaymentBO();

            var purchaseDetails = _POPaymentRepository.GetPurchaseOrderById(purchaseOrderId);

            var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId(purchaseOrderId);
            List<PurchaseOrderItemsDetailBO> items = new List<PurchaseOrderItemsDetailBO>();
            foreach (var item in purchaseOrderItems)
            {
                PurchaseOrderItemsDetailBO purchaseOrderItem = new PurchaseOrderItemsDetailBO
                {
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ID = item.ID,
                    IsDeleted = item.IsDeleted,
                    ItemName = item.ItemName,
                    ItemQuantity = item.ItemQuantity,
                    ItemTaxValue = Math.Round((Convert.ToDouble(item.ItemTaxValue)),2).ToString(),
                    ItemUnit = item.ItemUnit,
                    ItemUnitPrice = item.ItemUnitPrice,
                    Item_Code = item.Item_Code,
                    Item_ID = item.Item_ID,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedDate = item.LastModifiedDate,
                    PurchaseOrderId = item.PurchaseOrderId,
                    TotalItemCost = item.TotalItemCost
                };
                items.Add(purchaseOrderItem);
            }


            model.PurchaseOrderId = purchaseOrderId;
            model.PONumber = purchaseDetails.PONumber;
            model.TotalPOAmount = Convert.ToDecimal(purchaseDetails.GrandTotal);
            model.AdvancedPayment = Convert.ToDecimal(purchaseDetails.AdvancedPayment);
            model.VendorID = (int)purchaseDetails.VendorsID;
            model.AmountPaid = purchaseDetails.AmountPaid;
            model.VendorName = _CompanyRepository.GetById(model.VendorID).CompanyName;
            model.PurchaseOrderItems = items;
            model.PaymentDueDate = DateTime.Today;
            model.PaymentDate = DateTime.Today;

            BindPONumbers();
            return PartialView("_POPaymentDetails", model);

        }
        #endregion

        #region Bind PONumbers DropDown
        private void BindPONumbers()
        {
            var result = _POPaymentRepository.GetPONumbers();
            var resultList = new SelectList(result.ToList(), "Key", "Value");
            ViewData["PONumbers"] = resultList;
        }
        #endregion
    }
}