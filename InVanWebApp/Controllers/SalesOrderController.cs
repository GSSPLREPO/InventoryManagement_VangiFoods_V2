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
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

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
                //model.DeliveryDate = DateTime.Today; //Rahul commented for bind Delivery date set from Inquiry 29-05-23.

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

            Session["SalesOrderID"] = ID;
            SalesOrderBO model = _repository.GetSalesOrderById(ID);
            TempData["SalesOrderPDF"] = model;
            return View(model);

        }
        #endregion

        #region Export PDF Purchase Order Report
        /// <summary>
        /// Create by Maharshi on 03/05/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {


            StringBuilder sb = new StringBuilder();
            SalesOrderBO SalesOrderList = _repository.GetSalesOrderById(Convert.ToInt32(Session["SalesOrderID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + SalesOrderList.Signature;
            string ReportName = "Sales Order";

            //string PODate = Convert.ToDateTime(SalesOrderList.PODate).ToString("dd/MM/yyyy") + " ";
            //string DeliveryDate = Convert.ToDateTime(SalesOrderList.DeliveryDate).ToString("dd/MM/yyyy") + " ";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>SO Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.SONo + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>SO Document Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.SODate + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Work Order Type</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.WorkOrderType + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Work Order Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.WorkOrderNo + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Delivery Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.DeliveryDate + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Amendment Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.Amendment + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inquiry Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.InquiryNumber + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Location Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.LocationName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Client Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Supplier Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.SupplierAddress + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Delivery Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.DeliveryAddress + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            //sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            //sb.Append("<thead>");
            //sb.Append("<tr>");
            //sb.Append("<th style='text-align:center; font-family:Times New Roman;width:3%;font-size:15px;'>Item Details</th>");
            //sb.Append("</tr>");
            //sb.Append("</thead>");
            //sb.Append("<tbody></tbody></table>");

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

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Order Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Price (Per Unit)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Currency</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Tax(%)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Total Before Tax</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            decimal totalBeforeTax = 0;
            decimal totalTax = 0;

            foreach (var item in SalesOrderList.salesOrderItemsDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemTaxValue + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.TotalItemCost + "</td>");

                sb.Append("</tr>");
                i++;
                totalBeforeTax = totalBeforeTax + Convert.ToDecimal(item.TotalItemCost);
                totalTax = totalTax + (Convert.ToDecimal(item.ItemTaxValue) / 100) * Convert.ToDecimal(item.TotalItemCost);
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th colspan='2' style='text-align:left;padding: 2px; width:60%; font-family:Times New Roman;font-size:14px;'></th>");

            sb.Append("<th colspan='2' style='text-align:Center;padding: 2px; width:40%; font-family:Times New Roman;font-size:14px;'>Payment Details</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<td rowspan='6' colspan='2' style='text-align:justify;font-size:10px; font-family:Times New Roman;padding-right:20px;'>" + "</td>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (before tax):</th>");
            sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + totalBeforeTax + " INR" + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Discount (%)</th>");
            sb.Append("<td style='text-align:right;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.DiscountPercentage + " INR" + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total tax:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + totalTax.ToString("0") + " INR" + "</td>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Other Tax:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.OtherTax + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (after tax):</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.TotalAfterTax + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Grand Total:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.GrandTotal + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:right;padding-right: -308px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:right;padding-right: -308px; font-family:Times New Roman;font-size:12px;'>Advanced To Pay:</th>");
            sb.Append("<td style='text-align:right;padding-right: -280px; font-size:12px; font-family:Times New Roman;'>" + SalesOrderList.AdvancedPayment + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
            sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<br />");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 5px; font-family:Times New Roman;font-size:13px;;'>Terms And Condition</ th>");
            sb.Append("</tr>");
            sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<td style='text-align:justify;padding: 5px;width:86%;font-size:11px; font-family:Times New Roman;'>" + SalesOrderList.Terms + "</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");


            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    //Document pdfDoc = new Document(PageSize.A4.Rotate());
                    Document pdfDoc = new Document(PageSize.A4);
                    //pdfDoc.SetPageSize(new Rectangle(850f, 1100f));
                    //pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);


                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Sales_Order_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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


        //#region View Sales Order
        ///// <summary>
        ///// Created By: Farheen
        ///// Created Date : 23-03-2023
        ///// Description: This method responsible for View of sales order details.
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public ActionResult ViewSalesOrder(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] == null)
        //        return RedirectToAction("Index", "Login");

        //    BindCompany();
        //    BindTermsAndCondition();
        //    BindLocationName();


        //    SalesOrderBO model = _repository.GetSalesOrderById(ID);
        //    return View(model);

        //}
        //#endregion

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
            var result = _purchaseOrderRepository.GetCompanyList(2);
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