using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            var result = _POPaymentRepository.GetByID(ID);
            var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)result.PurchaseOrderId);
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
            result.PurchaseOrderItems = items;
            
            return View(result);
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
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            var result = _POPaymentRepository.GetByID(model.ID);
                            var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)result.PurchaseOrderId);
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
                            result.PurchaseOrderItems = items;
                            return View(result);
                        }
                        return RedirectToAction("Index", "POPayment");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Error while updating!');</script>";
                        var result = _POPaymentRepository.GetByID(model.ID);
                        var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)result.PurchaseOrderId);
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
                        result.PurchaseOrderItems = items;
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
                return RedirectToAction("Index", "POPayment");
            }
        }
        #endregion

        //#region This method is for View the PO payment
        //[HttpGet]
        //public ActionResult ViewPOPayment(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] != null)
        //    {
        //        var result = _POPaymentRepository.GetByID(ID);
        //        var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)result.PurchaseOrderId);
        //        List<PurchaseOrderItemsDetailBO> items = new List<PurchaseOrderItemsDetailBO>();
        //        foreach (var item in purchaseOrderItems)
        //        {
        //            PurchaseOrderItemsDetailBO purchaseOrderItem = new PurchaseOrderItemsDetailBO
        //            {
        //                CreatedBy = item.CreatedBy,
        //                CreatedDate = item.CreatedDate,
        //                ID = item.ID,
        //                IsDeleted = item.IsDeleted,
        //                ItemName = item.ItemName,
        //                ItemQuantity = item.ItemQuantity,
        //                ItemTaxValue = item.ItemTaxValue.ToString(),
        //                ItemUnit = item.ItemUnit,
        //                ItemUnitPrice = item.ItemUnitPrice,
        //                Item_Code = item.Item_Code,
        //                Item_ID = item.Item_ID,
        //                LastModifiedBy = item.LastModifiedBy,
        //                LastModifiedDate = item.LastModifiedDate,
        //                PurchaseOrderId = item.PurchaseOrderId,
        //                TotalItemCost = item.TotalItemCost
        //            };
        //            items.Add(purchaseOrderItem);
        //        }
        //        result.PurchaseOrderItems = items;

        //        return View(result);
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");
        //}
        //#endregion

        #region This method is for View the PO payment
        [HttpGet]
        public ActionResult ViewPOPayment(int ID)
        {
            Session["ID"] = ID;
            if (Session[ApplicationSession.USERID] != null)
            {
                var result = _POPaymentRepository.GetByID(ID);
                Session["POPaymentID"] = ID;
                var purchaseOrderItems = _POPaymentRepository.GetPOItemsByPurchaseOrderId((int)result.PurchaseOrderId);
                Session["PurchaseOrderID"] = result.PurchaseOrderId;
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
                result.PurchaseOrderItems = items;

                return View(result);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export PDF Bind GRN Item Details
        /// <summary>
        /// Create by Snehal on 19/06/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            StringBuilder sb = new StringBuilder();

            POPaymentBO POPayment = _POPaymentRepository.GetByID(Convert.ToInt32(Session["POPaymentID"]));
            var PurchaseOrder = _POPaymentRepository.GetPOItemsByPurchaseOrderId(Convert.ToInt32(Session["PurchaseOrderID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";

            string ReportName = "PO Payment Invoice";


            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>PO Payment </th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.PONumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payment Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(POPayment.PaymentDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Invoice Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.InvoiceNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Invoice Amount</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.PaymentAmount + " " + POPayment.CurrencyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Payment Due Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(POPayment.PaymentDueDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.VendorName + "</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:87%;font-size:15px;'>Item Details</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr style='text-align:center;padding: 5px; font-family:Times New Roman;background-color:#C0DBEA'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Units</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Tax</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Total Payable (Before Tax)</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            decimal TotalItemCost = 0;
            foreach (var item in PurchaseOrder)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + " INR" + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemTaxValue + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.TotalItemCost + "</td>");

                sb.Append("</tr>");
                i++;
                TotalItemCost = TotalItemCost + Convert.ToDecimal(item.TotalItemCost);
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<br/>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th colspan='3' style='text-align:left;padding: 2px; width:50%; font-family:Times New Roman;font-size:14px;'>PAYMENT MODE</th>");
            //sb.Append("<th colspan='2' style='border: 0.01px black;text-align:left;padding: 2px; width:50%; font-family:Times New Roman;font-size:14px;'>&nbsp;</th>");
            sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");

            sb.Append("<th colspan='2' style='text-align:LEFT;padding: 2px; width:50%; font-family:Times New Roman;font-size:14px;'>PAYMENT DETAILS</th>");
            //sb.Append("<th colspan='2' style='border: 0.01px black;text-align:Center;padding: 2px; width:50%; font-family:Times New Roman;font-size:14px;'>&nbsp;</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("</tr>");

            if (POPayment.PaymentMode == "Cheque")
            {
                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payment Mode</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.PaymentMode + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total PO Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + TotalItemCost + " " + POPayment.CurrencyName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Bank Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BankName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced Payment</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AdvancedPayment + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Branch Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BranchName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Paid Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AmountPaid + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Cheque Number</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.ChequeNumber + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payble Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.TotalPaybleAmount + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Balance Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.BalanceAmount + "</td>");
                sb.Append("</tr>");

            }
            else if (POPayment.PaymentMode == "Online")
            {
                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payment Mode</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.PaymentMode + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total PO Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + TotalItemCost + " " + POPayment.CurrencyName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Bank Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BankName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced Payment</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AdvancedPayment + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Branch Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BranchName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Paid Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AmountPaid + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Account Number</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.AccountNumber + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payble Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.TotalPaybleAmount + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Balance Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.BalanceAmount + "</td>");
                sb.Append("</tr>");

            }
            else if (POPayment.PaymentMode == "RTGS/NEFT")
            {
                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payment Mode</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.PaymentMode + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total PO Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + TotalItemCost + " " + POPayment.CurrencyName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Bank Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BankName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced Payment</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AdvancedPayment + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Branch Name</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.BranchName + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Paid Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AmountPaid + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Account Number</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.AccountNumber + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payble Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.TotalPaybleAmount + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>RTGS/NEFT IFSC Code</th>");
                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>RTGS/NEFT IFSC Code</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.IFSCCode + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Balance Amount (Rs) </th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.BalanceAmount + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>RTGS/NEFT IFSC Code</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.UTRNo + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'> </th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payment Mode</th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.PaymentMode + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total PO Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + TotalItemCost + " " + POPayment.CurrencyName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced Payment</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AdvancedPayment + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Paid Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.AmountPaid + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Payble Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.TotalPaybleAmount + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
                sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>  </td>");
                sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Balance Amount (Rs)</th>");
                sb.Append("<td style='text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + POPayment.BalanceAmount + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:1%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Paid/Un Paid</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.IsPaid + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:1%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Remarks</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + POPayment.Remarks + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "PO_Payment_Invoice_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    TempData["ReportName"] = ReportName.ToString();
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }
        #endregion

        #region PDF Helper both Set Border, Report Generated Date and Page Number Sheet

        #region Set Border
        /// <summary>
        /// setting border to pdf document
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pdfDoc"></param>
        public void setBorder(PdfWriter writer, Document pdfDoc)
        {
            //---------------------------------------
            var content = writer.DirectContent;
            var pageBorderRect = new Rectangle(pdfDoc.PageSize);

            pageBorderRect.Left += pdfDoc.LeftMargin - 15;
            pageBorderRect.Right -= pdfDoc.RightMargin - 15;
            pageBorderRect.Top -= pdfDoc.TopMargin - 7;
            pageBorderRect.Bottom += pdfDoc.BottomMargin - 5;

            //content.SetColorStroke(BaseColor.DARK_GRAY);
            //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom + 5, pageBorderRect.Width, pageBorderRect.Height);
            ////content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom - 5, pageBorderRect.Top, pageBorderRect.Right);
            //content.Stroke();

            //---------------------------------------

            content.SetColorStroke(BaseColor.RED);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();
        }
        #endregion

        #region Pdf Helper Class
        public class PageHeaderFooter : PdfPageEventHelper
        {
            private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);
            //private readonly Font _dateTime = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PurchaseOrderController purchaseOrderController = new PurchaseOrderController();
                purchaseOrderController.setBorder(writer, document);

                AddPageNumber(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                //----------------Font Value for Header & PageHeaderFooter--------------------
                Font plainFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                //--------------------------------------------For Generated Date-----------------------------------------------------
                var GeneratedDate = "Generated: " + DateTime.Now;
                var generatedDateTable = new PdfPTable(1);
                generatedDateTable.DefaultCell.Border = 0;

                var generatedDateCell = new PdfPCell(new Phrase(GeneratedDate, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(generatedDateCell);
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 135, document.Bottom - 5, writer.DirectContent);
                //-------------------------------------------For Generated Date-----------------------------------------------------

                //----------------------------------------For Page Number--------------------------------------------------
                var Page = "Page: " + writer.PageNumber.ToString();
                var pageNumberTable = new PdfPTable(1);

                pageNumberTable.DefaultCell.Border = 0;
                var pageNumberCell = new PdfPCell(new Phrase(Page, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                pageNumberCell.Border = 0;
                pageNumberTable.TotalWidth = 50;
                pageNumberTable.AddCell(pageNumberCell);
                pageNumberTable.WriteSelectedRows(0, 1, document.Right - 30, document.Bottom - 5, writer.DirectContent);
                //----------------------------------------For Page Number------------------------------------------------------
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                AddPageHeader(writer, document);
            }
            private void AddPageHeader(PdfWriter writer, Document document)
            {
                var text = ApplicationSession.ORGANISATIONTIITLE;

                var numberTable = new PdfPTable(1);
                numberTable.DefaultCell.Border = 0;
                var numberCell = new PdfPCell(new Phrase(text)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                numberCell.Border = 0;

                numberTable.TotalWidth = 200;
                numberTable.WriteSelectedRows(0, 1, document.Left - 40, document.Top + 25, writer.DirectContent);

            }
        }
        #endregion

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
            if (Session[ApplicationSession.USERID] != null)
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
                        ItemTaxValue = Math.Round((Convert.ToDouble(item.ItemTaxValue)), 2).ToString(),
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
            else
                return RedirectToAction("Index", "Login");

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