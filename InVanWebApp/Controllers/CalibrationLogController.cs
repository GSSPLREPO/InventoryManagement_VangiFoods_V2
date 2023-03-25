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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace InVanWebApp.Controllers
{
    public class CalibrationLogController : Controller
    {
        private ICalibrationLogRepository _calibrationLogRepository;
        private static ILog log = LogManager.GetLogger(typeof(CalibrationLogController));

        #region Initializing constructor
        /// <summary>
        /// Date: 20/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public CalibrationLogController()
        {
            _calibrationLogRepository = new CalibrationLogRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 20/02/2023
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _calibrationLogRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Rendered the user to the add CalibrationLog form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCalibrationLog()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CalibrationLogBO model = new CalibrationLogBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.CalibrationDoneDate = DateTime.Today;
                model.CalibrationDueDate = DateTime.Today;
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
        public ActionResult AddCalibrationLog(CalibrationLogBO model)
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
                        response = _calibrationLogRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Calibration Log Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "CalibrationLog");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "CalibrationLog");
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
        public ActionResult EditCalibrationLog(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CalibrationLogBO model = _calibrationLogRepository.GetById(Id);
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
        public ActionResult EditCalibrationLog(CalibrationLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _calibrationLogRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Calibration Log details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "CalibrationLog");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "CalibrationLog");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 20 Feb'23
        /// Snehal: Delete the perticular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _calibrationLogRepository.Delete(Id, userID);

                //_calibrationLogRepository.Delete(Id);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "CalibrationLog");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Report

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Snehal on 17 March '23
        /// Calling method for Calibration Log data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCalibrationLogList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;

            var calibrationLog = _calibrationLogRepository.GetAllCalibrationLogList(flagdate, fromDate, toDate);
            TempData["CalibrationLogPDF"] = calibrationLog;
            TempData["CalibrationLogExcel"] = calibrationLog;
            return Json(new { data = calibrationLog }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Excel Calibration Log
        //public void ExportAsExcel()
        public ActionResult ExportAsExcel()
        {
            if (TempData["CalibrationLogExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<CalibrationLogBO> calibrationLog = TempData["CalibrationLogExcel"] as List<CalibrationLogBO>;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Name Of Equipment");
            dt.Columns.Add("Id No");
            dt.Columns.Add("Department");
            dt.Columns.Add("Range");
            dt.Columns.Add("Range From");
            dt.Columns.Add("Range To");
            dt.Columns.Add("Frequency Of Calibration");
            dt.Columns.Add("Calibration Done Date");
            dt.Columns.Add("Calibration Due Date");
            dt.Columns.Add("Verify By");
            dt.Columns.Add("Remark");

            int i = 1;
            foreach (CalibrationLogBO st in calibrationLog)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["Name Of Equipment"] = st.NameOfEquipment.ToString();
                dr["Id No"] = st.IdNo.ToString();
                dr["Department"] = st.Department.ToString();
                dr["Range"] = st.Range.ToString();
                dr["Range From"] = st.RangeFrom.ToString();
                dr["Range To"] = st.RangeTo.ToString();
                dr["Frequency Of Calibration"] = st.FrequencyOfCalibration.ToString();
                dr["Calibration Done Date"] = st.CalibrationDoneDate.ToString();
                dr["Calibration Due Date"] = st.CalibrationDueDate.ToString();
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
            string filename = "Rpt_Equipment_Certification_Report_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Equipment Certification Report";/* The Calibration Log Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            if (fromdate == "01-01-0001")
            {
                fromdate = "";
                Fromdate = "From Date : " + DateTime.Today.ToString("dd/MM/yyyy");
            }

            if (todate == "01-01-0001")
            {
                todate = "";
                Todate = "To Date : " + DateTime.Today.ToString("dd/MM/yyyy");
            }

            String content1 = "";
            if (fromdate == "" || fromdate == null && todate == "" || todate == null)
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='6' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='8' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='6' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
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

        #region Export PDF Calibration Log
        /// <summary>
        /// Create by Snehal on 17/03/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {

            if (TempData["CalibrationLogPDF"] == null)
            {
                return View("Index");
            }
            StringBuilder sb = new StringBuilder();
            List<CalibrationLogBO> calibrationLog = TempData["CalibrationLogPDF"] as List<CalibrationLogBO>;

            if (calibrationLog.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Calibration Log Report";
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
            sb.Append("<label style='font-size:22px; bottom:20px;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=5 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromdate);
            sb.Append("</th>");
            sb.Append("<th colspan=5 style='text-align:right;font-size:11px;'>" + Todate + " " + todate);
            sb.Append("</th></tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");


            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;;font-size:13px;border: 0.05px  #e2e9f3;'>Sr.No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Name Of Equipment</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Id No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Department</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Range</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Range From</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Range To</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Frequency Of Calibration</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Calibration Done Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Calibration Due Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Verify By</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Remark</th>");


            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            calibrationLog.Count();
            //stockReport.r
            int i = 1;
            foreach (var item in calibrationLog)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + i + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:6%;'>" + item.NameOfEquipment + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + item.IdNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.Department + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.Range + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.RangeFrom + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.RangeTo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.FrequencyOfCalibration + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.CalibrationDoneDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.CalibrationDueDate + "</td>");
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
                    //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(850f, 1100f));
                    // pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_Calibration_Log_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
            var pageBorderRect = new iTextSharp.text.Rectangle(pdfDoc.PageSize);

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

        #endregion
    }
}