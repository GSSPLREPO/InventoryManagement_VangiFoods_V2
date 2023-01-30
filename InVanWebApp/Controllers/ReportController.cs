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
using log4net;
using System.Globalization;

namespace InVanWebApp.Controllers
{
    public class ReportController : Controller
    {
        private IReportRepository _repository;
        private ICompanyRepository _repositoryCompany;
        private IItemRepository _itemRepository;
        private ILocationRepository _locationRepository;

        private static ILog log = LogManager.GetLogger(typeof(ReportController));

        #region Initializing the constructors
        public ReportController()
        {
            _repository = new ReportRepository();
            _repositoryCompany = new CompanyRepository();
            _itemRepository = new ItemRepository();
            _locationRepository = new LocationRepository();
        }

        public ReportController(IReportRepository reportRepository)
        {
            _repository = reportRepository;
        }
        #endregion

        #region  PO report

        #region Binding the PO report data 

        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model1 = _repositoryCompany.GetAll();
                var dd = new SelectList(model1.ToList(), "ID", "CompanyName");
                ViewData["Vendors"] = dd;
                PurchaseOrderBO model = new PurchaseOrderBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth Purohit on 30-12-2022
        /// Calling method for PO Report Data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPOReportData(DateTime fromDate, DateTime toDate, string status, string vendor)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            var VendorId = Convert.ToInt32(vendor);
            Session["Status"] = status;
            Session["Vendor"] = vendor;
            var poReport = _repository.getPOReportData(fromDate, toDate, status, VendorId);
            return Json(new { data = poReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF PO Order
        /// <summary>
        /// Create by Maharshi on 02/01/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDF()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var status = Session["Status"].ToString();
            var VendorId = Convert.ToInt32(Session["Vendor"]);
            var POReportDetails = _repository.getPOReportData(fromDate, toDate, status, VendorId);
            TempData["POReportDataTemp"] = POReportDetails;
            if (TempData["POReportDataTemp"] == null)
            {
                return View("Index");
            }

            StringBuilder sb = new StringBuilder();
            List<PurchaseOrderBO> purchaseOrders = TempData["POReportDataTemp"] as List<PurchaseOrderBO>;

            if (purchaseOrders.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "PO Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:left;padding-right:20px;padding-bottom:-290px;font-size:11px;'>" + "From Date:" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;'>" + "To Date:" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Indent Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Status</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Vendor Name</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            purchaseOrders.Count();
            //stockReport.r
            foreach (var item in purchaseOrders)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PurchaseOrderDate + "</td>");  //Rahul updated PurchaseOrderDate 06-01-2023. 
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.IndentNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PurchaseOrderStatus + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.CompanyName + "</td>");

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
                    string filename = "RPT_PO_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel PO Report
        public void ExportAsExcel()
        {
            var status = Session["Status"].ToString();
            var VendorId = Convert.ToInt32(Session["Vendor"]);
            GridView gv = new GridView();
            List<PurchaseOrderBO> purchaseOrders = _repository.getPOReportData(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["toDate"]), status, VendorId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("PO Date");
            dt.Columns.Add("PO Number");
            dt.Columns.Add("Indent Number");
            dt.Columns.Add("PO Status");
            dt.Columns.Add("Vendor Name");

            foreach (PurchaseOrderBO st in purchaseOrders)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["PO Date"] = st.PurchaseOrderDate.ToString();
                dr["PO Number"] = st.PONumber.ToString();
                dr["Indent Number"] = st.IndentNumber.ToString();
                dr["PO Status"] = st.PurchaseOrderStatus.ToString();
                dr["Vendor Name"] = st.CompanyName.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_PO_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "&nbsp &nbsp&nbsp &nbsp PO &nbsp Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region Raw material received report

        #region Binding the Raw Material Received Report Data

        public ActionResult RawMaterialIndex()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItemDropDown();
                BindLocationDropDown();
                PurchaseOrderBO model = new PurchaseOrderBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Develop By Siddharth Purohit on 02-01-2023
        /// Calling method for Get Raw Material Report Data
        /// </summary>
        /// <returns></returns>
        public JsonResult getRawMaterialReceivedData(DateTime fromDate, DateTime toDate, string item, string wearhouse)
        {
            var ItemId = Convert.ToInt32(item);
            var WearhouseId = Convert.ToInt32(wearhouse);

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            Session["WarehouseId"] = WearhouseId;

            var poReport = _repository.getRawMaterialReceivedData(fromDate, toDate, ItemId, WearhouseId);

            return Json(new { data = poReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Raw material received
        /// <summary>
        /// Create by Maharshi on 02/01/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult PDFRawMaterialReceivedExport()
        {


            var ItemId = Convert.ToInt32(Session["ItemId"]);
            var WearhouseId = Convert.ToInt32(Session["WarehouseId"]);
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);

            var RawMaterialReport = _repository.getRawMaterialReceivedData(fromDate, toDate, ItemId, WearhouseId);

            TempData["RawMaterialReport"] = RawMaterialReport;

            if (TempData["RawMaterialReport"] == null)
            {
                return RedirectToAction("RawMaterialIndex", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<GRN_BO> RawMaterialReceived = TempData["RawMaterialReport"] as List<GRN_BO>;

            if (RawMaterialReceived.Count < 0)
                return RedirectToAction("RawMaterialIndex", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            string ReportName = "Raw Material Received Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:left;padding-right:-20px;padding-bottom:-290px;font-size:11px;'>" + "From Date:" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;'>" + "To Date:" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>GRN No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>GRN Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Iteam Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Iteam Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit (RS)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Quantity Received (Kg)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Stored At Location</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            RawMaterialReceived.Count();
            //stockReport.r
            foreach (var item in RawMaterialReceived)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.GRNCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.GRN_Date + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReceivedQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.LocationName + "</td>");
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
                    string filename = "RPT_Raw_Material_Received_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Raw material received Report
        public void ExportRawMaterialReceivedAsExcel()
        {
            var ItemId = Convert.ToInt32(Session["ItemId"]);
            var WearhouseId = Convert.ToInt32(Session["WarehouseId"]);
            GridView gv = new GridView();
            List<GRN_BO> GRN_BO = _repository.getRawMaterialReceivedData(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["toDate"]), ItemId, WearhouseId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("GRN No");
            dt.Columns.Add("GRN Date");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Price Per Unit (RS)");
            dt.Columns.Add("Quantity Received (Kg)");
            dt.Columns.Add("Item Stored At Location");

            foreach (GRN_BO st in GRN_BO)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["GRN No"] = st.GRNCode.ToString();
                dr["GRN Date"] = st.GRN_Date.ToString();
                dr["Item Code"] = st.ItemCode.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Price Per Unit (RS)"] = st.ItemUnitPrice.ToString();
                dr["Quantity Received (Kg)"] = st.ReceivedQty.ToString();
                dr["Item Stored At Location"] = st.LocationName.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Raw_Material_Received_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Raw Material Received Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></td></tr></tr>" +
                "<tr><td><td><td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></td></td></tr>" +
                "<tr><td><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></td></tr>" +
                "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td><td></td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td></td>"
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

        #endregion

        #region  Rejection report

        #region Binding the Rejection note report data 

        public ActionResult RejectionNoteReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                RejectionNoteBO model = new RejectionNoteBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Farheen on 10 Jan'23
        /// Calling method for Rejection note Report Data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRejectionNoteReportData(DateTime fromDate, DateTime toDate)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            var rejectionNoteReport = _repository.getRejectionReportData(fromDate, toDate);
            return Json(new { data = rejectionNoteReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Rejection Note
        /// <summary>
        /// Create by Maharshi on 02/01/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForRejection()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);

            var RejectionReportDetails = _repository.getRejectionReportData(fromDate, toDate);
            TempData["RejectionReportDataTemp"] = RejectionReportDetails;
            if (TempData["RejectionReportDataTemp"] == null)
            {
                return RedirectToAction("RejectionNoteReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<RejectionNoteItemDetailsBO> resultList = TempData["RejectionReportDataTemp"] as List<RejectionNoteItemDetailsBO>;

            if (resultList.Count < 0)
                return RedirectToAction("RejectionNoteReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Rejection Note Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-70px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:-70px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejection Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward QC Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Shorted Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejected Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteDate + "</td>");  //Rahul updated PurchaseOrderDate 06-01-2023. 
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQCNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + " " + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TotalRecevingQuantiy + " " + " (" + item.ItemUnit + ")</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectedQuantity + " " + " (" + item.ItemUnit + ")</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ApprovedBy + "</td>");

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
                    string filename = "RPT_Rejection_Note_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel Rejection note Report
        public void ExportAsExcelForRejection()
        {
            GridView gv = new GridView();
            List<RejectionNoteItemDetailsBO> resultList = _repository.getRejectionReportData(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["toDate"]));
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Rejection Number");
            dt.Columns.Add("Inward QC Number");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Unit Price");
            dt.Columns.Add("Shorted Quantity");
            dt.Columns.Add("Rejected Quantity");
            dt.Columns.Add("Approved By");

            foreach (RejectionNoteItemDetailsBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Date"] = st.RejectionNoteDate.ToString();
                dr["Rejection Number"] = st.RejectionNoteNo.ToString();
                dr["Inward QC Number"] = st.InwardQCNumber.ToString();
                dr["Item Name"] = st.Item_Name.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Unit Price"] = st.ItemUnitPrice.ToString() + " " + st.CurrencyName.ToString();
                dr["Shorted Quantity"] = st.TotalRecevingQuantiy.ToString() + " (" + st.ItemUnit.ToString() + ")";
                dr["Rejected Quantity"] = st.RejectedQuantity.ToString() + " (" + st.ItemUnit.ToString() + ")";
                dr["Approved By"] = st.ApprovedBy.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Rejection_Note_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Rejection Note Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td></td><td></td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td></td><td></td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td></td><td></td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td></td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region  Finished Goods Dispatch report

        #region Binding the Finished Goods report data 

        public ActionResult FinishedGoodsDispatchReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DeliveryChallanBO model = new DeliveryChallanBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindLocationDropDown();
                BindItemDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Farheen on 12 Jan'23
        /// Calling method for Finished Goods Dispatch Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFinishedGoodsDispatchReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            //Session["LocationId"] = LocationId;
            Session["ItemId"] = ItemId;
            var rejectionNoteReport = _repository.getFinishedGoodsReportData(fromDate, toDate, ItemId);
            return Json(new { data = rejectionNoteReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Finished Goods Dispatch
        /// <summary>
        /// Created by: Farheen
        /// Creadted Date: 12 Jan'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForFinishedGoods()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var resultDetails = _repository.getFinishedGoodsReportData(fromDate, toDate, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("FinishedGoodsDispatchReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<DeliveryChallanItemDetailsBO> resultList = TempData["ReportDataTemp"] as List<DeliveryChallanItemDetailsBO>;

            if (resultList.Count < 0)
                return RedirectToAction("FinishedGoodsDispatchReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Finished Goods Dispatch Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Challan Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Challan Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>FG Stored At Location</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Dispatch Quantity</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DeliveryChallanNumber + "</td>");  //Rahul updated PurchaseOrderDate 06-01-2023. 
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.OutwardDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DeliveryAddress + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ApprovedBy + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + " " + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DispatchQuantity + " " + "(" + item.ItemUnit + ")" + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ApprovedBy + "</td>");

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
                    string filename = "Rpt_FinishedGoods_Dispatch_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel Finished Goods Dispatch
        public void ExportAsExcelForFinishedGoods()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<DeliveryChallanItemDetailsBO> resultList = _repository.getFinishedGoodsReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Delivery Challan Number");
            dt.Columns.Add("Delivery Challan Date");
            dt.Columns.Add("FG Stored At Location");
            dt.Columns.Add("ApprovedBy");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item");
            dt.Columns.Add("Price Per Unit");
            dt.Columns.Add("Dispatch Quantity");
            //dt.Columns.Add("Approved By");

            foreach (DeliveryChallanItemDetailsBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Delivery Challan Number"] = st.DeliveryChallanNumber.ToString();
                dr["Delivery Challan Date"] = st.OutwardDate.ToString();
                dr["FG Stored At Location"] = st.DeliveryAddress.ToString();
                dr["ApprovedBy"] = st.ApprovedBy.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item"] = st.ItemName.ToString();
                dr["Price Per Unit"] = st.ItemUnitPrice.ToString() + " " + st.CurrencyName.ToString();
                dr["Dispatch Quantity"] = st.DispatchQuantity.ToString() + " " + "(" + st.ItemUnit.ToString() + ")";
                //dr["Approved By"] = st.ApprovedBy.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_FinishedGoods_Dispatch_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Finished Goods Dispatch Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td></td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td></td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td></td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region  Inventory FIFO report

        #region Binding the Inventory FIFO report data 

        public ActionResult InventoryFIFOReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                StockMasterBO model = new StockMasterBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindItemDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Farheen on 21 Jan'23
        /// Calling method for Inventory FIFO Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInventoryFIFOReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var ReportResult = _repository.getInventoryFIFOReportData(fromDate, toDate, ItemId);
            return Json(new { data = ReportResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Inventory FIFO
        /// <summary>
        /// Created by: Farheen
        /// Creadted Date: 21 Jan'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForInventoryFIFO()
        {
            string Fromdate = "From Date : ";
            string Todate = "To Date : ";

            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var resultDetails = _repository.getInventoryFIFOReportData(fromDate, toDate, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("InventoryFIFOReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<StockMasterBO> resultList = TempData["ReportDataTemp"] as List<StockMasterBO>;

            if (resultList.Count < 0)
                return RedirectToAction("InventoryFIFOReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Inventory Report (FIFO)";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='5' style='text-align:center'>");
            sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=3 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=3 style='text-align:right;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Date</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPriceWithCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.StockQuantity + " " + "(" + item.ItemUnit + ")" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardDate + "</td>");

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

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_InventoryFIFO_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel for Inventory FIFO
        public void ExportAsExcelForInventoryFIFO()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<StockMasterBO> resultList = _repository.getInventoryFIFOReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item");
            dt.Columns.Add("Price Per Unit");
            dt.Columns.Add("Stock Quantity");
            dt.Columns.Add("Inward Date");

            foreach (StockMasterBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item"] = st.ItemName.ToString();
                dr["Price Per Unit"] = st.ItemUnitPriceWithCurrency.ToString();
                dr["Stock Quantity"] = st.StockQuantity.ToString() + " " + "(" + st.ItemUnit.ToString() + ")";
                dr["Inward Date"] = st.InwardDate.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_InventoryFIFO_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Inventory Report (FIFO)";
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region  Total Inventory Cost Warehouse wise report

        #region Binding the Total Inventory Cost Warehouse wise report data 

        public ActionResult TotalInventoryCostReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                LocationWiseStockBO model = new LocationWiseStockBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindLocationDropDown();
                BindItemDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Farheen on 22 Jan'23
        /// Calling method for Total Inventory cost Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTotalInventoryCostReportData(DateTime fromDate, DateTime toDate, int LocationId, int ItemId)
        {
            List<LocationWiseStockBO> ReportResult = new List<LocationWiseStockBO>();

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            Session["LocationId"] = LocationId;
            ReportResult = _repository.getTotalInventoryCostData(fromDate, toDate, LocationId, ItemId);
            return Json(new { data = ReportResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Total Inventory Cost Warehouse wise report
        /// <summary>
        /// Created by: Farheen
        /// Creadted Date: 21 Jan'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForTotalInventoryCost()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var LocationId = Convert.ToInt32(Session["LocationId"]);

            var resultDetails = _repository.getTotalInventoryCostData(fromDate, toDate, LocationId, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("TotalInventoryCostReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<LocationWiseStockBO> resultList = TempData["ReportDataTemp"] as List<LocationWiseStockBO>;

            if (resultList.Count < 0)
                return RedirectToAction("TotalInventoryCostReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Total Inventory Cost Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string Fromdate = "From Date : ";
            string Todate = "To Date : ";
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='5' style='text-align:center'>");
            sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=3 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=3 style='text-align:right;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Total Inventory Value</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPriceWithCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Quantity + " " + "(" + item.ItemUnit + ")" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TotalInventoryValue + "</td>");

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

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_TotalInventoryCost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel for Total Inventory Cost Warehouse wise report
        public void ExportAsExcelForTotalInventoryCost()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var LocationId = Convert.ToInt32(Session["LocationId"]);

            List<LocationWiseStockBO> resultList = _repository.getTotalInventoryCostData(fromDate, toDate, LocationId, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item");
            dt.Columns.Add("Price Per Unit");
            dt.Columns.Add("Stock Quantity");
            dt.Columns.Add("Total Inventory Value");

            foreach (LocationWiseStockBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item"] = st.ItemName.ToString();
                dr["Price Per Unit"] = st.ItemUnitPriceWithCurrency.ToString();
                dr["Stock Quantity"] = st.Quantity.ToString() + " " + "(" + st.ItemUnit.ToString() + ")";
                dr["Total Inventory Value"] = st.TotalInventoryValue.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_TotalInventoryCost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Total Inventory Cost Report";
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td></td></td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td></td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region  Stock Reconciliation Report

        #region Binding the Stock reconciliation report data 

        public ActionResult StockReconciliationReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                StockAdjustmentBO model = new StockAdjustmentBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindLocationDropDown();
                BindItemDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Farheen on 22 Jan'23
        /// Calling method for Stock Reconciliation Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStockReconciliationReportData(DateTime fromDate, DateTime toDate, int LocationId, int ItemId)
        {
            List<StockAdjustmentDetailsBO> ReportResult = new List<StockAdjustmentDetailsBO>();

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            Session["LocationId"] = LocationId;
            ReportResult = _repository.getStockReconciliationData(fromDate, toDate, LocationId, ItemId);
            return Json(new { data = ReportResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Stock Reconciliation report
        /// <summary>
        /// Created by: Farheen
        /// Creadted Date: 21 Jan'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForStockReconciliation()
        {
            string Fromdate = "From Date : ";
            string Todate = "To Date : ";

            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var LocationId = Convert.ToInt32(Session["LocationId"]);

            var resultDetails = _repository.getStockReconciliationData(fromDate, toDate, LocationId, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("StockReconciliationReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<StockAdjustmentDetailsBO> resultList = TempData["ReportDataTemp"] as List<StockAdjustmentDetailsBO>;

            if (resultList.Count < 0)
                return RedirectToAction("StockReconciliationReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Stock Reconciliation Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='9' style='text-align:center'>");
            sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=5 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=5 style='text-align:right;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock Adjusted Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Price Per Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Recorded Stock</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Adjusted Stock</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Final Stock</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>UOM</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Remarks</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.StockAdjustedDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name+ "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPriceWithCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AvailableStock + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DifferenceInStock + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PhysicalStock+ "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Remarks + "</td>");

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

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_StockReconciliation_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel for Stock Reconciliation report
        public void ExportAsExcelForStockReconciliation()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var LocationId = Convert.ToInt32(Session["LocationId"]);

            List<StockAdjustmentDetailsBO> resultList = _repository.getStockReconciliationData(fromDate, toDate, LocationId, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Stock Adjusted Date");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item");
            dt.Columns.Add("Price Per Unit");
            dt.Columns.Add("Recorded Stock");
            dt.Columns.Add("Adjusted Stock");
            dt.Columns.Add("Final Stock");
            dt.Columns.Add("UOM");
            dt.Columns.Add("Remarks");

            foreach (StockAdjustmentDetailsBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Stock Adjusted Date"] = st.StockAdjustedDate.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item"] = st.Item_Name.ToString();
                dr["Price Per Unit"] = st.ItemUnitPriceWithCurrency.ToString();
                dr["Recorded Stock"] = st.AvailableStock.ToString();
                dr["Adjusted Stock"] = st.DifferenceInStock.ToString();
                dr["Final Stock"] = st.PhysicalStock.ToString();
                dr["UOM"] = st.ItemUnit.ToString();
                dr["Remarks"] = st.Remarks.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_StockReconciliation_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Stock Reconciliation Report";
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td></td><td></td><td colspan='4' > <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td></td><td></td><td></td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td></td><td></td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td></td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #endregion

        #region Mehtod for Bind Dropdowns
        public void BindItemDropDown()
        {
            var model = _itemRepository.GetAll();
            var Item_dd = new SelectList(model.ToList(), "ID", "Item_Name");
            ViewData["Item"] = Item_dd;


        }

        public void BindLocationDropDown()
        {
            var model = _locationRepository.GetAll();
            var Wearhouse_dd = new SelectList(model.ToList(), "ID", "LocationName");
            ViewData["WearhouseLocation"] = Wearhouse_dd;


        }

        #endregion

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
}