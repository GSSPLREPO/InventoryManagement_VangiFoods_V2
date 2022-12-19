using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class StockReportController : Controller
    {
        IStockMasterRepository _StockMasterRepository;

        #region Initializing the constructors
        public StockReportController()
        {
            _StockMasterRepository = new StockMasterRepository();
        }

        public StockReportController(IStockMasterRepository stockMasterRepository)
        {
            _StockMasterRepository = stockMasterRepository;
        }
        #endregion

        #region Binding the stock data

        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
       
        /// <summary>
        /// to get stock details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStock()
        {
            var stockDetails = _StockMasterRepository.GetAllStock();
            TempData["StockDetailsTemp"] = stockDetails;
            return Json(new { data = stockDetails }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [Obsolete]
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
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th Colspan='9' style='text-align:right;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");           
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");
            
            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price (Rs)</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock Quantity (KG)</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Inventory Value (Rs)</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Reorder</th>"); 
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Reorder Level (KG)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:11%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Reorder Quantity (KG)</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            stockReport.Count();
            //stockReport.r
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
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    
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
        /// setting border to pdf document
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

        public void ExportAsExcel()
        {

            GridView gv = new GridView();
            IEnumerable<StockReportBO> stockReports = _StockMasterRepository.GetAllStock();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Reorder");
            dt.Columns.Add("Item code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Unit Price (Rs)");
            dt.Columns.Add("Stock Quantity (KG)");
            dt.Columns.Add("Inventory Value (Rs)");
            dt.Columns.Add("Reorder Level (KG)");
            dt.Columns.Add("Item Reorder Quantity (KG)");

            foreach (StockReportBO st in stockReports)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.RowNumber.ToString();
                dr["Reorder"] = st.Reorder.ToString();
                dr["Item code"] = st.Item_Code.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Item Unit Price (Rs)"] = st.ItemUnitPrice.ToString();
                dr["Stock Quantity (KG)"] = st.StockQuantity.ToString();
                dr["Inventory Value (Rs)"] = st.InventoryValue.ToString();
                dr["Reorder Level (KG)"] = st.ReOrderLevel.ToString();
                dr["Item Reorder Quantity (KG)"] = st.ItemReOrderQuantity.ToString();
                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Stock_Movement_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Stock Movement Report";/* The Stock Movement Report name are given here  */
            string name = "Vangi Foods";/* The Vangi Foods are given here  */
            string address = "Sr No 673, Opp Surya Gate, Gana Rd, Karamsad, Gujarat 388325";/* The Address are given here  */




            String content1 = "<table>" + "<tr><td><td colspan='3' rowspan='5'> <img height='120' width='120' src='" + strPath + "'/></td></td>" +
                "<tr><td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></td></tr></tr>" +
                "<tr><td><td><td colspan='2'>" + name + "</td></td></tr>" +
                "<tr><td><td colspan='4'>" + address + "</td></td></tr>" + "</table>"
                + "<table><tr align='center'><td>" + sw.ToString() + "</tr></td></table>";


            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(content1);
            gv.GridLines = GridLines.None;
            Response.Flush();
            Response.Clear();
            Response.End();
        }

    }

    public class PageHeaderFooter : PdfPageEventHelper
    {
        private readonly Font _pageNumberFont = new Font(Font.NORMAL, 9f, Font.NORMAL, BaseColor.BLACK);

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
            var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_CENTER};
            numberCell.Border = 0;
            numberTable.AddCell(numberCell);

            

            numberTable.TotalWidth = 50;
            numberTable.WriteSelectedRows(0, -1, document.Right - 300, document.Bottom + 2, writer.DirectContent);
        }
    }



}