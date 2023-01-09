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
            sb.Append("<th  style='text-align:right;padding-right:50px;padding-bottom:-290px;font-size:11px;'>" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:40px;padding-bottom:-290px;font-size:11px;'>" + " " + toDate.ToString("dd/MM/yyyy"));
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
                dr["PO Date"] = st.PurchaseOrderDate.ToString(); //Rahul updated PurchaseOrderDate 06-01-2023. 
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
                return View("Index");
            }

            StringBuilder sb = new StringBuilder();
            List<GRN_BO> RawMaterialReceived = TempData["RawMaterialReport"] as List<GRN_BO>;

            if (RawMaterialReceived.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string address = "SR NO 673, OPP SURYA GATE, Gana Rd, Karamsad, Gujarat 388325";
            string ReportName = "Raw Material Received Report";
            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:2px; padding-left:10px;padding-right:10px;padding-bottom:-9px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;'>");
            sb.Append("<thead>");
            sb.Append("<tr >");
            sb.Append("<th  style='text-align:right;padding-right:50px;padding-bottom:-290px;font-size:11px;'>" + " " + fromDate.ToString("dd/MM/yyyy"));
            sb.Append("</th></tr>");
            sb.Append("<tr >");
            sb.Append("<th colspan=9 style='text-align:right;padding-right:20px;padding-bottom:-290px;font-size:11px;'>" + " " + toDate.ToString("dd/MM/yyyy"));
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
                    string filename = "RPT_Raw_Material_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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