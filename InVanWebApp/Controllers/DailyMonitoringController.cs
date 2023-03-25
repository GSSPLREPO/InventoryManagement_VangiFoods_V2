using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using log4net;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace InVanWebApp.Controllers
{
    public class DailyMonitoringController : Controller
    {
        private IDailyMonitoringRepository _dailyMonitoringRepository;
        private static ILog log = LogManager.GetLogger(typeof(DailyMonitoringController));

        #region Initializing constructor
        /// <summary>
        /// Date: 22/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public DailyMonitoringController()
        {
            _dailyMonitoringRepository = new DailyMonitoringRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 22/02/2023
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //var model = _dailyMonitoringRepository.GetAll();
                //var model = _dailyMonitoringRepository.GetAll();
                //return View(model);
                //DailyMonitoringBO model = new DailyMonitoringBO();
                //model.fromDate = DateTime.Today;
                //model.toDate = DateTime.Today;
                //return View(model);
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Rendered the user to the add DailyMonitoring form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDailyMonitoring()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DailyMonitoringBO model = new DailyMonitoringBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Snehal: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDailyMonitoring(DailyMonitoringBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        response = _dailyMonitoringRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Daily Monitoring Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "DailyMonitoring");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "DailyMonitoring");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Snehal: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditDailyMonitoring(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DailyMonitoringBO model = _dailyMonitoringRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDailyMonitoring(DailyMonitoringBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _dailyMonitoringRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Daily Monitoring Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "DailyMonitoring");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "DailyMonitoring");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _dailyMonitoringRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "DailyMonitoring");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Snehal on 09 Feb'23
        /// Calling method for Daily Monitoring data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllDailyMonitoringList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //Session["FromDate"] = fromDate;
            //Session["ToDate"] = toDate;
            //var dailyMonitoringBOs = _dailyMonitoringRepository.GetAllDailyMonitoring(fromDate, toDate);
            //return Json(new { data = dailyMonitoringBOs }, JsonRequestBehavior.AllowGet);

            //DateTime Fromdate = new DateTime();
            //DateTime Todate = new DateTime();

            //DateTime? Fromdate = null;
            //DateTime? Todate = null;
            //if (fromDate != null && toDate != null)
            //{
            //    //Fromdate = DateTime.ParseExact(fromDate, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture); 
            //    //Todate = DateTime.ParseExact(toDate, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            //    //Fromdate = DateTime.ParseExact(fromDate, "yyyy/mm/dd", CultureInfo.InvariantCulture);
            //    //Todate = DateTime.ParseExact(toDate, "yyyy/mm/dd", CultureInfo.InvariantCulture);

            //    Fromdate = DateTime.ParseExact(fromDate, "yyyy/mm/dd", CultureInfo.InvariantCulture);
            //    Todate = DateTime.ParseExact(toDate, "yyyy/mm/dd", CultureInfo.InvariantCulture);
            //}
                

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;

            var dailyMonitoringBOs = _dailyMonitoringRepository.GetAllDailyMonitoringList(flagdate, fromDate, toDate);
            TempData["DailyMonitoringPDF"] = dailyMonitoringBOs;
            TempData["DailyMonitoringExcel"] = dailyMonitoringBOs;
            return Json(new { data = dailyMonitoringBOs }, JsonRequestBehavior.AllowGet);
         
        }

        #endregion

        #region Excel Daily Monitoring
        //public void ExportAsExcel()
        public ActionResult ExportAsExcel()
        {
            if (TempData["DailyMonitoringExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<DailyMonitoringBO> dailyMonitoring = TempData["DailyMonitoringExcel"] as List<DailyMonitoringBO>;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Personal Hygiene");
            dt.Columns.Add("Cleaning And Sanitation");
            dt.Columns.Add("Cleaning Of Equipment");
            dt.Columns.Add("Water Potability");
            dt.Columns.Add("Allergic");
            dt.Columns.Add("Non Allergic");
            dt.Columns.Add("Vegetable Processing Area");
            dt.Columns.Add("Packaging & Labelling Area");
            dt.Columns.Add("Fgs Area");
            dt.Columns.Add("Inside");
            dt.Columns.Add("Out Side");
            dt.Columns.Add("Dry");
            dt.Columns.Add("Wet");
            dt.Columns.Add("Out Siders");
            dt.Columns.Add("Production Area");
            dt.Columns.Add("Office Staff");
            dt.Columns.Add("Verify By");
            dt.Columns.Add("Remark");

            int i = 1;
            foreach (DailyMonitoringBO st in dailyMonitoring)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["Date"] = st.Date.ToString();
                dr["Personal Hygiene"] = st.PersonalHygine.ToString();
                dr["Cleaning And Sanitation"] = st.CleaningAndSanitation.ToString();
                dr["Cleaning Of Equipment"] = st.CleaningOfEquipment.ToString();
                dr["Water Potability"] = st.WaterPotability.ToString();
                dr["Allergic"] = st.Allergic.ToString();
                dr["Non Allergic"] = st.NonAllergic.ToString();
                dr["Vegetable Processing Area"] = st.VegetableProcessingArea.ToString();
                dr["Packaging & Labelling Area"] = st.PackagingLabellingArea.ToString();
                dr["Fgs Area"] = st.FgsArea.ToString();
                dr["Inside"] = st.Inside.ToString();
                dr["Out Side"] = st.OutSide.ToString();
                dr["Dry"] = st.Dry.ToString();
                dr["Wet"] = st.Wet.ToString();
                dr["Out Siders"] = st.OutSiders.ToString();
                dr["Production Area"] = st.ProductionArea.ToString();
                dr["Office Staff"] = st.OfficeStaff.ToString();
                dr["Verify By"] = st.VerifyByName.ToString();
                dr["Remark"] = st.Remark.ToString();


                dt.Rows.Add(dr);
                i++;
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Daily_Monitoring_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Daily Monitoring Report";/* The Daily Monitoring Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            if (fromdate == "01-01-0001")
            {
                fromdate = "";
            }
            if (todate == "01-01-0001")
            {
                todate = "";
            }
            //String content1 = "<table>" + "<tr><td colspan='2' rowspan='3'> <img height='150' width='150' src='" + strPath + "'/></td>" +
            //    "<tr><td colspan='4'> <span align='center' style='font-size:25px;font-weight:bold;color:Red;'>&nbsp;" + ReportName + "</span></td></tr></tr>" +
            //    "<tr><td><td colspan='2'><span align='center' style='font-weight:bold'>" + name + "</span></td></tr>" +
            //    "<tr><td><td><td colspan='4'><span align='center' style='font-weight:bold'>" + address + "</span></td></td></td></tr>" +
            //    "<tr><tr><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Fromdate + fromdate
            //    + "<td><td><td></td><td Style='font-size:15px;Font-weight:bold;'>" + Todate + todate + "</td></td>"
            //    + "</td></tr>" + "</table>"
            //    + "<table><tr align='center'><td>" + sw.ToString() + "</tr></td></table>";

            String content1 = "";
            if (fromdate == "" || fromdate == null && todate == "" || todate == null)
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
              "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='18' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='10' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='10' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
               /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
               + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
           


            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(content1);
            gv.GridLines = GridLines.None;
            Response.Flush();
            Response.Clear();
            Response.End();

            return View("Index");
        }
        #endregion

        #region Export PDF Daily Monitoring
        /// <summary>
        /// Create by Snehal on 10/03/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            //DateTime fromDate = Convert.ToDateTime(Session["FromDate"]);
            //DateTime toDate = Convert.ToDateTime(Session["ToDate"]);

            //var status = Session["Status"].ToString();
            //var VendorId = Convert.ToInt32(Session["Vendor"]);
            //var DailyMonitoringReportDetails = GetAllDailyMonitoringList(fromDate, toDate);
            //var DailyMonitoringReportDetails = TempData["DailyMonitoringPDF"];
            //TempData["DailyMonitoringReportDataPDF"] = DailyMonitoringReportDetails;
            //if (TempData["DailyMonitoringReportDataPDF"] == null)
            //{
            //    return View("Index");
            //}
            if (TempData["DailyMonitoringPDF"] == null)
            {
                return View("Index");
            }
            StringBuilder sb = new StringBuilder();
            List<DailyMonitoringBO> dailyMonitoring = TempData["DailyMonitoringPDF"] as List<DailyMonitoringBO>;

            if (dailyMonitoring.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Daily Monitoring Report";
            string Fromdate = "From Date : ";
            string Todate = "To Date:";
            string fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            if (fromdate == "01-01-0001")
            {
                fromdate = "";
                Fromdate = "";
            }
            if (todate == "01-01-0001")
            {
                todate = "";
                Todate = "";
            }

            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: No;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='10' style='text-align:center'>");
            //sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center; color:#0e3f6f'>" + ReportName + "</label>");
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=10 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromdate);
            sb.Append("</th>");
            sb.Append("<th colspan=10 style='text-align:right;font-size:11px;'>" + Todate + " " + todate);
            sb.Append("</th></tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;;font-size:13px;border: 0.05px  #e2e9f3;'>Sr.No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:4%;;font-size:13px;border: 0.05px  #e2e9f3;'>Personal Hygiene</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Cleaning And Sanitation</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Cleaning Of Equipment</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Water Potability</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Allergic</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Non Allergic</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Vegetable Processing Area</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Packaging & Labelling Area</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Fgs Area</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>In Side</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Out Side</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Dry</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Wet</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Out Siders</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Production Area</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Office Staff</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Verify By</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Remark</th>");


            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            dailyMonitoring.Count();
            //stockReport.r
            int i = 1;
            foreach (var item in dailyMonitoring)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:1%;'>" + i + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.Date + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:4%;'>" + item.PersonalHygine + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.CleaningAndSanitation + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.CleaningOfEquipment + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.WaterPotability + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.Allergic + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.NonAllergic + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.VegetableProcessingArea + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.PackagingLabellingArea + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.FgsArea + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.Inside + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.OutSide + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.Dry + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.Wet + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.OutSiders + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.ProductionArea + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.OfficeStaff + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.VerifyByName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.Remark + "</td>");



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
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    //pdfDoc.SetPageSize(new Rectangle(850f, 1100f));
                    pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_Daily_Monitoring_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
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
