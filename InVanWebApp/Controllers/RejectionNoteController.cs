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
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.Text;
using System.IO;

namespace InVanWebApp.Controllers
{
    public class RejectionNoteController : Controller
    {
        private IRejectionNoteRepository _RejectionNoteRepository;
        private IInwardQCSortingRepository _QCRepository;
        private IGRNRepository _GRNrepository;
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Constructor(s)

        /// <summary>
        /// Raj: Constructor without parameters
        /// </summary>
        public RejectionNoteController()
        {
            _RejectionNoteRepository = new RejectionNoteRepository();
            _QCRepository = new InwardQCSortingRepository();
            _GRNrepository = new GRNRepository();
        }

        /// <summary>
        /// Raj: Constructor With Parameters for initalizing objects.
        /// </summary>
        /// <param name="RejectionNoteRepository"></param>
        public RejectionNoteController(IRejectionNoteRepository RejectionNoteRepository, IInwardQCSortingRepository QCRepository)
        {
            _RejectionNoteRepository = RejectionNoteRepository;
            _QCRepository = QCRepository;
            _GRNrepository = new GRNRepository();
        }
        #endregion

        #region Bind Grid
        // GET: RejectionNote 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _RejectionNoteRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert Rejection Note
        /// <summary>
        /// Raj: Render View for the Add Rejection Note Details. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRejectionNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindInwardNoNumber();
                BindPreProductionQCNumber();     //Rahul 18 Apr 23.
                BindInwardNumber();  
                RejectionNoteBO model = new RejectionNoteBO();
                model.NoteDate = DateTime.UtcNow;
                //==========Document number for Rejection note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                var DocumentNumber = objDocNo.GetDocumentNo(5);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Raj: Bind All Inward QC behalf of Inward Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetInwardQCById(int id)
        {
            var inwardItems = _QCRepository.GetInwDetailsById(id);
            return Json(inwardItems);
        }
        #endregion

        #region Get Inward number details
        public JsonResult BindInwardDetails(string id)
        {
            int IQCId = 0;
            if (id != "" && id != null)
                IQCId = Convert.ToInt32(id);

            var result = _RejectionNoteRepository.GetInwardDetailsById(IQCId);
            return Json(result);
        }

        #endregion

        #region Bind all Production material note and it's details including which item Pre Production
        /// <summary>
        /// Rahul 18 Apr 23.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PPINote_Id"></param>
        /// <returns></returns>
        public JsonResult ProdIndent_NoDeatils(string id, string PPINote_Id = null)
        {
            int PPQCId = 0;
            int PPNote_Id = 0;
            if (id != "" && id != null)
                PPQCId = Convert.ToInt32(id);
            if (PPINote_Id != "" && PPINote_Id != null)
                PPNote_Id = Convert.ToInt32(PPINote_Id);

            var result = _RejectionNoteRepository.GetProdIndent_NoDeatils(PPQCId, PPNote_Id);
            return Json(result);
        }
        #endregion

        #region Bind dropdown of Inward Number
        public void BindInwardNumber()
        {

            var result = _GRNrepository.GetInwardNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwardNumber"] = resultList;
        }
        /// <summary>
        /// Rahul: Bind dropdown of PreProduction QC Number
        /// 18 Apr 2023.
        /// </summary>
        public void BindPreProductionQCNumber()
        {
            var result = _RejectionNoteRepository.GetPreProductionQCNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "QCNumber");
            ViewData["PreProductionQCAndId"] = resultList;
        }

        //public void BindInwardNoNumber()
        //{
        //    var result = _QCRepository.GetInwNumberForDropdown();
        //    var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
        //    ViewData["InwNumberAndId"] = resultList;
        //}
        #endregion

