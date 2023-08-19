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
using System.Web.Script.Serialization;

namespace InVanWebApp.Controllers
{
    public class RequestForQuotationController : Controller
    {
        private IRequestForQuotationRepository _requestForQuotationRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IIndentRepository _indentRepository;
        private static ILog log = LogManager.GetLogger(typeof(RequestForQuotationController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public RequestForQuotationController()
        {
            _requestForQuotationRepository = new RequestForQuotationRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            _indentRepository = new IndentRepository();
        }
        /// <summary>
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="unitRepository"></param>
        public RequestForQuotationController(IRequestForQuotationRepository requestForQuotationRepository)
        {
            _requestForQuotationRepository = requestForQuotationRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: RequestForQuotation
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _requestForQuotationRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
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

        #region Bind dropdowns Location Name 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        #endregion

        #region Bind dropdowns Location Master          
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

        #region Bind dropdowns Indent
        public void BindIndentDropDown(string type = null)
        {
            var result = _purchaseOrderRepository.GetIndentListForDropdown(type);
            var resultList = new SelectList(result.ToList(), "ID", "IndentNo");
            ViewData["IndentDD"] = resultList;
        }
        #endregion

        #region Function for get item details by id,CurrencyId 
        public JsonResult GetIndentDescription(string id, string tempCurrencyId)
        {
            int Id = Convert.ToInt32(id);
            int CurrencyId = Convert.ToInt32(tempCurrencyId);
            var result = _indentRepository.GetItemDetailsById(Id, CurrencyId);
            return Json(result);
        }
        #endregion 

        #region Function for get item details 
        public JsonResult GetitemDetails(string id)
        {
            var itemId = Convert.ToInt32(id);
            var itemDetails = _requestForQuotationRepository.GetItemDetails(itemId);
            return Json(itemDetails);
        }
        #endregion 

        #region Insert function
        /// <summary>
        /// Rahul: Rendered the user to the add Request For Quotation form. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRequestForQuotation()
        {
            //return View();

            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindLocationName();
                BindCurrencyPrice();
                BindIndentDropDown();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(8);
                ViewData["DocumentNo"] = DocumentNumber;

                RequestForQuotationBO model = new RequestForQuotationBO();
                model.Date = DateTime.Today;
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
        public ActionResult AddRequestForQuotation(RequestForQuotationBO model, HttpPostedFileBase Signature)
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
                        model.CreatedByID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _requestForQuotationRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Request For Quotation inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Request For Quotation! Can not be inserted!');</script>";
                            BindCompany();
                            BindLocationName();
                            BindCurrencyPrice();
                            BindIndentDropDown();
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                            var DocumentNumber = objDocNo.GetDocumentNo(8);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "RequestForQuotation");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindLocationName();
                        BindCurrencyPrice();
                        BindIndentDropDown();
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                        var DocumentNumber = objDocNo.GetDocumentNo(8);
                        ViewData["DocumentNo"] = DocumentNumber;

                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                BindCompany();
                BindLocationName();
                BindCurrencyPrice();
                BindIndentDropDown();
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(8);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Rahul: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="RequestForQuotationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRequestForQuotation(int RequestForQuotationId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindCurrencyPrice();
                BindLocationName();

                RequestForQuotationBO model = _requestForQuotationRepository.GetDetailsForRFQView(RequestForQuotationId);

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
        public ActionResult EditRequestForQuotation(RequestForQuotationBO model, HttpPostedFileBase Signature)
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
                        model.LastModifiedByID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _requestForQuotationRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Request For Quotation updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindCompany();
                            BindCurrencyPrice();
                            BindLocationName();
                            RequestForQuotationBO model1 = _requestForQuotationRepository.GetDetailsForRFQView(model.RequestForQuotationId);

                            return View(model1);
                        }

                        return RedirectToAction("Index", "RequestForQuotation");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindCompany();
                        BindCurrencyPrice();
                        BindLocationName();
                        RequestForQuotationBO model1 = _requestForQuotationRepository.GetDetailsForRFQView(model.RequestForQuotationId);

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
                return RedirectToAction("Index", "RequestForQuotation");
            }
        }

        #endregion

