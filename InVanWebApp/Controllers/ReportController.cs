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
        private IReportRepository _reportRepository;
        private ICompanyRepository _repositoryCompany;
        private IItemRepository _itemRepository;
        private ILocationRepository _locationRepository;
        private IPOPaymentRepository _pOPaymentRepository;
        private IIssueNoteRepository _issueNoteRepository;
        private IGRNRepository _gRNRepository;
        private IRejectionNoteRepository _rejectionNoteRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;


        private static ILog log = LogManager.GetLogger(typeof(ReportController));

        #region Initializing the constructors
        public ReportController()
        {
            _repository = new ReportRepository();
            _reportRepository = new ReportRepository();
            _repositoryCompany = new CompanyRepository();
            _itemRepository = new ItemRepository();
            _locationRepository = new LocationRepository();
            _pOPaymentRepository = new POPaymentRepository();
            _issueNoteRepository = new IssueNoteRepository();
            _gRNRepository = new GRNRepository();
            _rejectionNoteRepository = new RejectionNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
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
            int VendorId = 0; //Rahul added 13-04-23.
            if (vendor != "" && vendor != null)
                VendorId = Convert.ToInt32(vendor);
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
            sb.Append("<th  style='text-align:left;padding-right:20px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date:" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date:" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Indent Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Status</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Supplier Name</th>");

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
            dt.Columns.Add("Supplier Name");

            foreach (PurchaseOrderBO st in purchaseOrders)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["PO Date"] = st.PurchaseOrderDate.ToString();
                dr["PO Number"] = st.PONumber.ToString();
                dr["Indent Number"] = st.IndentNumber.ToString();
                dr["PO Status"] = st.PurchaseOrderStatus.ToString();
                dr["Supplier Name"] = st.CompanyName.ToString();

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
            string ReportName = "PO Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='3' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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
            sb.Append("<th  style='text-align:left;padding-right:-20px;padding-bottom:-290px;font-family:Times New Roman;font-size:11px;'>" + "From Date:" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-family:Times New Roman;font-size:11px;'>" + "To Date:" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

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

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-size:25px;font-family:Times New Roman;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='4' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='4' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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
            sb.Append("<th  style='text-align:right;padding-right:-80px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-weight:bold;color:Red;font-family:Times New Roman; text-color:red bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

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

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='6' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='4' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='5' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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
            sb.Append("<label style='font-size:22px;font-family:Times New Roman;text-align:center;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Times New Roman;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=3 style='text-align:left;font-size:11px;font-family:Times New Roman;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=3 style='text-align:right;font-size:11px;font-family:Times New Roman;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
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

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='3' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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
            sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Times New Roman;'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=3 style='text-align:left;font-size:11px;font-family:Times New Roman;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=3 style='text-align:right;font-family:Times New Roman;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
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
                    string filename = "Rpt_Total_Inventory_Cost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
            string filename = "Rpt_Total_Inventory_Cost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
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
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='3' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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
            sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Times New Roman;'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=5 style='text-align:left;font-size:11px;font-family:Times New Roman;padding-bottom:3px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=5 style='text-align:right;font-family:Times New Roman;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
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
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPriceWithCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AvailableStock + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DifferenceInStock + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PhysicalStock + "</td>");
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

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='5' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='5' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  Inventory Analysis Report (FIFO) 

        #region Binding the Inventory Analysis Report (FIFO) data 

        public ActionResult InventoryAnalysisReportFIFO()
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
        /// Develop By Rahul on 22 Feb'23
        /// Calling method for Inventory Analysis Report (FIFO) data  
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInventoryAnalysisReportFIFOData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var ReportResult = _repository.getInventoryAnalysisFIFOReportData(fromDate, toDate, ItemId);
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
        public ActionResult ExprotAsPDFForInventoryAnalysisFIFO()
        {
            string Fromdate = "From Date : ";
            string Todate = "To Date : ";

            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var resultDetails = _repository.getInventoryAnalysisFIFOReportData(fromDate, toDate, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("InventoryAnalysisReportFIFO", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<StockMasterBO> resultList = TempData["ReportDataTemp"] as List<StockMasterBO>;

            if (resultList.Count < 0)
                return RedirectToAction("InventoryAnalysisReportFIFO", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Inventory Analysis Report (FIFO)";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th colspan=2 style='text-align:left;padding-right:-60px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + Fromdate + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=10 style='text-align:right;padding-right:-530px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            //sb.Append("<tr >");
            //sb.Append("<th Colspan='9' style='text-align:right;padding-right:-370px;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            //sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='12' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:-350px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-weight:bold;color:Red;font-family:Times New Roman; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:10%;font-size:10px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Vendor Name</ th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th colspan=4 style='text-align:center;padding: 5px; font-family:Times New Roman;width:75%;font-size:10px;border: 0.05px  #e2e9f3;'>Available Stock</ th>");
            sb.Append("<th colspan=4 style='text-align:center;padding: 5px; font-family:Times New Roman;width:75%;font-size:10px;border: 0.05px  #e2e9f3;'>Stock In</ th>");
            sb.Append("<th colspan=4 style='text-align:center;padding: 5px; font-family:Times New Roman;width:75%;font-size:10px;border: 0.05px  #e2e9f3;'>Stock Out</ th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>GRN No</ th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>PO No</ th>");
            sb.Append("<th rowspan=2 style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Outward No</ th>");

            sb.Append("</tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Unit Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Total Price</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Currency</ th>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Unit Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Total Price</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Currency</ th>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Unit Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Total Price</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:10px;border: 0.05px  #e2e9f3;'>Currency</ th>");

            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            foreach (var item in resultList)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.CompanyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.AvlDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.AvlQty + " " + "(" + item.ItemUnit + ")" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.AvlUnitPrice + " " + "( " + item.AvlCurrency + " )" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.AvlTotalPrice + " " + "( " + item.AvlCurrency + " )" + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.AvlCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.GRNDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockInQty + " " + "(" + item.ItemUnit + ")" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.ItemUnitPrice + " " + "( " + item.CurrencyName + " )" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockInTotalPrice + " " + "( " + item.CurrencyName + " )" + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.DeliveryChallanDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockOutQty + " " + "(" + item.ItemUnit + ")" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockOutUnitPrice + " " + "( " + item.StockOutCurrency + " )" + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockOutTotalPrice + " " + "( " + item.StockOutCurrency + " )" + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.StockOutCurrency + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.GRNCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.PO_Number + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:10px; font-family:Times New Roman;'>" + item.Outward_No + "</td>");


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

                    writer.PageEvent = new PageHeaderFooter1();
                    pdfDoc.Open();

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_InventoryAnalysisReportFIFO_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel for Inventory FIFO
        public void ExportAsExcelForInventoryAnalysisFIFO()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<StockMasterBO> resultList = _repository.getInventoryAnalysisFIFOReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Available Stock Date");
            dt.Columns.Add("Available Stock Quantity");
            dt.Columns.Add("Available Stock Unit Price");
            dt.Columns.Add("Available Stock Total Price");
            dt.Columns.Add("Stock In Date");
            dt.Columns.Add("Stock In Quantity");
            dt.Columns.Add("Stock In Unit Price");
            dt.Columns.Add("Stock In Total Price");
            dt.Columns.Add("Stock Out Date");
            dt.Columns.Add("Stock Out Quantity");
            dt.Columns.Add("Stock Out Unit Price");
            dt.Columns.Add("Stock Out Total Price");
            dt.Columns.Add("GRN No");
            dt.Columns.Add("PO No");
            dt.Columns.Add("Outward No");

            foreach (StockMasterBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Vendor Name"] = st.CompanyName.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Available Stock Date"] = st.AvlDate.ToString();
                dr["Available Stock Quantity"] = st.AvlQty.ToString();
                dr["Available Stock Unit Price"] = st.AvlUnitPrice.ToString() + " " + "( " + st.AvlCurrency.ToString() + " )";
                dr["Available Stock Total Price"] = st.AvlTotalPrice.ToString() + " " + "( " + st.AvlCurrency.ToString() + " )";
                dr["Stock In Date"] = st.GRNDate.ToString();
                dr["Stock In Quantity"] = st.StockInQty.ToString();
                dr["Stock In Unit Price"] = st.ItemUnitPrice.ToString() + " " + "( " + st.CurrencyName.ToString() + " )";
                dr["Stock In Total Price"] = st.StockInTotalPrice.ToString() + " " + "( " + st.CurrencyName.ToString() + " )";
                dr["Stock Out Date"] = st.DeliveryChallanDate.ToString();
                dr["Stock Out Quantity"] = st.StockOutQty.ToString();
                dr["Stock Out Unit Price"] = st.StockOutUnitPrice.ToString() + " " + "( " + st.StockOutCurrency.ToString() + " )";
                dr["Stock Out Total Price"] = st.StockOutTotalPrice.ToString() + " " + "( " + st.StockOutCurrency.ToString() + " )";
                dr["GRN No"] = st.GRNCode.ToString();
                dr["PO No"] = st.PO_Number.ToString();
                dr["Outward No"] = st.Outward_No.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_InventoryAnalysisReportFIFO_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Inventory Analysis Report (FIFO)";
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='16' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='16' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='16' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='9' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='10' style='text-align:right; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  Company Report

        #region Binding the Company report data 

        public ActionResult CompanyReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CompanyBO model = new CompanyBO();
                //model.fromDate = DateTime.Today;
                //model.toDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Yatri 10 March'23
        /// Calling method for Company Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult getCompanyDataByType(string CompanyType)
        {
            List<CompanyBO> ReportResult = new List<CompanyBO>();


            Session["CompanyType"] = CompanyType;

            ReportResult = _repository.getCompanyDataByType(CompanyType);
            return Json(new { data = ReportResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Company report
        /// <summary>
        /// Created by: Yatri
        /// Creadted Date: 10 March 23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForCompany()
        {

            var CompanyType = Convert.ToString(Session["CompanyType"]);


            var resultDetails = _repository.getCompanyDataByType(CompanyType);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("CompanyReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<CompanyBO> resultList = TempData["ReportDataTemp"] as List<CompanyBO>;

            if (resultList.Count < 0)
                return RedirectToAction("CompanyReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Company Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='5' style='text-align:center'>");
            sb.Append("<label style='font-size:22px;font-family:Times New Roman;text-align:center;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Times New Roman'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=7 style='text-align:right;font-size:11px;padding-bottom:3px;'>" + "Date: " + " " + DateTime.Now.ToString("dd/MMM/yyyy"));
            sb.Append("</th>");
            //sb.Append("<th colspan=5 style='text-align:right;font-size:11px;'>" + Todate + " " + toDate.ToString("dd/MM/yyyy"));
            //sb.Append("</th>");
            sb.Append("</tr>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Type</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Contact Person</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Contact No</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Email Id</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Address</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Remarks</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.CompanyType + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.CompanyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ContactPersonName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ContactPersonNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.EmailId + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Address + "</td>");
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
                    string filename = "Rpt_Company_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Export Excel for Company report
        public void ExportAsExcelForCompany()
        {
            GridView gv = new GridView();

            var CompanyType = Convert.ToString(Session["CompanyType"]);


            List<CompanyBO> resultList = _repository.getCompanyDataByType(CompanyType);

            DataTable dt = new DataTable();

            dt.Columns.Add("CompanyType");
            dt.Columns.Add("CompanyName");
            dt.Columns.Add("ContactPersonName");
            dt.Columns.Add("ContactPersonNo");
            dt.Columns.Add("EmailId");
            dt.Columns.Add("Address");
            dt.Columns.Add("Remarks");

            foreach (CompanyBO st in resultList)
            {
                DataRow dr = dt.NewRow();

                dr["CompanyType"] = st.CompanyType.ToString();
                dr["CompanyName"] = st.CompanyName.ToString();
                dr["ContactPersonName"] = st.ContactPersonName.ToString();
                dr["ContactPersonNo"] = st.ContactPersonNo.ToString();
                dr["EmailId"] = st.EmailId.ToString();
                dr["Address"] = st.Address.ToString();
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
            string filename = "Rpt_Company_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Company Report";
            string Date = "Date : ";
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */

            string content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
                "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
                "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
                "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>" +
                "<tr><td colspan='7' style='text-align:right; font-size:15px;font-weight:bold'>" + Date + DateTime.Now.ToString("dd/MMM/yyyy") + "</td></tr>"
                + "</table>"
                + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  Purchase Invoice Report

        #region Binding the Purchase Invoice Report data 

        public ActionResult PurchaseInvoiceReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PurchaseOrderBO model = new PurchaseOrderBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                PurchaseOrderDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth Purohit on 09 MAR'23
        /// Calling method for Purchase Invoice Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPurchaseInvoiceReportData(DateTime fromDate, DateTime toDate, int ItemId, string Status)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            //Session["LocationId"] = LocationId;
            Session["ItemId"] = ItemId;
            Session["Status"] = Status;
            var POInvoice = _repository.getPurchaseInvoiceReportData(fromDate, toDate, ItemId, Status);
            return Json(new { data = POInvoice }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Purchase Invoice Report
        /// <summary>
        /// Created by: Siddharth Purohit
        /// Creadted Date: 09 MAR'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForPurchaseInvoice()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var Status = Session["Status"].ToString();
            var resultDetails = _repository.getPurchaseInvoiceReportData(fromDate, toDate, itemId, Status);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("PurchaseInvoiceReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<PurchaseOrderBO> resultList = TempData["ReportDataTemp"] as List<PurchaseOrderBO>;

            if (resultList.Count < 0)
                return RedirectToAction("PurchaseInvoiceReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Purchase Invoice Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-80px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-60px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            sb.Append("<br/>");

            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;padding-right:30px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            // sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Company Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Invoice Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Advanced Payment</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Amount Paid</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Balance Payment</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Payment Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;'>Payment Status</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PONumber + "</td>");  //Rahul updated PurchaseOrderDate 06-01-2023. 
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PurchaseOrderDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DelDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.CompanyName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InvoiceNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AdvancedPayment + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AmountPaid + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BalancePayment + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PayDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PurchaseOrderStatus + "</td>");
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
                    string filename = "Rpt_Purchase_Invoice_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel Purchase Invoice Report
        public void ExportAsExcelForPurchaseInvoice()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var Status = Session["Status"].ToString();

            List<PurchaseOrderBO> resultList = _repository.getPurchaseInvoiceReportData(fromDate, toDate, itemId, Status);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("PO Number");
            dt.Columns.Add("PO Date");
            dt.Columns.Add("Delivery Date");
            dt.Columns.Add("Company Name");
            dt.Columns.Add("Invoice Amount");
            dt.Columns.Add("Advance Amount");
            dt.Columns.Add("Amount Paid");
            dt.Columns.Add("Balance Payment");
            dt.Columns.Add("Payment Date");
            dt.Columns.Add("Payment Status");
            //dt.Columns.Add("Approved By");

            foreach (PurchaseOrderBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["PO Number"] = st.PONumber.ToString();
                dr["PO Date"] = st.PurchaseOrderDate.ToString();
                dr["Delivery Date"] = st.DelDate.ToString();
                dr["Company Name"] = st.CompanyName.ToString();
                dr["Invoice Amount"] = st.InvoiceNumber.ToString();
                dr["Advance Amount"] = st.AdvancedPayment.ToString();
                dr["Amount Paid"] = st.AmountPaid.ToString();
                dr["Balance Payment"] = st.BalancePayment.ToString();
                dr["Payment Date"] = st.PayDate.ToString();
                dr["Payment Status"] = st.PurchaseOrderStatus.ToString();



                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Purchase_Invoice_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Purchase Invoice Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='5' style='text-align:left; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='6' style='text-align:right; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  Issue Note Report

        #region Binding the Issue Note Report data 

        public ActionResult IssueNoteReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                IssueNoteBO model = new IssueNoteBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                InvoiceNoteDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth Purohit on 10 MAR'23
        /// Calling method for Issue Invoice Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetIssueNoteReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            //Session["LocationId"] = LocationId;
            Session["ItemId"] = ItemId;
            var IssueNoteInvoice = _repository.getIssueNoteReportData(fromDate, toDate, ItemId);
            return Json(new { data = IssueNoteInvoice }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Purchase Invoice Report
        /// <summary>
        /// Created by: Siddharth Purohit
        /// Creadted Date: 10 MAR'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForIssueNote()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var resultDetails = _repository.getIssueNoteReportData(fromDate, toDate, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("IssueNoteReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<IssueNoteBO> resultList = TempData["ReportDataTemp"] as List<IssueNoteBO>;

            if (resultList.Count < 0)
                return RedirectToAction("IssueNoteReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Issue Note Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;font-family:Times New Roman;padding-right:-10px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='8' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-family:Times New Romanfont-weight:bold;color:Red; text-color:red bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Issue Note Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Issue Note Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Quantity Requested</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Quantity Issued</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Available Stock Before Issue</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Stock After Issuing</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.IssueNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.IssueNoteNoDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.QuantityRequested + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.QuantityIssued + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AvailableStockBeforeIssue + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.StockAfterIssuing + "</td>");


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
                    string filename = "Rpt_Issue_Note_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel Purchase Invoice Report
        public void ExportAsExcelForIssueNote()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<IssueNoteBO> resultList = _repository.getIssueNoteReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Issue Note Number");
            dt.Columns.Add("Issue Note Date");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Quantity Requested");
            dt.Columns.Add("Quantity Issued");
            dt.Columns.Add("Available Stock Before Issue");
            dt.Columns.Add("Stock After Issuing");


            foreach (IssueNoteBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Issue Note Number"] = st.IssueNoteNo.ToString();
                dr["Issue Note Date"] = st.IssueNoteNoDate.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Quantity Requested"] = st.QuantityRequested.ToString();
                dr["Quantity Issued"] = st.QuantityIssued.ToString();
                dr["Available Stock Before Issue"] = st.AvailableStockBeforeIssue.ToString();
                dr["Stock After Issuing"] = st.StockAfterIssuing.ToString();




                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Issue_Note_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Issue Note Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='5' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='5' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='5' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='4' style='text-align:left;font-family:Times New Roman; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='4' style='text-align:right;font-family:Times New Roman; font-size:15px;font-weight:bold'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  GRN Report

        #region Binding the GRN Report data 

        public ActionResult GRNReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                GRN_BO model = new GRN_BO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                GRNDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth Purohit on 10 MAR'23
        /// Calling method for GRN Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGRNReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            //Session["LocationId"] = LocationId;
            Session["ItemId"] = ItemId;
            var GRNReport = _repository.getGRNReportData(fromDate, toDate, ItemId);
            return Json(new { data = GRNReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF GRN Report
        /// <summary>
        /// Created by: Siddharth Purohit
        /// Creadted Date: 10 MAR'23
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForGRNReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);
            var resultDetails = _repository.getGRNReportData(fromDate, toDate, itemId);

            TempData["ReportDataTemp"] = resultDetails;
            if (TempData["ReportDataTemp"] == null)
            {
                return RedirectToAction("GRNReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<GRN_BO> resultList = TempData["ReportDataTemp"] as List<GRN_BO>;

            if (resultList.Count < 0)
                return RedirectToAction("GRNReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "GRN Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;

            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;font-family:Times New Roman;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;font-family:Times New Roman;padding-right:-60px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-weight:bold;color:Red;font-family:Times New Roman; text-color:red; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:13%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>GRN Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>GRN Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Order Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Received Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:15px;border: 0.05px  #e2e9f3;'>Inward Note Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward QC Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Address</ th>");



            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.GRN_Date + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.GRNCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.OrderQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReceivedQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardNoteNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQCNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DeliveryAddress + "</td>");

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
                    string filename = "Rpt_GRN_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        #endregion

        #region Excel GRN Report
        public void ExportAsExcelForGRNReport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<GRN_BO> resultList = _repository.getGRNReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("GRN Date");
            dt.Columns.Add("GRN Code");
            dt.Columns.Add("Order Quantity");
            dt.Columns.Add("Received Quantity");
            dt.Columns.Add("Inward Quantity");
            dt.Columns.Add("Inward Note Number");
            dt.Columns.Add("Inward QC Number");
            dt.Columns.Add("Delivery Address");


            foreach (GRN_BO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Item Code"] = st.ItemCode.ToString();
                dr["GRN Date"] = st.GRN_Date.ToString();
                dr["GRN Code"] = st.GRNCode.ToString();
                dr["Order Quantity"] = st.OrderQty.ToString();
                dr["Received Quantity"] = st.ReceivedQty.ToString();
                dr["Inward Quantity"] = st.InwardQty.ToString();
                dr["Inward Note Number"] = st.InwardNoteNumber.ToString();
                dr["Inward QC Number"] = st.InwardQCNumber.ToString();
                dr["Delivery Address"] = st.DeliveryAddress.ToString();




                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "GRN_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "GRN Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
              + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Fromdate + fromdate
              + "</td><td colspan='5' style='text-align:right; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Todate + todate
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region Binding the Rejection report data 

        public ActionResult RejectionReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                RejectionNoteBO model = new RejectionNoteBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindRejectionDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 13 MAR 2023
        /// Calling method for Rejection Report Data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRejectionReportData(DateTime fromDate, DateTime toDate, int ItemId, int FlagDebitNote = 0)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            Session["FagDebitNote"] = FlagDebitNote;

            var rejectionNoteReport = _repository.getRejectionReportData(fromDate, toDate, ItemId, FlagDebitNote);

            return Json(new { data = rejectionNoteReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Rejection Note
        /// <summary>
        /// Create by Siddhareth on 13 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForRejectionReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            var RejectionReportDetails = _repository.getRejectionReportData(fromDate, toDate, itemId);
            TempData["RejectionReportDataTemp"] = RejectionReportDetails;
            if (TempData["RejectionReportDataTemp"] == null)
            {
                return RedirectToAction("RejectionReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<RejectionNoteItemDetailsBO> resultList = TempData["RejectionReportDataTemp"] as List<RejectionNoteItemDetailsBO>;

            if (resultList.Count < 0)
                return RedirectToAction("RejectionReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Rejection Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-80px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-60px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-family:Times New Roman;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejection Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward QC/Pre-Production QC Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Reason For Rejection</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price(Rs)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Received Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejected Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReasonForRejection + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TotalRecevingQuantiy + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectedQuantity + "</td>");
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
                    string filename = "Rpt_Rejection_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Rejection note Report
        public void ExportAsExcelForRejectionReport()
        {
            GridView gv = new GridView();
            var itemId = Convert.ToInt32(Session["ItemId"]);
            List<RejectionNoteItemDetailsBO> resultList = _repository.getRejectionReportData(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["toDate"]), itemId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Rejection Number");
            dt.Columns.Add("Inward QC or Pre-Production QC Number");
            dt.Columns.Add("Reason For Rejection");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Unit Price(Rs)");
            dt.Columns.Add("Recived Quantity(KG)");
            dt.Columns.Add("Rejected Quantity(KG)");
            dt.Columns.Add("Approved By");

            foreach (RejectionNoteItemDetailsBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Date"] = st.RejectionNoteDate.ToString();
                dr["Rejection Number"] = st.RejectionNoteNo.ToString();
                dr["Inward QC or Pre-Production QC Number"] = st.InwardNumber.ToString();
                dr["Reason For Rejection"] = (st.ReasonForRR == null) ? "" : st.ReasonForRR.ToString();
                dr["Item Name"] = st.Item_Name.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Unit Price(Rs)"] = st.ItemUnitPrice.ToString();
                dr["Rejected Quantity(KG)"] = st.RejectedQuantity.ToString();
                dr["Recived Quantity(KG)"] = st.TotalRecevingQuantiy.ToString();
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
            string filename = "Rpt_Rejection_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Rejection Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='5' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region Yeild report

        #region Yeild report data 

        public ActionResult YeildReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindFGLocationBatchNumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 21 MAR 2023
        /// Calling method for Yeild report data 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetYeildReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var yeildReport = _repository.getYeildReportData(fromDate, toDate, ItemId);
            return Json(new { data = yeildReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Yeild report
        /// <summary>
        /// Create by Siddhareth on 21 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForYeildReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            var YeildReport = _repository.getYeildReportData(fromDate, toDate, itemId);
            TempData["YeildReportDataTemp"] = YeildReport;
            if (TempData["YeildReportDataTemp"] == null)
            {
                return RedirectToAction("YeildReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["YeildReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("YeildReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Yeild Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:20px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Work Order Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Batch Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Product Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Expected Yeild</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Actual Yeild</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WorkOrderNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BatchNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ExpectedYeild + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ActualYeild + "</td>");
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
                    string filename = "Rpt_Yeild_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Batchwise Production report
        public void ExportAsExcelForYeildReport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<ReportBO> resultList = _repository.getYeildReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Work Order Number");
            dt.Columns.Add("Batch Number");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Expected Yeild");
            dt.Columns.Add("Actual Yeild");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Work Order Number"] = st.WorkOrderNumber.ToString();
                dr["Batch Number"] = st.BatchNumber.ToString();
                dr["Product Name"] = st.ProductName.ToString();
                dr["Expected Yeild"] = st.ExpectedYeild.ToString();
                dr["Actual Yeild"] = st.ActualYeild.ToString();





                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Yeild_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Yeild Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='3' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region FG Locationwise report

        #region FG Locationwise report data 

        public ActionResult FGLocationWiseReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindLocationDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 21 MAR 2023
        /// Calling method for FG Location wise report data 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFGLocationwiseReportData(DateTime fromDate, DateTime toDate, int LocationId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = LocationId;
            var FGLocationwiseReport = _repository.getFGLocationwiseReportData(fromDate, toDate, LocationId);
            return Json(new { data = FGLocationwiseReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF FG Locationwise report
        /// <summary>
        /// Create by Siddhareth on 21 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForFGLocationwiseReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var locationId = Convert.ToInt32(Session["ItemId"]);

            var FGLocationwiseReport = _repository.getFGLocationwiseReportData(fromDate, toDate, locationId);
            TempData["FGLocationwiseReportDataTemp"] = FGLocationwiseReport;
            if (TempData["FGLocationwiseReportDataTemp"] == null)
            {
                return RedirectToAction("FGLocationWiseReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["FGLocationwiseReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("FGLocationWiseReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "FG (Location wise Report)";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:5px;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;font-family:Times New Roman;padding-right:-1px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='5' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-weight:bold;font-family:Times New Roman;color:Red; text-color:red bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Location Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Quantity</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.LocationName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Quantity + "</td>");

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
                    string filename = "Rpt_FGLocationwise_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel FG Locationwise report
        public void ExportAsExcelForFGLocationwiseReport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var locationId = Convert.ToInt32(Session["ItemId"]);

            List<ReportBO> resultList = _repository.getFGLocationwiseReportData(fromDate, toDate, locationId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Location Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Unit");
            dt.Columns.Add("Item Unit Price");
            dt.Columns.Add("Quantity");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Location Name"] = st.LocationName.ToString();
                dr["Item Code"] = st.ItemCode.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Item Unit"] = st.ItemUnit.ToString();
                dr["Item Unit Price"] = st.ItemUnitPrice.ToString();
                dr["Quantity"] = st.Quantity.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_FGLocationwise_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "FG (Location wise Report)";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;font-family:Times New Roman;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-size:15px;font-family:Times New Roman;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='3' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='4' style='text-align:right; font-size:15px;font-family:Times New Roman;font-weight:bold'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        public void PurchaseOrderDropDown()
        {
            var model = _pOPaymentRepository.GetAll();
            var PurchaseOrder_dd = new SelectList(model.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberdd"] = PurchaseOrder_dd;
        }

        public void InvoiceNoteDropDown()
        {
            var model = _issueNoteRepository.GetIssueNoteNumber();
            var IssueNoteNo_dd = new SelectList(model.ToList(), "ID", "IssueNoteNo");
            ViewData["IssueNoteNOdd"] = IssueNoteNo_dd;


        }

        public void GRNDropDown()
        {
            var model = _gRNRepository.GetAll();
            var GRNReport_dd = new SelectList(model.ToList(), "ID", "GRNCode");
            ViewData["GRNReportdd"] = GRNReport_dd;


        }

        public void BindRejectionDropDown()
        {
            var model = _rejectionNoteRepository.GetAll();
            var RejectionReport_dd = new SelectList(model.ToList(), "ID", "RejectionNoteNo");
            ViewData["RejectionNumberdd"] = RejectionReport_dd;


        }

        public void BindBatchNumberDropDown()
        {
            var model = _repository.GetAll();
            var BatchNumberdd = new SelectList(model.ToList(), "BatchNumber", "BatchNumber");
            ViewData["BatchNumberdd"] = BatchNumberdd;
        }

        public void BindFGLocationBatchNumberDropDown()
        {
            var model = _repository.GetGFLocationBatchNumber();
            var BatchNumberdd = new SelectList(model.ToList(), "ID", "BatchNumber");
            ViewData["BatchNumberdd"] = BatchNumberdd;
        }

        #endregion

        #region  Raw Material Cost analysis 

        #region Binding the Raw Material Cost analysis report data 

        public ActionResult RawMaterialCostAnalysisReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindItemDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 23 MAR 2023
        /// Calling method for Raw Material Cost Analysis report data 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRawMaterialCostAnalysisReportData(DateTime fromDate, DateTime toDate, int itemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = itemId;
            var rawMaterialCostAnalysis = _repository.getRawMaterialCostAnalysisReportData(fromDate, toDate, itemId);
            return Json(new { data = rawMaterialCostAnalysis }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Raw Material Cost Analysis report
        /// <summary>
        /// Create by Siddhareth on 23 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForRawmaterialCostAnalyisReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            var rawMaterialCostAnalysis = _repository.getRawMaterialCostAnalysisReportData(fromDate, toDate, itemId);
            TempData["rawMaterialCostAnalysisDataTemp"] = rawMaterialCostAnalysis;
            if (TempData["rawMaterialCostAnalysisDataTemp"] == null)
            {
                return RedirectToAction("RawMaterialCostAnalysisReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["rawMaterialCostAnalysisDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("RawMaterialCostAnalysisReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Raw Material Cost Analysis Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-5px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:10px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Work Order Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Quantity Used</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Raw Material Cost</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WorkOrderNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.QuantityUsed + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RawMaterialCost + "</td>");
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
                    string filename = "Rpt_RawMaterialCostAnalysis_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Raw Material Cost Analysis report
        public void ExportAsExcelForRawmaterialCostAnalyisReport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            List<ReportBO> resultList = _repository.getRawMaterialCostAnalysisReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Work Order Number");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Quantity Used");
            dt.Columns.Add("Item Unit Price");
            dt.Columns.Add("Raw Material Cost");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Work Order Number"] = st.WorkOrderNumber.ToString();
                dr["Item Code"] = st.ItemCode.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Quantity Used"] = st.QuantityUsed.ToString();
                dr["Item Unit Price"] = st.ItemUnitPrice.ToString();
                dr["Raw Material Cost"] = st.RawMaterialCost.ToString();





                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_RawMaterialCostAnalysis_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Raw Material Cost Analysis Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='3' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='4' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  BatchwiseProductionCost report

        #region Binding the Batchwise Production report data 

        public ActionResult BatchwiseProductionCostReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindBatchNumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 21 MAR 2023
        /// Calling method for  Batchwise Production report data 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBatchwiseProductionCostReportData(DateTime fromDate, DateTime toDate, string ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var batchWiseProductionCost = _repository.getBatchwiseProductionCostReportData(fromDate, toDate, ItemId);
            return Json(new { data = batchWiseProductionCost }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Batchwise Production report
        /// <summary>
        /// Create by Siddhareth on 21 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForBatchwiseProductionCostReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var itemId = Convert.ToInt32(Session["ItemId"]);
            var itemId = (Session["ItemId"].ToString());

            var BatchwiseProductionReport = _repository.getBatchwiseProductionCostReportData(fromDate, toDate, itemId);
            TempData["BatchwiseProductionReportDataTemp"] = BatchwiseProductionReport;
            if (TempData["BatchwiseProductionReportDataTemp"] == null)
            {
                return RedirectToAction("BatchwiseProductionCostReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["BatchwiseProductionReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("BatchwiseProductionCostReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Batch-wise Production Cost";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            //sb.Append("<tr>");
            //sb.Append("<th style='text-align:right;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            //sb.Append("</th></tr>");
            //sb.Append("<tr>");
            //sb.Append("<th colspan=6 style='text-align:right;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            //sb.Append("</th></tr>");

            sb.Append("<tr>");
            sb.Append("<th colspan=3 style='text-align:left;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th>");
            sb.Append("<th colspan=3 style='text-align:right;font-family:Times New Roman;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");

            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='10' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px;font-weight:bold;color:Red;font-family:Times New Roman; text-color:red; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:12px;border: 0.05px  #e2e9f3;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Work Order Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Production Material Issue Note Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Product Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:25%;font-size:13px;border: 0.05px  #e2e9f3;'>Batch Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Raw Material Cost</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 1px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WorkOrderNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductionMaterailIssueNoteNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BatchNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RawMaterialCost + "</td>");
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
                    string filename = "Rpt_BatchwiseProductionCost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Batchwise Production report
        public void ExportAsExcelForBatchwiseProductionCostReport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var itemId = (Session["ItemId"].ToString());

            List<ReportBO> resultList = _repository.getBatchwiseProductionCostReportData(fromDate, toDate, itemId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Work Order Number");
            dt.Columns.Add("Production Material Issue Note Number");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Batch Number");
            dt.Columns.Add("Raw Material Cost");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Work Order Number"] = st.WorkOrderNumber.ToString();
                dr["Production Material Issue Note Number"] = st.ProductionMaterailIssueNoteNumber.ToString();
                dr["Product Name"] = st.ProductName.ToString();
                dr["Batch Number"] = st.BatchNumber.ToString();
                dr["Raw Material Cost"] = st.RawMaterialCost.ToString();





                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_BatchwiseProductionCost_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Batch-wise Production Cost";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' ><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr></tr>" +
                "<tr align='center'><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #region Delivery Challan (Against SO) report

        #region Delivery Challan (Against SO) report

        public ActionResult DeliveryChallanAgainstSOReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindSONumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 03 APR 2023
        /// Calling method for Delivery Challan (Against SO) report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDeliveryChallanAgainstSOReportData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = SONumberId;
            var DeliveryChallanAgainstSOReport = _reportRepository.getDeliveryChallanAgainstSOReportData(fromDate, toDate, SONumberId);
            return Json(new { data = DeliveryChallanAgainstSOReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Delivery Challan (Against SO) report
        /// <summary>
        /// Create by Siddhareth on 03 APR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForDeliveryChallanAgainstSOreport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            string SONumberID = Convert.ToString(Session["ItemId"]);

            var DeliveryChallanAgainstSOreport = _reportRepository.getDeliveryChallanAgainstSOReportData(fromDate, toDate, SONumberID);
            TempData["DeliveryChallanAgainstSOreportDataTemp"] = DeliveryChallanAgainstSOreport;
            if (TempData["DeliveryChallanAgainstSOreportDataTemp"] == null)
            {
                return RedirectToAction("DeliveryChallanAgainstSOReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["DeliveryChallanAgainstSOreportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("DeliveryChallanAgainstSOReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Delivery Challan (Against SO) Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:20px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-1px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='6' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>SO Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>SO Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Challan NO</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Delivery Challan Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Client Name</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SODate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DeliveryChallanNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DeliveryChallanDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ClientName + "</td>");

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
                    string filename = "Rpt_DeliveryChallan(AgainstPO)_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel PDF Delivery Challan (Against SO) report
        public void ExportAsExcelForDeliveryChallanAgainstSOreport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            string SONumberId = Convert.ToString(Session["ItemId"]);

            List<ReportBO> resultList = _reportRepository.getDeliveryChallanAgainstSOReportData(fromDate, toDate, SONumberId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("SO Number");
            dt.Columns.Add("SO Date");
            dt.Columns.Add("Delivery Challan No");
            dt.Columns.Add("Delivery Challan Date");
            dt.Columns.Add("Client Name");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["SO Number"] = st.SONumber.ToString();
                dr["SO Date"] = st.SODate.ToString();
                dr["Delivery Challan No"] = st.DeliveryChallanNo.ToString();
                dr["Delivery Challan Date"] = st.DeliveryChallanDate.ToString();
                dr["Client Name"] = st.ClientName.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_DeliveryChallan(AgainstPO)_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Delivery Challan (Against SO) Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' ><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td colspan='4'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #region Sales report

        #region Sales report

        public ActionResult SalesReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindSONumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 03 APR 2023
        /// Calling method for Sales  report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSalesReportData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = SONumberId;
            var SalesReport = _reportRepository.getSalesReportData(fromDate, toDate, SONumberId);
            return Json(new { data = SalesReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Sales report
        /// <summary>
        /// Create by Siddhareth on 03 APR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForSalesreport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            string SONumberID = Convert.ToString(Session["ItemId"]);

            var SalesReport = _reportRepository.getSalesReportData(fromDate, toDate, SONumberID);
            TempData["SalesReportDataTemp"] = SalesReport;
            if (TempData["SalesReportDataTemp"] == null)
            {
                return RedirectToAction("SalesReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["SalesReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("SalesReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Sales Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:20px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-1px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='6' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Inquiry No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>SO Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>SO Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Client Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Status</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InquiryNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SODate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ClientName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Status + "</td>");

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
                    string filename = "Rpt_Sales_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Sales report
        public void ExportAsExcelForSalesreport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            string SONumberId = Convert.ToString(Session["ItemId"]);

            List<ReportBO> resultList = _reportRepository.getSalesReportData(fromDate, toDate, SONumberId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Inquiry No");
            dt.Columns.Add("SO Number");
            dt.Columns.Add("SO Date");
            dt.Columns.Add("Client Name");
            dt.Columns.Add("Status");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Inquiry No"] = st.InquiryNo.ToString();
                dr["SO Number"] = st.SONumber.ToString();
                dr["SO Date"] = st.SODate.ToString();
                dr["Client Name"] = st.ClientName.ToString();
                dr["Status"] = st.Status.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Sales_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Sales Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
              "<tr><td colspan='4' ><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ReportName + "</span></td></tr></tr>" +
              "<tr><td><td colspan='4'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
              "<tr><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></tr>" +
              "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
              + "<td><td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #region Sales Invoice report

        #region Sales Invoice report

        public ActionResult SalesInvoiceReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindSONumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 03 APR 2023
        /// Calling method for Sales Invoice report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSalesReportInvoiceData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = SONumberId;
            var SalesInvoiceReport = _reportRepository.getSalesInvoiceReportData(fromDate, toDate, SONumberId);
            return Json(new { data = SalesInvoiceReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Sales Invoice report
        /// <summary>
        /// Create by Siddhareth on 03 APR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForSalesInvoicereport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            string SONumberID = Convert.ToString(Session["ItemId"]);

            var SalesReport = _reportRepository.getSalesInvoiceReportData(fromDate, toDate, SONumberID);
            TempData["SalesInvoiceReportDataTemp"] = SalesReport;
            if (TempData["SalesInvoiceReportDataTemp"] == null)
            {
                return RedirectToAction("SalesInvoiceReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["SalesInvoiceReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("SalesInvoiceReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Sales Invoice Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:20px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-1px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='6' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:100px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>SO Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Invoice No</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Anvoice Amount</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Amount Recived</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Balance Recivable</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InquiryNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InvoiceAmount + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AmountRecived + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BalanceRecivable + "</td>");

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
                    string filename = "Rpt_SalesInvoice_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Sales Invoice report
        public void ExportAsExcelForSalesInvoicereport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            string SONumberId = Convert.ToString(Session["ItemId"]);

            List<ReportBO> resultList = _reportRepository.getSalesInvoiceReportData(fromDate, toDate, SONumberId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("SO Number");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Invoice Amount");
            dt.Columns.Add("Amount Recived");
            dt.Columns.Add("Balance Recivable");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["SO Number"] = st.SONumber.ToString();
                dr["Invoice No"] = st.InvoiceNo.ToString();
                dr["Invoice Amount"] = st.InvoiceAmount.ToString();
                dr["Amount Recived"] = st.AmountRecived.ToString();
                dr["Balance Recivable"] = st.BalanceRecivable.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_SalesInvoice_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Sales Invoice Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4'style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #region Debit Note report

        #region Debit Note report

        public ActionResult DebitNoteReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindDBNoteNumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 03 APR 2023
        /// Calling method for Debit Note Report data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDBNoteData(DateTime fromDate, DateTime toDate, int DBNoteNumberId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = DBNoteNumberId;
            var DBNoteReport = _reportRepository.getDBNoteReportData(fromDate, toDate, DBNoteNumberId);
            return Json(new { data = DBNoteReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Debit Notee report
        /// <summary>
        /// Create by Siddhareth on 03 APR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForDBNotereport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var DBNoteNumberId = Convert.ToInt32(Session["ItemId"]);

            var DBNoteReport = _reportRepository.getDBNoteReportData(fromDate, toDate, DBNoteNumberId);
            TempData["DBNoteDataTemp"] = DBNoteReport;
            if (TempData["DBNoteDataTemp"] == null)
            {
                return RedirectToAction("DebitNoteReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["DBNoteDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("DebitNoteReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Debit Note Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-10px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-1px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='6' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; color:red; bottom:20px;font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Debit Note No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Debit Note Date</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO No</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Quantity</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Debited Quantity</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DebitNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DebitNoteDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PONo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.POQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.DebitedQuantity + "</td>");

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
                    string filename = "Rpt_DebitNote_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Debit Note report
        public void ExportAsExcelForDBNotereport()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            //var locationId = Convert.ToInt32(Session["LocationId"]);
            var DBNoteNumberId = Convert.ToInt32(Session["ItemId"]);

            List<ReportBO> resultList = _reportRepository.getDBNoteReportData(fromDate, toDate, DBNoteNumberId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Debit Note No");
            dt.Columns.Add("Debit Note Date");
            dt.Columns.Add("PO No");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("PO Quantity");
            dt.Columns.Add("Debited Quantity");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Debit Note No"] = st.DebitNoteNo.ToString();
                dr["Debit Note Date"] = st.DebitNoteDate.ToString();
                dr["PO No"] = st.PONo.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["PO Quantity"] = st.POQuantity.ToString();
                dr["Debited Quantity"] = st.DebitedQuantity.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_DebitNote_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Debit Note Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date : ";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
                "<tr><td colspan='4' ><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ReportName + "</span></td></tr></tr>" +
                "<tr><td><td colspan='4'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
                "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
                "<tr><tr><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
                + "<td><td><td></td><td></td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
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

        #region Wastage Report 

        #region Binding the Wastage report data 
        public ActionResult WastageReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                RejectionNoteBO model = new RejectionNoteBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindInwardPODropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul 7 Apr 2023
        /// Calling method for Wastage Report Data  
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWastageReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var wastageReport = _repository.getWastageReportData(fromDate, toDate, ItemId);
            return Json(new { data = wastageReport }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export PDF Wastage Report
        /// <summary>
        /// Rahul Export PDF Wastage Report 07 Apr 2023. 
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForWastageReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            var WastageReportDetails = _repository.getWastageReportData(fromDate, toDate, itemId);
            TempData["WastageReportDataTemp"] = WastageReportDetails;
            if (TempData["WastageReportDataTemp"] == null)
            {
                return RedirectToAction("WastageReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<RejectionNoteItemDetailsBO> resultList = TempData["WastageReportDataTemp"] as List<RejectionNoteItemDetailsBO>;

            if (resultList.Count < 0)
                return RedirectToAction("WastageReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Wastage Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th colspan='8' style='text-align:left;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan='9' style='text-align:right;padding-right:-480px;padding-bottom:-290px;font-size:11px;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            //sb.Append("<tr >");
            //sb.Append("<th Colspan='9' style='text-align:right;padding-right:-370px;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            //sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='14' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-family:Times New Roman;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejection Number</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Number</ th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward QC Number</ th>");    ///Remove 17-05-23.
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Production QC Number</ th>");  ///added 17-05-23.  
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PO Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Supplier Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price(Rs)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Received Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejected Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Wastage Percentage</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Reason For Wastage</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectionNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardNumber + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQCNumber + "</td>");     ///Remove 17-05-23.
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductionQCNumber + "</td>");  ///added 17-05-23.  
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SupplierName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.QuantityTookForSorting + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectedQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WastageQuantityInPercentage + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReasonForRR + "</td>");
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
                    pdfDoc.SetPageSize(new Rectangle(850f, 1150f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter1();
                    pdfDoc.Open();

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_Wastage_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

        public class PageHeaderFooter1 : PdfPageEventHelper
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
                var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = -150 };
                numberCell.Border = 0;
                numberTable.AddCell(numberCell);

                numberTable.TotalWidth = 50;
                numberTable.WriteSelectedRows(0, -1, document.Right - 300, document.Bottom + 4, writer.DirectContent);
            }
        }


        #endregion

        #region Excel Wastage Report 
        public void ExportAsExcelForWastageReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            GridView gv = new GridView();
            List<RejectionNoteItemDetailsBO> resultList = _repository.getWastageReportData(fromDate, toDate, itemId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Rejection Number"); ///added 17-04-23.  
            dt.Columns.Add("Inward Number");
            //dt.Columns.Add("Inward QC Number"); ///Remove 17-05-23.
            dt.Columns.Add("Production QC Number"); ///added 17-05-23.
            dt.Columns.Add("PO Number");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Unit Price(Rs)");
            dt.Columns.Add("Inward Quantity (KG)");
            dt.Columns.Add("Received Quantity(KG)");
            //dt.Columns.Add("Balance Quantity (KG)");///Remove 17-04-23.
            dt.Columns.Add("Rejected Quantity(KG)");
            dt.Columns.Add("Wastage Percentage");
            dt.Columns.Add("Reason For Wastage");
            dt.Columns.Add("Approved By");

            foreach (RejectionNoteItemDetailsBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Date"] = st.RejectionNoteDate.ToString(); ///updated 17-04-23.
                dr["Rejection Number"] = st.RejectionNoteNo.ToString(); ///added 17-04-23.  
                dr["Inward Number"] = st.InwardNumber.ToString();
                //dr["Inward QC Number"] = st.InwardQCNumber.ToString();    ///Remove 17-05-23.
                dr["Production QC Number"] = st.ProductionQCNumber.ToString();  ///added 17-05-23.
                dr["PO Number"] = st.PONumber.ToString();
                dr["Supplier Name"] = st.SupplierName.ToString();
                dr["Item Name"] = st.Item_Name.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Unit Price(Rs)"] = st.ItemUnitPrice.ToString();
                dr["Inward Quantity (KG)"] = st.InwardQuantity.ToString();
                dr["Received Quantity(KG)"] = st.QuantityTookForSorting.ToString();
                //dr["Balance Quantity (KG)"] = st.BalanceQuantity.ToString(); ///Remove 17-04-23.
                dr["Rejected Quantity(KG)"] = st.RejectedQuantity.ToString();
                dr["Wastage Percentage"] = st.WastageQuantityInPercentage.ToString();
                dr["Reason For Wastage"] = (st.ReasonForRR == null) ? "" : st.ReasonForRR.ToString();///updated 17-04-23.
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
            string filename = "Rpt_Wastage_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Wastage Report";/* The Wastage Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='14' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='14' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='14' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='8' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='8' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region  Pre-production QC Report 

        #region Binding the Pre-production QC Report data 
        public ActionResult PreProduction_QCReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PreProduction_QC_Details model = new PreProduction_QC_Details();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindPreProduction_QCWODropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul 11 Apr 2023
        /// Calling method for Pre-Production_QC Report Data  
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreProduction_QCReportData(DateTime fromDate, DateTime toDate, int ItemId)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["ItemId"] = ItemId;
            var preProduction_QCReport = _repository.getPreProduction_QCReportData(fromDate, toDate, ItemId);
            return Json(new { data = preProduction_QCReport }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export PDF Pre-production QC Report
        /// <summary>
        /// Rahul Export PDF Pre-production QC Report 12 Apr 2023.  
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForPreProduction_QCReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            var PreProduction_QCReportDetails = _repository.getPreProduction_QCReportData(fromDate, toDate, itemId);
            TempData["PreProduction_QCReportDataTemp"] = PreProduction_QCReportDetails;
            if (TempData["PreProduction_QCReportDataTemp"] == null)
            {
                return RedirectToAction("PreProduction_QCReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<PreProduction_QC_Details> resultList = TempData["PreProduction_QCReportDataTemp"] as List<PreProduction_QC_Details>;

            if (resultList.Count < 0)
                return RedirectToAction("PreProduction_QCReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Pre Production QC Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:left;padding-right:-80px;padding-bottom:-290px;font-size:11px;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-440px;padding-bottom:-290px;font-size:11px;'>" + "Todate :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            //sb.Append("<tr >");
            //sb.Append("<th Colspan='9' style='text-align:right;padding-right:-370px;padding-bottom:-85px;font-size:11px;'>" + DateTime.Now.ToString("dd/MMM/yyyy"));
            //sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='14' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-family:Times New Roman;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:16%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>WO Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Material Issue Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>PQC Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price(Rs)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Inward Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Received Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Balance Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejected Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Wastage Quantity (%)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Reason For Wastage</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.InwardQCDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WONumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.MaterialIssueNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PQCNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.IssuedQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.QuantityTookForSorting + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BalanceQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectedQuantity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WastageQuantityInPercentage + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ReasonForWastage + "</td>");
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
                    pdfDoc.SetPageSize(new Rectangle(850f, 1150f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter1();
                    pdfDoc.Open();
                    //pdfDoc.NewPage();

                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Rpt_PreProduction_QC_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }
        #endregion

        #region Excel Pre-production QC Report 
        public void ExportAsExcelForPreProduction_QCReport()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var itemId = Convert.ToInt32(Session["ItemId"]);

            GridView gv = new GridView();
            List<PreProduction_QC_Details> resultList = _repository.getPreProduction_QCReportData(fromDate, toDate, itemId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("WO Number");
            dt.Columns.Add("Material Issue Number");
            dt.Columns.Add("PQC Number");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Unit Price(Rs)");
            dt.Columns.Add("Inward Quantity (KG)");
            dt.Columns.Add("Received Quantity(KG)");
            dt.Columns.Add("Balance Quantity (KG)");
            dt.Columns.Add("Rejected Quantity(KG)");
            dt.Columns.Add("Wastage Quantity (%)");
            dt.Columns.Add("Reason For Wastage");
            dt.Columns.Add("Approved By");

            foreach (PreProduction_QC_Details st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["Date"] = st.InwardQCDate.ToString();
                dr["WO Number"] = st.WONumber.ToString();
                dr["Material Issue Number"] = st.MaterialIssueNumber.ToString();
                dr["PQC Number"] = st.PQCNumber.ToString();
                dr["Item Name"] = st.Item_Name.ToString();
                dr["Item Code"] = st.Item_Code.ToString();
                dr["Item Unit Price(Rs)"] = st.ItemUnitPrice.ToString();
                dr["Inward Quantity (KG)"] = st.IssuedQuantity.ToString();
                dr["Received Quantity(KG)"] = st.QuantityTookForSorting.ToString();
                dr["Balance Quantity (KG)"] = st.BalanceQuantity.ToString();
                dr["Rejected Quantity(KG)"] = st.RejectedQuantity.ToString();
                dr["Wastage Quantity (%)"] = st.WastageQuantityInPercentage.ToString();
                dr["Reason For Wastage"] = (st.ReasonForWastage == null) ? "" : st.ReasonForWastage.ToString();
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
            string filename = "Rpt_PreProduction_QC_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Pre Production QC Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='13' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='13' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='13' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='8' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='7' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region Post production Rejection Report

        #region Binding the  Post Production Rejection report

        public ActionResult PostProductionRejectionReport()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ReportBO model = new ReportBO();
                model.fromDate = DateTime.Today;
                model.toDate = DateTime.Today;
                BindBatchNumberDropDown();
                BindWorkOrderNumberDropDown();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }


        /// <summary>
        /// Develop By Siddharth on 25 APR 2023
        /// Calling method for  Post Production Rejection report Data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPostProductionRejectionData(DateTime fromDate, DateTime toDate, string BatchNumber = "", string WorkOrderNumber = "")
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            Session["BatchNumber"] = BatchNumber;
            Session["WorkOrderNumber"] = WorkOrderNumber;
            var postProductionRejectionNoteReport = _repository.getPostProductionRejectionReportData(fromDate, toDate, BatchNumber, WorkOrderNumber);
            return Json(new { data = postProductionRejectionNoteReport }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF Post Production Rejection Report
        /// <summary>
        /// Create by Siddhareth on 13 MAR 2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExprotAsPDFForPostProjectionRejection()
        {
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            var batchNumber = Convert.ToString(Session["BatchNumber"]);
            var workOrderNumber = Convert.ToString(Session["WorkOrderNumber"]);

            var PostProductionRejectionReportDetails = _repository.getPostProductionRejectionReportData(fromDate, toDate, batchNumber, workOrderNumber);
            TempData["PostProductionRejectionReportDataTemp"] = PostProductionRejectionReportDetails;
            if (TempData["PostProductionRejectionReportDataTemp"] == null)
            {
                return RedirectToAction("PostProductionRejectionReport", "Report");
            }

            StringBuilder sb = new StringBuilder();
            List<ReportBO> resultList = TempData["PostProductionRejectionReportDataTemp"] as List<ReportBO>;

            if (resultList.Count < 0)
                return RedirectToAction("PostProductionRejectionReport", "Report");

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = ApplicationSession.ORGANISATIONADDRESS;
            string ReportName = "Post Production Rejection Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:-90px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "From Date :" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr > <th></th>");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:-170px;padding-bottom:-290px;font-size:11px;font-family:Times New Roman;'>" + "To Date :" + " " + toDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center;' Colspan='1'>" +
                "<img height='150' width='150' src='" + strPath + "'/></th>");
            sb.Append("<th Colspan='12' style='text-align:center;font-size:22px;padding-bottom:2px;padding-right:40px'>");
            //sb.Append("<br/>");
            sb.Append("<label style='font-size:22px; text-color:red bottom:20px;font-family:Times New Roman;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Times New Roman;'>" + name + "</label>");
            //sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:11px;font-family:Times New Roman;'>" + address + "</label>");

            sb.Append("</th></tr>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:12px;border: 0.05px  #e2e9f3;width:50px;'>Sr.No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:20%;font-size:13px;border: 0.05px  #e2e9f3;'>RNDate</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:17%;font-size:13px;border: 0.05px  #e2e9f3;'>WO Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:17%;font-size:13px;border: 0.05px  #e2e9f3;'>Batch Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:18%;font-size:13px;border: 0.05px  #e2e9f3;'>Post-Production RN Number</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Stage</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Name</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Code</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Item Unit Price (Rs)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:17%;font-size:13px;border: 0.05px  #e2e9f3;'>Total Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:17%;font-size:13px;border: 0.05px  #e2e9f3;'>Rejected Quantity (KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:17%;font-size:13px;border: 0.05px  #e2e9f3;'>Remarks</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:19%;font-size:13px;border: 0.05px  #e2e9f3;'>Approved By</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            resultList.Count();
            //stockReport.r
            foreach (var item in resultList)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RNDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WorkOrderNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BatchNumber + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PostProductionRejectionNoteNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Stage + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ItemUnitPrice + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TotalQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.RejectedQty + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Remarks + "</td>");
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
                    string filename = "Rpt_Rejection_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }


        #endregion

        #region Excel Post Production Rejection Report
        public void ExportAsExcelForPostProjectionRejection()
        {
            GridView gv = new GridView();
            DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            DateTime toDate = Convert.ToDateTime(Session["ToDate"]);
            string BatchNumber = Convert.ToString(Session["BatchNumber"]);
            string WorkOrderNumber = Convert.ToString(Session["WorkOrderNumber"]);


            List<ReportBO> resultList = _repository.getPostProductionRejectionReportData(fromDate, toDate, BatchNumber, WorkOrderNumber);
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("RN Date");
            dt.Columns.Add("WO Number");
            dt.Columns.Add("Batch Number");
            dt.Columns.Add("Post-Production RN Number");
            dt.Columns.Add("Stage");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Unit Price (Rs)");
            dt.Columns.Add("Total Quantity (KG)");
            dt.Columns.Add("Rejected Quantity (KG)");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("Approved By");

            foreach (ReportBO st in resultList)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = st.SrNo.ToString();
                dr["RN Date"] = st.RNDate.ToString();
                dr["WO Number"] = st.WorkOrderNumber.ToString();
                dr["Batch Number"] = st.BatchNumber.ToString();
                dr["Post-Production RN Number"] = st.PostProductionRejectionNoteNo.ToString();
                dr["Stage"] = st.Stage.ToString();
                dr["Item Name"] = st.ItemName.ToString();
                dr["Item Code"] = st.ItemCode.ToString();
                dr["Item Unit Price (Rs)"] = st.ItemUnitPrice.ToString();
                dr["Total Quantity (KG)"] = st.TotalQty.ToString();
                dr["Rejected Quantity (KG)"] = st.RejectedQty.ToString();
                dr["Remarks"] = st.Remarks.ToString();
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
            string filename = "Rpt_Post_Production_Rejection_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Post Production Rejection Report";/* The Stock Movement Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            String content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;font-family:Times New Roman;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + name + "</td></tr>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-weight:bold;font-family:Times New Roman;'>" + address + "</td></tr>"
               + "<tr><td colspan='7' style='text-align:left; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Fromdate + fromdate
               + "</td><td colspan='6' style='text-align:right; font-size:15px;font-weight:bold;font-family:Times New Roman;'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";


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

        #region Bind dropdown

        public void BindWorkOrderNumberDropDown()
        {
            var model = _repository.Getall();
            var WorkOrderNumberdd = new SelectList(model.ToList(), "ID", "WorkOrderNumber");
            ViewData["WorkOrderNumberdd"] = WorkOrderNumberdd;
        }
        public void BindSONumberDropDown()
        {
            var model = _repository.GetSONumber();
            var SONumberdd = new SelectList(model.ToList(), "SONumber", "SONumber");
            ViewData["SONumberdd"] = SONumberdd;

        }

        public void BindDBNoteNumberDropDown()
        {
            var model = _repository.GetDebitNoteNumber();
            var DBNoteNumberdd = new SelectList(model.ToList(), "ID", "DebitNoteNo");
            ViewData["DBNoteNumberdd"] = DBNoteNumberdd;

        }

        /// <summary>
        /// Rahul 07-04-23. 
        /// Bind Inward PONo. for Wastage report.  
        /// </summary>
        public void BindInwardPODropDown()
        {
            var model = _purchaseOrderRepository.GetAll();
            var WastageReport_dd = new SelectList(model.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberdd"] = WastageReport_dd;
        }

        /// <summary>
        /// Rahul 12-04-23. 
        /// Bind WONo. for Pre-production QC report.  
        /// </summary>
        public void BindPreProduction_QCWODropDown()
        {
            var model = _reportRepository.Getall();
            var WastageReport_dd = new SelectList(model.ToList(), "ID", "WorkOrderNumber");
            ViewData["WONumberdd"] = WastageReport_dd;
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