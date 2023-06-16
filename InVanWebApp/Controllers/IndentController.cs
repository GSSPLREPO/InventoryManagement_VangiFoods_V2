using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using System.Text;
using System.IO;

namespace InVanWebApp.Controllers
{
    public class IndentController : Controller
    {
        private IIndentRepository _repository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(IndentController));

        #region Initializing constructor
        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public IndentController()
        {
            _repository = new IndentRepository();
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public IndentController(IndentRepository indentRepository)
        {
            _repository = indentRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
        [HttpGet]
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

        #region Insert functionality of Indent
        [HttpGet]
        public ActionResult AddIndent()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindLocation();
                BindDesignations();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(9);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                IndentBO model = new IndentBO();
                model.IndentDate = DateTime.Today;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Create By:Farheen
        /// Dscription: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddIndent(IndentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Indent inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            BindUsers();
                            BindLocation();
                            BindDesignations();
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                            var DocumentNumber = objDocNo.GetDocumentNo(9);
                            ViewData["DocumentNo"] = DocumentNumber;

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            ViewData["itemListForDD"] = dd;

                            return View(model);
                        }

                        return RedirectToAction("Index", "Indent");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindLocation();
                        BindDesignations();
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                        var DocumentNumber = objDocNo.GetDocumentNo(9);
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

                BindUsers();
                BindLocation();
                BindDesignations();
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(9);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
        }


        #endregion

        #region Update Functions
        /// <summary>
        ///Create By: Farheen
        ///Description: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindLocation();
                BindDesignations();

                IndentBO model = _repository.GetById(ID);
                model.indent_Details = _repository.GetItemDetailsByIndentId(ID);
                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                string itemListForDD = "itemListForDD";

                if (model != null)
                {
                    var ItemCount = model.indent_Details.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        itemListForDD = "itemListForDD";
                        itemListForDD = itemListForDD + i;
                        dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.indent_Details[i].ItemId);
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
        public ActionResult EditIndent(IndentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Indent updated successfully!');</script>";

                        
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindUsers();
                            BindLocation();
                            BindDesignations();
                            IndentBO model1 = _repository.GetById(model.ID);
                            model1.indent_Details = _repository.GetItemDetailsByIndentId(model.ID);

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model1 != null)
                            {
                                var ItemCount = model1.indent_Details.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }

                        return RedirectToAction("Index", "Indent");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindLocation();
                        BindDesignations();
                        IndentBO model1 = _repository.GetById(model.ID);
                        model1.indent_Details = _repository.GetItemDetailsByIndentId(model.ID);

                        //Binding item grid with sell type item.
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        string itemListForDD = "itemListForDD";

                        if (model1 != null)
                        {
                            var ItemCount = model1.indent_Details.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                itemListForDD = "itemListForDD";
                                itemListForDD = itemListForDD + i;
                                dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
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
                return RedirectToAction("Index", "Indent");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Create By: Farheen
        /// Description: This function is for deleting the Indent and 
        /// Indent details by using Indent ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Indent deleted successfully!');</script>";
                return RedirectToAction("Index", "Indent");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dorpdowns
        public void BindUsers()
        {
            var UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);
            var result = _userDetailsRepository.GetAll(UserId);
            var resultList = new SelectList(result.ToList(), "EmployeeID", "EmployeeName");
            ViewData["EmployeeName"] = resultList;
        }
        public void BindLocation()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        public void BindDesignations()
        {
            var designations = _userDetailsRepository.GetDesignationForDropDown();
            var designationsList = new SelectList(designations.ToList(), "DesignationID", "DesignationName");
            ViewData["Designations"] = designationsList;

        }

        #endregion

        #region View Indent
        /// <summary>
        /// Created By: Yatri
        /// Created Date : 24-04-2023
        /// Description: This method responsible for View of Indent details.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ViewIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindUsers();
                //BindLocation();
                IndentBO model = _repository.GetItemDetailsByIndentById(ID);
                Session["ID"] = ID;
                TempData["IdentPDF"] = model;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }
        #endregion

        #region Export PDF ident Report
        /// <summary>
        /// Create by Maharshi on 03/05/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {

            StringBuilder sb = new StringBuilder();
            IndentBO ResultList = _repository.GetItemDetailsByIndentById(Convert.ToInt32(Session["ID"]));
            //var result = _purchaseOrderRepository.GetCompanyList(1);
            //var resultList = new SelectList(result.ToList(), "VendorsID", "CompanyName");
            //List<PurchaseOrderBO> companyName = Session["CompanyName"] as List<PurchaseOrderBO>;
            //if (purchaseOrderList.Count <= 0)
            //    return View("Index");


            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + ResultList.Signature;
            string ReportName = "Ident";

            //string PODate = Convert.ToDateTime(ResultList.PODate).ToString("dd/MM/yyyy") + " ";
            //string DeliveryDate = Convert.ToDateTime(ResultList.DeliveryDate).ToString("dd/MM/yyyy") + " ";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Indent Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.IndentNo + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Ident Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.IndentDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Ident By</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + ResultList.UserName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + ResultList.LocationName + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Remarks</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.Description + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Bidding End Date</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(ResultList.BiddingEndDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");
            //sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.LocationName + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier Name</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.VendorIDs + "</td>");
            //foreach (var item in resultList.Items)
            //{
            //    sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" +  + "</td>");

            //}
            //sb.Append("</tr>");

            //sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Delivery Details</th>");
            //sb.Append("<td colspan='3' style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ResultList.DeliveryAddress + "</td>");
            //sb.Append("</tr>");

            //sb.Append("</thead>");
            //sb.Append("</table>");


            //sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            //sb.Append("<thead>");

            //sb.Append("<tr>");
            //sb.Append("<th style='text-align:center; font-family:Times New Roman;width:3%;font-size:15px;'>Item Details</th>");
            //sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody></tbody></table>");
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

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item </th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Item Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Units</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>HSN Code</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            foreach (var item in ResultList.indent_Details)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.RequiredQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                //sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + Convert.ToDateTime(item.DeliveryDate).ToString("dd/MM/yyyy") + "</td>");
                //sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.HSN_Code + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            //sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            //sb.Append("<thead>");
            //sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            //sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            //sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");

            //sb.Append("</thead>");
            //sb.Append("</table>");
            //sb.Append("<br />");




            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4);
                    //pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);


                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Ident_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
                IndentController indentController = new IndentController();
                indentController.setBorder(writer, document);

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

                //--------------------------------------------For user Name-----------------------------------------------------
                /*var UserName = "Generated: " + DateTime.Now;
                var UserNameTable = new PdfPTable(1);
                UserNameTable.DefaultCell.Border = 0;

                var UserNameCell = new PdfPCell(new Phrase(UserName, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(UserNameCell);
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 210, document.Bottom - 5, writer.DirectContent);
                */
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


        //#region View Indent
        ///// <summary>
        ///// Created By: Yatri
        ///// Created Date : 24-04-2023
        ///// Description: This method responsible for View of Indent details.
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>

        //[HttpGet]
        //public ActionResult ViewIndent(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] != null)
        //    {
        //        //BindUsers();
        //        //BindLocation();
        //        IndentBO model = _repository.GetItemDetailsByIndentById(ID);
        //        return View(model);
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");

        //}
        //#endregion

    }
}