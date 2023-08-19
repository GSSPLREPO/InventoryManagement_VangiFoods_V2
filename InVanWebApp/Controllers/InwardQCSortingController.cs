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
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace InVanWebApp.Controllers
{
    public class InwardQCSortingController : Controller
    {
        private IInwardQCSortingRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(InwardQCSortingController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public InwardQCSortingController()
        {
            _repository = new InwardQCSortingRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public InwardQCSortingController(InwardQCSortingRepository inwardQCSortingRepository)
        {
            _repository = inwardQCSortingRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
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

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add inward QC sorting.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInwardQC()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindInwardNoNumber();

                InwardQCBO model = new InwardQCBO();
                model.InwardQCDate = DateTime.Today;
                //==========Document number for Inward note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=4 i.e. for generating the Inward QC sorting (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(4);
                ViewData["DocumentNo"] = DocumentNumber;

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
        public ActionResult AddInwardQC(InwardQCBO model)
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
                            TempData["Success"] = "<script>alert('Inward QC done successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Inward sorting! Can not be completed!');</script>";
                            BindInwardNoNumber();
                            model.InwardQCDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(4);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "InwardQCSorting");

                    }
                    else
                    {
                        //TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindInwardNoNumber();
                        model.InwardQCDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(4);
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

                BindInwardNoNumber();
                model.InwardQCDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(4);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region Bind dropdown of Inward Number
        public void BindInwardNoNumber()
        {
            var result = _repository.GetInwNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwNumberAndId"] = resultList;
        }
        #endregion

        #region Bind all Inward note and it's details including which item inward 
        public JsonResult BindInwDetails(string id, string InwNote_Id = null)
        {
            int InwdQCId = 0;
            int InwdNote_Id = 0;
            if (id != "" && id != null)
                InwdQCId = Convert.ToInt32(id);
            if (InwNote_Id != "" && InwNote_Id != null)
                InwdNote_Id = Convert.ToInt32(InwNote_Id);

            var result = _repository.GetInwDetailsById(InwdQCId, InwdNote_Id);
            TempData["ItemDetail"] = result; 
            return Json(result);
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 21 Oct'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteInwardQC(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Inward QC deleted successfully!');</script>";
                return RedirectToAction("Index", "InwardQCSorting");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Inward note
        [HttpGet]
        public ActionResult ViewInwardQC(int ID)
        {
            Session["InwardQC_Id"] = ID;    
            if (Session[ApplicationSession.USERID] != null)
            {
                InwardQCBO model = _repository.GetById(ID);
               return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export Pdf Inward QC Report
        /// <summary>
        /// Created By: Vedant Parikh
        /// Date: 22 June'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPdf()
        {
            StringBuilder sb = new StringBuilder();
            InwardQCBO inwardQCList = _repository.GetById(Convert.ToInt32(Session["InwardQC_Id"]));
            List<InwardNoteBO> ItemDetail = TempData["ItemDetail"] as List<InwardNoteBO>;

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";

            string ReportName = "Inward QC (Check-1)";
            //string strAuthorizedSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + inwardQCList.Signature;

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Inward QC No</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + inwardQCList.InwardQCNo + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Inward QC Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(inwardQCList.InwardQCDate).ToString("dd-MM-yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward Number</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + inwardQCList.InwardNumber + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'></th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'></td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");
            //sb.Append("<br/>"); //Rahul removed 'sb.Append("<br/>");' not in use 19-08-23.
            ///Rahul added 'table' tag for 'Item Details' start 19-08-23. 
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:87%;font-size:15px;'>Item Details</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");
            ///Rahul added 'table' tag for 'Item Details' end 19-08-23. 
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr style='text-align:center;padding: 5px; font-family:Times New Roman;background-color:#C0DBEA'>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Inward Total Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.5%;font-size:10px;border: 0.01px black;'>Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1.8%;font-size:10px;border: 0.01px black;'>Quantity Took for Sorting</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.8%;font-size:10px;border: 0.01px black;'>Rejected Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Remarks</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;

            foreach (var item in ItemDetail)
            {
                sb.Append("<tr style='text-align:center;'>");
                //sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.InwardQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.QuantityTookForSorting + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.RejectedQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Remarks + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
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
                    string filename = "InwardQC_(Check-1)_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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

    }
}