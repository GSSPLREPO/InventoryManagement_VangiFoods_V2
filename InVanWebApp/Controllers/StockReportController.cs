using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace InVanWebApp.Controllers
{
    public class StockReportController : Controller
    {
        IStockMasterRepository _StockMasterRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockReportController));

        #region Initializing Cunstructor
        /// <summary>
        /// Raj: Constructor without parameter
        /// </summary>
        public StockReportController()
        {
            _StockMasterRepository = new StockMasterRepository();
        }

        public StockReportController(IStockMasterRepository stockMasterRepository)
        {
            _StockMasterRepository = stockMasterRepository;
        }
        #endregion
        // GET: StockReport
        
        #region Bind the grid
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Bind grid by fetvching the stock details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStock()
        {
            var stockDetails = _StockMasterRepository.GetAllStock();
            TempData["StockDetailsTemp"] = stockDetails;
            return Json(new { data = stockDetails }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Pdf export functionality
        /// <summary>
        /// 17/11/2022 Bhandavi
        /// Export report to pdf
        /// </summary>
        /// <returns></returns>
      //  [Obsolete]
        public ActionResult ExprotAsPDF()
        {
            var stockDetails = _StockMasterRepository.GetAllStock();
            TempData["StockDetailsTemp"] = stockDetails;
            if (TempData["StockDetailsTemp"] == null)
            {
                return View("Index");
            }

            StringBuilder sb = new StringBuilder();
            List<StockReportBO> stockReport = TempData["StockDetailsTemp"] as List<StockReportBO>;

            if (stockReport.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            string ReportName = "Stock Report";
            string name = "Vangi Foods";
            string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            sb.Append("<div style='padding:9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>"); /* font*/
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th Colspan='9' style='text-align:right;padding: 5px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            //sb.Append("<br/>");           
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price (Rs)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock Quantity (Kg)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Inventory Value (Rs)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Reorder</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Reorder Level</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Reorder Quantity</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in stockReport)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RowNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.StockQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InventoryValue + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Reorder + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReOrderLevel + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemReOrderQuantity + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    //pdfDoc.NewPage();

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_Stock_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        /// <summary>
        /// Setting border to pdf document
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pdfDoc"></param>
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

        #endregion
    }

    public class PageHeaderFooter : PdfPageEventHelper
    {
        private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            //-----------------------------------
            StockReportController stockReportController = new StockReportController();
            stockReportController.setBorder(writer, document);

            //------------------------------------
            AddPageNumber(writer, document);
        }

        private void AddPageNumber(PdfWriter writer, Document document)
        {
            var text = writer.PageNumber.ToString();

            var numberTable = new PdfPTable(1);
            numberTable.DefaultCell.Border = 0;
            var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_CENTER };
            numberCell.Border = 0;
            numberTable.AddCell(numberCell);



            numberTable.TotalWidth = 50;
            numberTable.WriteSelectedRows(0, -1, document.Right - 300, document.Bottom + 2, writer.DirectContent);
        }
    }
}