        #region Function for uploading the signature 
        /// <summary>
        /// Date: 14 Oct 2022 
        /// Rahul: Upload Signature File items. 
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

        #region Request For Quotation View for Punch Quotation  
        /// <summary>
        /// Created By: Rahul 
        /// Created Date : 17-12-2022 
        /// Description: This method responsible for View of Request For Quotation details.  
        /// </summary>
        /// <param name="RequestForQuotationId"></param> 
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestForQuotationView(int ID)
        {
            Session["RequestForQuotationView"] = ID;
            if (Session[ApplicationSession.USERID] != null)
            {
                //Binding item grid.             
                RequestForQuotationBO model = _requestForQuotationRepository.GetDetailsForRFQView(ID);
                if (model.VendorIDs != null)
                {
                    string vId = model.VendorIDs;
                    string[] values = vId.Split(',');
                    model.vendorIdLength = values.Length;
                }
                return PartialView("_RFQPV", model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Insert function RFQ Supplier Details view 
        /// <summary>
        ///Rahul: Rendered the user to the edit RFQ Supplier Details View page with details of a perticular record.
        /// </summary>
        /// <param name="RequestForQuotationId"></param>
        /// <returns></returns>        
        [HttpGet]
        public ActionResult AddRFQSupplierDetails(int RequestForQuotationId, int VendorsID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindCompany();
                BindCurrencyPrice();
                BindLocationName();

                RequestForQuotationBO model = _requestForQuotationRepository.GetRFQbyId(RequestForQuotationId, VendorsID);

                model.VendorsID = VendorsID; //added 01-02-2023
                model.CompanyName = model.companyDetails[0].CompanyName;
                model.SupplierAddress = model.companyDetails[0].Address;
                model.IndianCurrencyValue = model.CurrencyPrice;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul:  Pass the data to the repository for RFQ Supplier Details view insertion that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRFQSupplierDetails(RFQ_VendorDetailsBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (ModelState.IsValid)
                    {
                        model.CreatedByID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _requestForQuotationRepository.InsertRFQSupplierDetails(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('RFQ Supplier Details inserted/updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                        }

                        return RedirectToAction("Index", "RequestForQuotation");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return RedirectToAction("Index", "RequestForQuotation");
        }
        #endregion

        #region Bind RFQview Company Name 
        public JsonResult BindCompanyName(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _requestForQuotationRepository.GetCompanyNameForRFQView(Id);
            return Json(result);
        }
        #endregion

        #region Fetch Item details vendor wise
        public JsonResult FetchItemDetailsVendorWise(string id, string RFQId)
        {
            int Id = 0, RFQ_Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            if (RFQId != null && RFQId != "")
                RFQ_Id = Convert.ToInt32(RFQId);
            var result = _requestForQuotationRepository.ItemDetailsVendorWise(RFQ_Id, Id);
            return Json(result);
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 20 Dec'22 
        /// Rahul: Delete the perticular record Purchase Order 
        /// </summary>
        /// <param name="RequestForQuotationId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRequestForQuotation(int RequestForQuotationId)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _requestForQuotationRepository.Delete(RequestForQuotationId, userID);
                TempData["Success"] = "<script>alert('Request For Quotation deleted successfully!');</script>";
                return RedirectToAction("Index", "RequestForQuotation");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region View RFQ Supplier Details
        /// <summary>
        /// Created By: Farheen
        /// Created Date : 05-02-2023
        /// Description: This method responsible for View of RFQ Supplier details and for generating PO.
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        public ActionResult ViewRFQSupplierDetails(int ID, int RfqID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            RFQ_VendorDetailsBO model = _requestForQuotationRepository.ItemDetailsVendorWise(RfqID, 0, ID);
            if (model == null)
            {
                TempData["Success"] = "<script>alert('Quotation for this vendor is not punched yet! Please try again');</script>";
                return RedirectToAction("Index", "RequestForQuotation");
            }
            else
                return View(model);

        }
        #endregion

        #region Method for Generating PO
        /// <summary>
        /// Created By: Farheen
        /// Created Date : 05-02-2023
        /// Description: This method for generating Purchase order.
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GeneratePO(RFQ_VendorDetailsBO model)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            GenerateDocumentNo();
            BindTermsAndCondition();
            model.PODate = DateTime.Today;

            var json = new JavaScriptSerializer();
            var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);
            List<RFQ_Vendor_ItemDetailsBO> resultList = new List<RFQ_Vendor_ItemDetailsBO>();
            foreach (var item in data)
            {
                RFQ_Vendor_ItemDetailsBO objItemDetails = new RFQ_Vendor_ItemDetailsBO();
                objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                objItemDetails.CurrencyName = item.ElementAt(6).Value.ToString();
                objItemDetails.ItemTaxValue = item.ElementAt(7).Value.ToString();
                objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(8).Value);

                resultList.Add(objItemDetails);
            }
            model.rfqVendorItemDetails = resultList;
            return View(model);
        }


        /// <summary>
        /// Created By: Farheen
        /// Created Date: 05 Feb'23
        /// Description: This function is for saving PO data
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Signature"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPurchaseOrder(RFQ_VendorDetailsBO model, HttpPostedFileBase Signature)
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
                        model.CreatedByID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _requestForQuotationRepository.InsertPO(model);
                        if (response.Status)
                        {
                            TempData["Success"] = "<script>alert('Purchase Order generated successfully!');</script>";
                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Purchase Order! Can not be inserted!');</script>";
                            return RedirectToAction("Index", "RequestForQuotation");
                        }

                        return RedirectToAction("Index", "RequestForQuotation");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return RedirectToAction("Index", "RequestForQuotation");
        }


        public void GenerateDocumentNo()
        {
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=2 i.e. for generating the Purchase Order (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(2);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        public void BindTermsAndCondition()
        {
            var result = _purchaseOrderRepository.GetTermsAndConditionList();
            var resultList = new SelectList(result.ToList(), "TermsAndConditionID", "Terms");
            ViewData["TermsAndConditionID"] = resultList;
        }
        #endregion

        #region View Request For Quotation
        /// <summary>
        /// Created By: Rahul
        /// Created Date : 10-04-2023. 
        /// Description: This method responsible for View of Request For Quotation details. 
        /// </summary>
        /// <param name="RequestForQuotationId"></param> 
        /// <returns></returns>
        public ActionResult ViewRequestForQuotation(int RequestForQuotationId)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindCompany();
            BindCurrencyPrice();
            BindLocationName();
            Session["RequestForQuotationId"] = RequestForQuotationId;
            RequestForQuotationBO model = _requestForQuotationRepository.GetDetailsForRFQView(RequestForQuotationId);
            return View(model);

        }
        #endregion

        #region Export PDF RFQ Report
        /// <summary>
        /// Create by Maharshi on 03/05/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            StringBuilder sb = new StringBuilder();
            RequestForQuotationBO ResultList = _requestForQuotationRepository.GetDetailsForRFQView(Convert.ToInt32(Session["RequestForQuotationId"]));
            var CompanyNameList = _requestForQuotationRepository.GetCompanyNameForRFQView(Convert.ToInt32(Session["RequestForQuotationId"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + ResultList.Signature;
            string ReportName = "Request For Quotation";

            var CompanyName = "";
            foreach (var items in CompanyNameList)
            {
                CompanyName = items.CompanyName + "," + CompanyName;
            }

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>RFQ Number</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.RFQNO + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>RFQ Document Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.Date).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Delivery Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.DeliveryDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Indent Number</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + ResultList.IndentNumber + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Bidding Start Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.BiddingStartDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Bidding End Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.BiddingEndDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.LocationName + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier Name</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Delivery Details</th>");
            sb.Append("<td colspan='3' style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.DeliveryAddress + "</td>");
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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Unit</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Delivery Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>HSN Code</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            foreach (var item in ResultList.itemDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + Convert.ToDateTime(item.DeliveryDate).ToString("dd/MM/yyyy") + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.HSN_Code + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<br />");

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
                    string filename = "Request_For_Quotation_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
                var GeneratedDate = "Generated By: " + System.Web.HttpContext.Current.Session[ApplicationSession.USERNAME] + " On " + DateTime.Now;
                var generatedDateTable = new PdfPTable(1);
                generatedDateTable.DefaultCell.Border = 0;

                var generatedDateCell = new PdfPCell(new Phrase(GeneratedDate, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(generatedDateCell);
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

    }
}