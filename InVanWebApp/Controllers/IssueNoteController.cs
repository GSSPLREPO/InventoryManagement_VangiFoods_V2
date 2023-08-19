using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;
using InVanWebApp_BO;
using System.Text;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace InVanWebApp.Controllers
{
    public class IssueNoteController : Controller
    {
        private IIssueNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IStockAdjustmentRepository _stockAdjustmentRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 24 Jan'23
        /// </summary>
        public IssueNoteController()
        {
            _repository = new IssueNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _stockAdjustmentRepository = new StockAdjustmentRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 24 Jan'23
        /// </summary>
        /// <param name="stockAdjustmentRepository"></param>
        public IssueNoteController(IIssueNoteRepository issueNoteRepository)
        {
            _repository = issueNoteRepository;
        }
        #endregion

        #region Bind Grid
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _repository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add Stock adjustment note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddIssueNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindUserName();

                IssueNoteBO model = new IssueNoteBO();
                model.IssueNoteDate = DateTime.Today;
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
        public ActionResult AddIssueNote(IssueNoteBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        if (model.txtItemDetails == null || model.txtItemDetails == "[]]")
                        {
                            TempData["Success"] = "<script>alert('No item is issued! Material issue note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindUserName();

                            model.IssueNoteDate = DateTime.Today;
                            return View(model);
                        }
                        else
                        {
                            response = _repository.Insert(model);
                            if (response.Status)
                                TempData["Success"] = "<script>alert('Material issue note is created successfully!');</script>";
                            else
                            {
                                TempData["Success"] = "<script>alert('Error! Material issue note cannot be created!');</script>";
                                BindLocationName();
                                GenerateDocumentNo();
                                BindUserName();

                                model.IssueNoteDate = DateTime.Today;
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index", "IssueNote");
                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindUserName();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.IssueNoteDate = DateTime.Today;
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

                BindLocationName();
                GenerateDocumentNo();
                BindUserName();

                model.IssueNoteDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 24 Jan'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteIssueNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Issue note deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "IssueNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Issue Note
        [HttpGet]
        public ActionResult ViewIssueNote(int ID)
        {
            Session["MaterialIssueNoteID"] = ID;
            if (Session[ApplicationSession.USERID] != null)
            {
                IssueNoteBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export Pdf Material Issue Note Report
        /// <summary>
        /// Created By: Vedant Parikh
        /// Date: 22 June'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPdf()
        {
            StringBuilder sb = new StringBuilder();
            IssueNoteBO issueNoteList = _repository.GetById(Convert.ToInt32(Session["MaterialIssueNoteID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";

            string ReportName = "Material Issue Note";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Issue Note Number</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.IssueNoteNo + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Issue Note Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.IssueNoteDate + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Issue By</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.IssueByName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Purpose</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.Purpose + "</td>");
            if (issueNoteList.Purpose == "Production")
            {
                sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Work Order Number</th>");
                sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.WorkOrderNumber + "</td>");
            }
            else if (issueNoteList.Purpose == "QC")
            {
                sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>QC Number</th>");
                sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.QCNumber + "</td>");
            }
            else if (issueNoteList.Purpose == "Dispatch")
            {
                sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Sales Order Number</th>");
                sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.SONumber + "</td>");
            }
            else if (issueNoteList.Purpose == "Others")
            {
                sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Other Document Number</th>");
                sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.OtherPurpose + "</td>");
            }
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Location Name</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.LocationName + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Remarks</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + issueNoteList.Remarks + "</td>");
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

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>Sr. No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.8%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5.5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Price Per Unit (Rs)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.4%;font-size:10px;border: 0.01px black;'>Available Stock</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.4%;font-size:10px;border: 0.01px black;'>Requested Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.4%;font-size:10px;border: 0.01px black;'>Issued Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.4%;font-size:10px;border: 0.01px black;'>Final Stock</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.8%;font-size:10px;border: 0.01px black;'>UOM</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.8%;font-size:10px;border: 0.01px black;'>Comments</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;

            foreach (var item in issueNoteList.IssueNoteDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + " " + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.AvailableStockBeforeIssue + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.QuantityRequested + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.QuantityIssued + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.StockAfterIssuing + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Description + "</td>");
                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
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
                    string filename = "Material_Issue_Note_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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

        #region PDF Helper Class
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

        #region Bind dropdowns 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }
        public void BindUserName()
        {
            var result = _repository.GetUserNameList();
            var resultList = new SelectList(result.ToList(), "UserId", "Username");
            ViewData["UserList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=13 i.e. for generating the Issue Note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(13);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        public JsonResult GetItemList(string id)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);

            var result = _stockAdjustmentRepository.GetItemListByLocationId(Location_Id);
            return Json(result);
        }
        #endregion
    }
}