using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Text;
using InVanWebApp_BO;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class StockMovementController : Controller
    {
        IStockMovementRepository _repository;

        #region Constructor Initialization.
        public StockMovementController()
        {
            _repository = new StockMovementRepository();
        }

        public StockMovementController(IStockMovementRepository stockMovementRepository)
        {
            _repository = stockMovementRepository;
        }

        #endregion
            
        #region Bind data in datatable/grid
        /// <summary>
        /// Created by: Yatri
        /// Created Date: 25 Nov'22
        /// Description: This function is for binding the grid
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //var model = _repository.GetAllTransfferedStock();
                StockMovementBO model = new StockMovementBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// to get stock details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStockMovementDate(DateTime fromDate, DateTime toDate)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            var stockDetails = _repository.GetAllTransfferedStock(fromDate, toDate);
            TempData["StockDetailsTemp"] = stockDetails;
            return Json(new { data = stockDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export pdf functionality.
        /// <summary>
        /// 25/11/2022 Yatri
        /// Export report to pdf
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public object ExprotAsPDF()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var stockDetails = _repository.GetAllTransfferedStock(fromDate,toDate);
            TempData["StockDetailsTemp"] = stockDetails;
            if (TempData["StockDetailsTemp"] == null)
            {
                return View("Index");
            }

            StringBuilder sb = new StringBuilder();
            List<StockMovementBO> stockReport = TempData["StockDetailsTemp"] as List<StockMovementBO>;

            if (stockReport.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            string ReportName = "Stock Movement Report";
            string name = "Vangi Foods";
            string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th Colspan='9' style='text-align:right;padding-right:-370px;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:-350px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>From Location Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>From Location Before Transfer Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Transfer Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Value Out</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Balance Quantity From Location</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Action</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>To Location Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>To Location Final Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Value In</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Unit Price</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in stockReport)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Date + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.FromLocationName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.FromLocation_BeforeTransferQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TransferQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ValueOut + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BalanceQty_FromLocation + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Action + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ToLocationName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ToLocation_FinalQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ValueIn + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.UnitPrice + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    pdfDoc.SetPageSize(new Rectangle(850f, 1100f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    //pdfDoc.NewPage();


                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_Stock_Movement_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        public void setBorder(PdfWriter writer, Document pdfDoc)
        {
            //---------------------------------------
            var content = writer.DirectContent;
            var pageBorderRect = new Rectangle(pdfDoc.PageSize);

            pageBorderRect.Left += pdfDoc.LeftMargin;
            pageBorderRect.Right -= pdfDoc.RightMargin;
            pageBorderRect.Top -= pdfDoc.TopMargin;
            pageBorderRect.Bottom += pdfDoc.BottomMargin;

            content.SetColorStroke(BaseColor.LIGHT_GRAY);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();

            //---------------------------------------
        }

        public class PageHeaderFooter : PdfPageEventHelper
        {
            private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK);

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                StockReportController stockReportController = new StockReportController();
                stockReportController.setBorder(writer, document);
                AddPageNumber(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                var text = writer.PageNumber.ToString();

                var numberTable = new PdfPTable(1);
                numberTable.DefaultCell.Border = 0;
                var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_MIDDLE,PaddingLeft = -150 };
                numberCell.Border = 0;
                numberTable.AddCell(numberCell);


                numberTable.TotalWidth = 50;
                numberTable.WriteSelectedRows(0, -1, document.Right - 300, document.Bottom + 20, writer.DirectContent);
            }
        }

        #endregion
    }

}