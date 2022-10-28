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

namespace InVanWebApp.Controllers
{
    public class StockReportController : Controller
    {
        IStockMasterRepository _StockMasterRepository;


        public StockReportController()
        {
            _StockMasterRepository = new StockMasterRepository();
        }

        public StockReportController(IStockMasterRepository stockMasterRepository)
        {
            _StockMasterRepository = stockMasterRepository;
        }

        // GET: StockReport
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetStock()
        {
            var stockDetails = _StockMasterRepository.GetAllStock();
            TempData["StockDetailsTemp"] = stockDetails;
            return Json(new { data = stockDetails }, JsonRequestBehavior.AllowGet);
        }

        [Obsolete]
        public ActionResult ExprotAsPDF()
        {
            StringBuilder sb = new StringBuilder();
            List<StockReportBO> stockReport = TempData["StockDetailsTemp"] as List<StockReportBO>;

            if (stockReport.Count < 0)
                return View("Index");

            sb.Append("<table style='text-align:center;padding: 5px;font-size:12px;border: 1px solid #000;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th Colspan='7' style='text-align:center;padding: 10px;border: 1px solid #000;font-size:15px;'>Stock Report</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='text-align:center;padding: 5px;'>");
            //sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;width:50px;'>ID</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Item Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Stock Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Inventory Value</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>ReOrder Level</th>");
            sb.Append("<th style='text-align:center;padding: 10px;border: 1px solid #000;'>Item ReOrder Quantity</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in stockReport)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;border: 1px solid #000;'>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.ID + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.StockQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.InventoryValue + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.ReOrderLevel + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 1px solid #000;'>" + item.ItemReOrderQuantity + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();
                    //pdfDoc.NewPage();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    return File(memoryStream.ToArray(), "application/pdf", "Stock_Report" + ".pdf");
                }
            }
        }
    }
}