        #region 
        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRejectionNote(RejectionNoteBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        response = _RejectionNoteRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Rejection Note insert successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Rejection Note! Can not be completed!');</script>";
                            //BindInwardNoNumber();
                            BindPreProductionQCNumber();    //Rahul 18 Apr 23.
                            BindInwardNumber(); 
                            model.NoteDate = DateTime.Today;
                            //==========Document number for Rejection note============//
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                            var DocumentNumber = objDocNo.GetDocumentNo(5);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "RejectionNote"); //

                    }
                    else
                    {
                        if (model.PONumber != null && model.PONumber != "")
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        }

                        //BindInwardNoNumber();
                        BindPreProductionQCNumber();    //Rahul 18 Apr 23.
                        BindInwardNumber(); 
                        model.NoteDate = DateTime.Today;
                        //==========Document number for Rejection note============//
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                        var DocumentNumber = objDocNo.GetDocumentNo(5);
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

                //BindInwardNoNumber();
                BindPreProductionQCNumber();    //Rahul 18 Apr 23.
                BindInwardNumber(); 
                model.NoteDate = DateTime.Today;
                //==========Document number for Rejection note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                var DocumentNumber = objDocNo.GetDocumentNo(5);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        //#region This method is for View the Rejection Note Details    
        ///// <summary>
        ///// Created By: Rahul
        ///// Created Date : 05-01-2023. 
        ///// Description: This method responsible for View of Rejection Note details. 
        ///// </summary>
        ///// <param name="InquiryID"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult ViewRejectionNote(int RejectionID)
        //{
        //    if (Session[ApplicationSession.USERID] != null)
        //    {
        //        //Binding item grid.             
        //        RejectionNoteBO model = _RejectionNoteRepository.GetRejectionNoteById(RejectionID);
        //        return View(model);
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");
        //}
        //#endregion

        #region This method is for View the Rejection Note Details    
        /// <summary>
        /// Created By: Rahul
        /// Created Date : 05-01-2023. 
        /// Description: This method responsible for View of Rejection Note details. 
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewRejectionNote(int RejectionID)
        {
            Session["RejectionNoteID"] = RejectionID;
            if (Session[ApplicationSession.USERID] != null)
            {
                //Binding item grid.             
                RejectionNoteBO model = _RejectionNoteRepository.GetRejectionNoteById(RejectionID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export PDF Outward
        /// <summary>
        /// Created by: Vedant
        /// Date: 21 June'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPdf()
        {
            StringBuilder sb = new StringBuilder();
            RejectionNoteBO RejectionNoteList = _RejectionNoteRepository.GetRejectionNoteById(Convert.ToInt32(Session["RejectionNoteID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strAuthorizedSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + Signature;

            string ReportName = "Rejection Note";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Rejection Note Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.RejectionNoteNo + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Note Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.NoteDate + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>QC Type</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.QCType + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward QC Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.InwardQCNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward Note Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.InwardNoteNumber + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.PONumber + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward Note Number</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.InwardNoteNumber + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RejectionNoteList.SupplierName + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:87%;font-size:15px;'>Item Details</th>");
            //sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");


            sb.Append("<tr style='text-align:center;padding: 5px; font-family:Times New Roman;background-color:#C0DBEA'>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Description</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Price Per Unit (Rs)</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Currency</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.5%;font-size:10px;border: 0.01px black;'>Total Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.5%;font-size:10px;border: 0.01px black;'>Inward Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.5%;font-size:10px;border: 0.01px black;'>Rejected Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.8%;font-size:10px;border: 0.01px black;'>Quantity Took for sorting</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.8%;font-size:10px;border: 0.01px black;'>Units</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Wastage</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Tax</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Remarks</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;

            foreach (var item in RejectionNoteList.itemDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + " " + item.CurrencyName + "</td>");
                //sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.TotalQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.InwardQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.RejectedQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.QuantityTookForSorting + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.WastageQuantityInPercentage + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemTaxValue + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Remarks + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
            sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            //sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signaure</th></tr>");
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
                    string filename = "Rejection_Note_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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

        #region PDF Helper Class
        public class PageHeaderFooter : PdfPageEventHelper
        {
            private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PurchaseOrderController purchaseOrderController = new PurchaseOrderController();
                purchaseOrderController.setBorder(writer, document);

                AddPageNumber(writer, document);
                //base.OnEndPage(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                //----------------Font Value for Header & PageHeaderFooter--------------------
                Font plainFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                //--------------------------------------------For Generated Date-----------------------------------------------------
                var GeneratedDate = "Generated By: " + System.Web.HttpContext.Current.Session[ApplicationSession.USERNAME] + " On " + DateTime.Now;
                //var GeneratedDate = "Generated: " + DateTime.Now;
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
                //base.OnStartPage(writer, document);
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

        #region Delete function
        /// <summary>
        /// Date: 18 Jan'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="RejectionID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteRejectionNote(int RejectionID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _RejectionNoteRepository.Delete(RejectionID, userID);
                TempData["Success"] = "<script>alert('Rejection Note deleted successfully!');</script>";
                return RedirectToAction("Index", "RejectionNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion



    }
}