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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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
            string Fromdate = "From Date:";
            string Todate = "To Date:";
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Stock Movement Report";
            string name = "Vangi Foods";
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-440px;padding-bottom:-290px;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            //sb.Append("<tr >");
            //sb.Append("<th Colspan='9' style='text-align:right;padding-right:-370px;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            //sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='9' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:-350px;'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Date of Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit (RS)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>From Location Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>From Location Before Transfer Quantity (Kg)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:13px;border: 0.05px  #e2e9f3;'>Transfer Quantity (KG)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Value Out (Rs)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Balance Quantity From Location (KG)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Action</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>To Location Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>To Location Final Quantity (KG)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Value In (Rs)</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in stockReport)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardDateOfItem + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.UnitPrice + "</td>");
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
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.UnitPrice + "</td>");
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
                numberTable.WriteSelectedRows(0, -1, document.Right - 300, document.Bottom + 4, writer.DirectContent);
            }
        }

        #endregion

        #region Excel Functionality
        public void ExportAsExcel()
        {

            GridView gv = new GridView();
            List<StockMovementBO> stockMovements = _repository.GetAllTransfferedStock(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["toDate"]));
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Inward Date of Item");
            dt.Columns.Add("Price Per Unit (Rs)");
            dt.Columns.Add("Date");
            dt.Columns.Add("From Location");
            dt.Columns.Add("From Location Before Transfer Quantity (KG)");
            dt.Columns.Add("Transfer Quantity (KG)");
            dt.Columns.Add("Value Out (Rs)");
            dt.Columns.Add("Balance Quantity From Location (KG)");
            dt.Columns.Add("Action");
            dt.Columns.Add("To Location");
            dt.Columns.Add("To Location Final Quantity (KG)");
            dt.Columns.Add("Value In (Rs)");
            
            foreach (StockMovementBO st in stockMovements)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Name"] = st.Item_Name.ToString();
                dr["Inward Date of Item"] = st.InwardDateOfItem.ToString();
                dr["Price Per Unit (Rs)"] = st.UnitPrice.ToString();
                dr["Date"] = st.Date.ToString();
                dr["From Location"] = st.FromLocationName.ToString();
                dr["From Location Before Transfer Quantity (KG)"] = st.FromLocation_BeforeTransferQty.ToString();
                dr["Transfer Quantity (KG)"] = st.TransferQuantity.ToString();
                dr["Value Out (Rs)"] = st.ValueOut.ToString();
                dr["Balance Quantity From Location (KG)"] = st.BalanceQty_FromLocation.ToString();
                dr["Action"] = st.Action.ToString();
                dr["To Location"] = st.ToLocationName.ToString();
                dr["To Location Final Quantity (KG)"] = st.ToLocation_FinalQty.ToString();
                dr["Value In (Rs)"] = st.ValueIn.ToString();
                
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
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = "Vangi Foods";/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td><td colspan='3' rowspan='5'> <img height='150' width='150' src='" + strPath + "'/></td></td>" +
                "<tr><td><td><td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></td></td></td></tr></tr>" +
                "<tr><td><td><td><td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></td></td></td></tr>" +
                "<tr><td><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></td></tr>" +
                "<tr><tr><td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td><td><td><td><td><td><td><td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td></td></td></td></td></td></td></td></td></td>"
                + "</td></tr>" + "</table>"
                + "<table><tr align='center'><td>" + sw.ToString() + "</tr></td></table>";


            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(content1);
            gv.GridLines = GridLines.None;
            Response.Flush();
            Response.Clear();
            Response.End();
        }
        #endregion
    }

}