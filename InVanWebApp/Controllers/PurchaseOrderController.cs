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
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace InVanWebApp.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IIndentRepository _indentRepository;
        private IInwardNoteRepository _inwardNoteRepository;
        private ITermsConditionRepository _termsConditionRepository;

        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public PurchaseOrderController()
        {
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _indentRepository = new IndentRepository();
            _inwardNoteRepository = new InwardNoteRepository();
            _termsConditionRepository = new TermsConditionRepository();
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
                BindIndentDropDown();
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
                        {
                            if (model.DraftFlag == true)
                                TempData["Success"] = "<script>alert('Purchase order inserted as draft successfully!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Purchase Order inserted successfully!');</script>";
    
                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Purchase Order! Can not be inserted!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindCurrencyPrice();
                            BindLocationName();
                            BindIndentDropDown();
                            UploadSignature(Signature);
                            return View(model);
                        }

                        return RedirectToAction("Index", "PurchaseOrder");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindIndentDropDown();
                        BindTermsAndCondition();
                        BindCurrencyPrice();
                        //BindOrganisations();
                        BindLocationName();
                        UploadSignature(Signature);
                        //var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                        //var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        //ViewData["itemListForDD"] = dd;
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
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPurchaseOrder(int PurchaseOrderId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindTermsAndCondition();
                BindCurrencyPrice();
                BindLocationName();
                //BindIndentDropDown("POAmendment");

                PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);

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
                            BindCurrencyPrice();
                            BindLocationName();
                            //BindIndentDropDown("POAmendment");
                            PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);

                            return View(model1);
                        }

                        return RedirectToAction("Index", "PurchaseOrder");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        BindCurrencyPrice();
                        BindLocationName();
                        //BindIndentDropDown("POAmendment");
                        PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);

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
                return RedirectToAction("Index", "PurchaseOrder");
            }
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
                    BindCurrencyPrice();
                    BindLocationName();
                    //BindIndentDropDown("POAmendment");

                    PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);

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
                            TempData["Success"] = "<script>alert('Duplicate entry!');</script>";
                            BindCompany();
                            BindTermsAndCondition();
                            BindCurrencyPrice();
                            BindLocationName();
                            //BindIndentDropDown("POAmendment");

                            PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);

                            return View(model1);
                        }
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindTermsAndCondition();
                        BindCurrencyPrice();
                        BindLocationName();
                        //BindIndentDropDown("POAmendment");
                        PurchaseOrderBO model1 = _purchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId);
                        
                        return View(model1);
                    }
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

        //#region View Purchase Order
        ///// <summary>
        ///// Created By: Raj
        ///// Created Date : 12-11-2022
        ///// Description: This method responsible for View of Purchase Order details.
        ///// </summary>
        ///// <param name="PurchaseOrderId"></param>
        ///// <returns></returns>
        //public ActionResult ViewPurchaseOrder(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] == null)
        //        return RedirectToAction("Index", "Login");

        //    BindCompany();
        //    BindTermsAndCondition();
        //    BindCurrencyPrice();
        //    BindLocationName();
        //    //BindIndentDropDown("POAmendment");

        //    PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(ID,1);
        //    return View(model);

        //}
        //#endregion

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
            Session["PurchaseId"] = ID;
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindCompany();
            BindTermsAndCondition();
            BindCurrencyPrice();
            BindLocationName();
            //BindIndentDropDown("POAmendment");

            PurchaseOrderBO model = _purchaseOrderRepository.GetPurchaseOrderById(ID, 1);
            TempData["PurchaseOrderPDF"] = model;
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

            // List<PurchaseOrderBO> purchasePurchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(Convert.ToInt32(Session["PurchaseId"]), 1);

            //if (TempData["PurchaseOrderPDF"] == null)
            //{
            //    return View("Index");
            //}
            StringBuilder sb = new StringBuilder();
            PurchaseOrderBO purchaseOrderList = _purchaseOrderRepository.GetPurchaseOrderById(Convert.ToInt32(Session["PurchaseId"]), 1);
            //if (purchaseOrderList.Count <= 0)
            //    return View("Index");


            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + purchaseOrderList.Signature;
            string ReportName = "Purchase Order";

            string PODate = Convert.ToDateTime(purchaseOrderList.PODate).ToString("dd/MM/yyyy") + " ";
            string DeliveryDate = Convert.ToDateTime(purchaseOrderList.DeliveryDate).ToString("dd/MM/yyyy") + " ";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Title</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.Tittle + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>PO No.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.PONumber + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Doc. Dt.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + PODate + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Delivery Dt.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + DeliveryDate + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Amendment No.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.Amendment + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Indent No.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.IndentNumber + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.LocationName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Supplier Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.DeliveryAddress + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Delivery Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.SupplierAddress + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");

            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:3%;font-size:15px;'>Item Details</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody></tbody></table>");

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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item Description</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Units</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Currency</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Tax(%)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Total Before Tax</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            decimal totalBeforeTax = 0;
            decimal totalTax = 0;

            foreach (var item in purchaseOrderList.itemDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
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
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total tax:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + totalTax.ToString("0") + " INR" + "</td>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Other Tax:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.OtherTax + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (after tax):</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.TotalAfterTax + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Grand Total:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.GrandTotal + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced To Pay:</th>");
            sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + purchaseOrderList.AdvancedPayment + " INR" + "</td>");
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
            sb.Append("<td style='text-align:justify;padding: 5px;width:86%;font-size:11px; font-family:Times New Roman;'>" + purchaseOrderList.Terms + "</td>");
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
                    string filename = "Purchase_Order_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    TempData["ReportName"] = ReportName.ToString();
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

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
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 110, document.Bottom - 5, writer.DirectContent);
                //-------------------------------------------For Generated Date-----------------------------------------------------

                //--------------------------------------------For user Name-----------------------------------------------------
                var UserName = "Generated: " + DateTime.Now;
                var UserNameTable = new PdfPTable(1);
                UserNameTable.DefaultCell.Border = 0;

                var UserNameCell = new PdfPCell(new Phrase(UserName, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(UserNameCell);
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 210, document.Bottom - 5, writer.DirectContent);
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

        #endregion


        #region Transaction Timeline View for Purchase Order 
        /// <summary>
        /// Created By: Rahul 
        /// Created Date : 07-12-2022 
        /// Description: This method responsible for View of Purchase Order details. 
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        public ActionResult TimelineViewPurchaseOrder(int ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            PurchaseOrderBO modelPO = _purchaseOrderRepository.GetPurchaseOrderById(ID);
            PurchaseOrderBO timelinePO = _purchaseOrderRepository.GetDetailsForTimelineView(ID);
            InwardNoteBO inwardNote = _inwardNoteRepository.GetPOById(ID);

            if (timelinePO != null)
            {
                modelPO.GRNDate = (DateTime)timelinePO.GRNDate;
                modelPO.GRNCode = timelinePO.GRNCode;

                modelPO.PaymentDate = (DateTime)timelinePO.PaymentDate;
                modelPO.InvoiceNumber = timelinePO.InvoiceNumber;
            }
            if (inwardNote != null)
            {
                modelPO.InwardDate = (DateTime)inwardNote.InwardDate;
                modelPO.InwardNumber = inwardNote.InwardNumber;
            }
            //if (timelinePO.GRNCode != null)
            //{
            //    modelPO.GRNDate = (DateTime)timelinePO.GRNDate;
            //    modelPO.GRNCode = timelinePO.GRNCode;
            //}

            //if (timelinePO.InvoiceNumber != null)
            //{
            //    modelPO.PaymentDate = (DateTime)timelinePO.PaymentDate;
            //    modelPO.InvoiceNumber = timelinePO.InvoiceNumber;
            //}

            //if (inwardNote.InwardNumber != null)
            //{
            //    modelPO.InwardDate = (DateTime)inwardNote.InwardDate;
            //    modelPO.InwardNumber = inwardNote.InwardNumber;
            //}


            return PartialView("_TimelinePV", modelPO);

        }
        #endregion
        
        #region Bind dropdowns Company 
        public void BindCompany()
        {
            var result = _purchaseOrderRepository.GetCompanyList(1);
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
        
        #region Bind dropdowns Indent
        public void BindIndentDropDown(string type=null)
        {
            var result = _purchaseOrderRepository.GetIndentListForDropdown(type);
            var resultList = new SelectList(result.ToList(), "ID", "IndentNo");
            ViewData["IndentDD"] = resultList;
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

        #region Bind dropdowns Currency Price
        public void BindCurrencyPrice()
        {
            var result = _purchaseOrderRepository.GetCurrencyPriceList();
            var resultList = new SelectList(result.ToList(), "CurrencyID", "CurrencyName");
            ViewData["CurrencyList"] = resultList;
        }
        #endregion

        #region Bind descriptions of terms and Indent
        public JsonResult GetTermsDescription(string id)
        {
            int taxId = Convert.ToInt32(id);
            var result = _termsConditionRepository.GetById(taxId);
            return Json(result);
        }
        
        public JsonResult GetIndentDescription(string id, string tempCurrencyId)
        {
            int Id = Convert.ToInt32(id);
            int CurrencyId = Convert.ToInt32(tempCurrencyId);
            var result = _indentRepository.GetItemDetailsById(Id, CurrencyId);
            return Json(result);
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
        
        #region Function for get item details (Not in used in PO)
        public JsonResult GetitemDetails(string id,string currencyId)
        {
            var itemId = Convert.ToInt32(id);
            var currencyID = Convert.ToInt32(currencyId);
            var itemDetails = _purchaseOrderRepository.GetItemDetails(itemId, currencyID);
            //var finalDetials = itemDetails.Item_Name +"#"+ itemDetails.UnitName +"#"+ itemDetails.Price+"#"+itemDetails.Tax;
            //return Json(finalDetials);
            return Json(itemDetails);
        }
        #endregion
    }
}