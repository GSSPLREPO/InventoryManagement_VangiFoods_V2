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
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.IO;

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
            var result = _purchaseOrderRepository.GetCompanyList(2);
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
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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
                            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
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
            Session["InquiryID"] = InquiryID;
            //Binding item grid with sell type item.
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            InquiryFormBO model = _inquiryFormRepository.GetInquiryFormById(InquiryID);
            TempData["InquiryID"] = model;
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
            InquiryFormBO InquiryFromList = _inquiryFormRepository.GetInquiryFormById(Convert.ToInt32(Session["InquiryID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + InquiryFromList.Signature;
            string ReportName = "Inquiry";

            //string PODate = Convert.ToDateTime(InquiryFromList.PODate).ToString("dd/MM/yyyy") + " ";
            string DeliveryDate = Convert.ToDateTime(InquiryFromList.DeliveryDate).ToString("dd/MM/yyyy") + " ";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -180px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Inquiry Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.InquiryNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Inquiry Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(InquiryFromList.DateOfInquiry).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Contact Person</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.ContactPersonName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Clint Email</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.ClientEmail + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.LocationName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Clint Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:20%;top:-10 px;padding-top: -12px; font-family:Times New Roman;font-size:12px; '>Supplier Address</th>");
            sb.Append("<td style='width:20%;top:0 px;padding-top: 0px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.DeliveryAddress + "</td>");
            sb.Append("<th style='width:20%;top:-10 px;padding-top: -12px; font-family:Times New Roman;font-size:12px; '>Client Address</th>");
            sb.Append("<td style='width:20%;top:-10 px;padding-top: -12px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.SupplierAddress + "</td>");    
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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Units</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Price Per Unit (RS)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Quoted Price (Per Unit)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Expected Price (Per Unit)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Closer Price (Per Unit)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Currency</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Tax (%)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Total Before Tax</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Delivery Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>HSN Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Remarks</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            decimal totalBeforeTax = 0;
            decimal totalTax = 0;

            foreach (var item in InquiryFromList.itemDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.QuotedPrice + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ExpectedPrice + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.CloserPrice + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemTaxValue + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.TotalItemCost + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.DeliveryDate + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.HSN_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Remarks + "</td>");

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
            //sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + totalBeforeTax + " INR" + "</td>");
            sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + totalBeforeTax + " INR" + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total tax:</th>");
            sb.Append("<td style='text-align:right;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + totalTax.ToString("0") + " INR" + "</td>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Grand Total</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.TotalAfterTax + " INR" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (after tax):</th>");
            sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.GrandTotal + " INR" + "</td>");
            sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Grand Total:</th>");
            //sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.GrandTotal + " INR" + "</td>");
            //sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced To Pay:</th>");
            sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + InquiryFromList.AdvancedPayment + " INR" + "</td>");
            sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
            //sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            //sb.Append("</tr>");
            //sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            //sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            //sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<br />");

            //sb.Append("<table>");
            //sb.Append("<thead>");
            //sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 5px; font-family:Times New Roman;font-size:13px;;'>Terms And Condition</ th>");
            //sb.Append("</tr>");
            //sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<td style='text-align:justify;padding: 5px;width:86%;font-size:11px; font-family:Times New Roman;'>" + InquiryFromList.Terms + "</td>");
            //sb.Append("</tr>");
            //sb.Append("</thead>");
            //sb.Append("</table>");


            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    Document pdfDoc = new Document(PageSize.A4.Rotate());
                    //Document pdfDoc = new Document(PageSize.A4);
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
                    string filename = "Inquiry_From_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
                InquiryFormController InquiryFormController = new InquiryFormController();
                InquiryFormController.setBorder(writer, document);

                AddPageNumber(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                //----------------Font Value for Header & PageHeaderFooter--------------------
                Font plainFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                //--------------------------------------------For Generated Date-----------------------------------------------------
                //var GeneratedDate = "Generated: " + DateTime.Now;
                var GeneratedDate = "Generated By: " + System.Web.HttpContext.Current.Session[ApplicationSession.USERNAME] + " On " + DateTime.Now;
                var generatedDateTable = new PdfPTable(1);
                generatedDateTable.DefaultCell.Border = 0;

                var generatedDateCell = new PdfPCell(new Phrase(GeneratedDate, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(generatedDateCell);
                //generatedDateTable.WriteSelectedRows(0, 1, document.Left - 135, document.Bottom - 5, writer.DirectContent);
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 50, document.Bottom - 5, writer.DirectContent);
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

        //#region View Inquiry Form 
        ///// <summary>
        ///// Created By: Rahul
        ///// Created Date : 05-01-2023. 
        ///// Description: This method responsible for View of Inquiry Form details. 
        ///// </summary>
        ///// <param name="InquiryID"></param>
        ///// <returns></returns>
        //public ActionResult ViewInquiryForm(int InquiryID)  
        //{
        //    if (Session[ApplicationSession.USERID] == null)
        //        return RedirectToAction("Index", "Login");

        //    BindCompany();            
        //    BindCurrencyPrice();
        //    BindLocationName();            

        //    //Binding item grid with sell type item.
        //    var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
        //    var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
        //    ViewData["itemListForDD"] = dd;
        //    InquiryFormBO model = _inquiryFormRepository.GetInquiryFormById(InquiryID); 
        //    return View(model);
        //}
        //#endregion

    }
}