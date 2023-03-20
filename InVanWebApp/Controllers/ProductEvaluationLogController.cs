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
    public class ProductEvaluationLogController : Controller
    {
        private IProductEvaluationLogRepository _productEvaluationLogRepository;
        private static ILog log = LogManager.GetLogger(typeof(ProductEvaluationLogController));

        #region Initializing constructor
        /// <summary>
        /// Date: 14/03/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public ProductEvaluationLogController()
        {
            _productEvaluationLogRepository = new ProductEvaluationLogRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 14/03/2023
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
              //  var model = _productEvaluationLogRepository.GetAll();
                //return View(model);
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Snehal on 14 March' 2023
        /// Calling method for ProductEvaluationLog Data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllProductEvaluationLogList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            var dailyMonitoringBOs = _productEvaluationLogRepository.GetAllProductEvaluationLogList(flagdate, fromDate, toDate);
            TempData["ProductEvaluationLogPDF"] = dailyMonitoringBOs;
            TempData["ProductEvaluationLogExcel"] = dailyMonitoringBOs;
            return Json(new { data = dailyMonitoringBOs }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 14 March' 2023
        /// Snehal: Rendered the user to the add ProductEvaluationLog form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProductEvaluationLog()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ProductEvaluationLogBO model = new ProductEvaluationLogBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.PELDate = DateTime.Today;
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
        public ActionResult AddProductEvaluationLog(ProductEvaluationLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    ModelState.Remove("PELDateAfter7Days");
                    ModelState.Remove("PhAfter7Days");
                    ModelState.Remove("TexColTasteAfter7Days");
                    ModelState.Remove("AcidAfter7Days");
                    ModelState.Remove("SaltAfter7Days");
                    ModelState.Remove("ViscosityAfter7Days");

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        //ModelState.Remove("PELDateAfter7Days");
                        //ModelState.Remove("PhAfter7Days");
                        //ModelState.Remove("TexColTasteAfter7Days");
                        //ModelState.Remove("AcidAfter7Days");
                        //ModelState.Remove("SaltAfter7Days");
                        //ModelState.Remove("ViscosityAfter7Days");
                        response = _productEvaluationLogRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Product Evaluation Log Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "ProductEvaluationLog");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "ProductEvaluationLog");
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
        ///Date: 14 March' 2023
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductEvaluationLog(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ProductEvaluationLogBO model = new ProductEvaluationLogBO();
                model = _productEvaluationLogRepository.GetById(Id);
                if(model.PELDateAfter7Days==null)
                    model.PELDateAfter7Days = DateTime.Today.AddDays(7);
                //model.Status = ;
                
                
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 14 March'23
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductEvaluationLog(ProductEvaluationLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //ModelState.Remove("CreatedBy");
                //ModelState.Remove("CreatedDate");
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productEvaluationLogRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Product Evaluation Log Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "ProductEvaluationLog");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "ProductEvaluationLog");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 14 March' 2023
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
                _productEvaluationLogRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "ProductEvaluationLog");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Excel Product Evaluation Log
        //public void ExportAsExcel()
        public ActionResult ExportAsExcel()
        {
            if (TempData["ProductEvaluationLogExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<ProductEvaluationLogBO> productEvaluationLog  = TempData["ProductEvaluationLogExcel"] as List<ProductEvaluationLogBO>;
            DataTable dt = new DataTable();
            dt.Columns.Add("Date");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Batch Code");
            dt.Columns.Add("Ph");
            dt.Columns.Add("Tex, Col & Taste");
            dt.Columns.Add("Acid");
            dt.Columns.Add("Salt");
            dt.Columns.Add("Viscosity");
            dt.Columns.Add("Date after 7 days");
            dt.Columns.Add("Ph after 7 days");
            dt.Columns.Add("Tex, Col & Taste ");
            dt.Columns.Add("Acid ");
            dt.Columns.Add("Salt ");
            dt.Columns.Add("Viscosity ");
            dt.Columns.Add("Work Order");
            dt.Columns.Add("Status");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Verify By");


            foreach (ProductEvaluationLogBO st in productEvaluationLog )
            {
                DataRow dr = dt.NewRow();
                dr["Date"] = st.PELDate.ToString();
                dr["Product Name"] = st.ProductName.ToString();
                dr["Batch Code"] = st.BatchCode.ToString();
                dr["Ph"] = st.Ph.ToString();
                dr["Tex, Col & Taste"] = st.TexColTaste.ToString();
                dr["Acid"] = st.Acid.ToString();
                dr["Salt"] = st.Salt.ToString();
                dr["Viscosity"] = st.Viscosity.ToString();
                dr["Date after 7 days"] = st.PELDateAfter7Days == null ? "" : st.PELDateAfter7Days.ToString(); 
                dr["Ph after 7 days"] = st.PhAfter7Days == null ? "" : st.PhAfter7Days.ToString(); 
                dr["Tex, Col & Taste "] = st.TexColTasteAfter7Days == null ? "" : st.TexColTasteAfter7Days.ToString();
                dr["Acid "] = st.AcidAfter7Days == null ? "" : st.AcidAfter7Days.ToString();
                dr["Salt "] = st.SaltAfter7Days == null ? "" : st.SaltAfter7Days.ToString();
                dr["Viscosity "] = st.ViscosityAfter7Days == null ? "" : st.ViscosityAfter7Days.ToString();
                dr["Work Order"] = st.WorkOrder==null?"": st.WorkOrder.ToString();
                dr["Status"] = st.Status.ToString();
                dr["Remark"] = st.Remark.ToString();
                dr["Verify By"] = st.VerifyByName.ToString();

                dt.Rows.Add(dr);
            }
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string filename = "Rpt_Product_Evaluation_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Product Evaluation Report";/* The Daily Monitoring Report name are given here  */
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

        #region Export PDF Product Evaluation Log
        /// <summary>
        /// Create by Snehal on 14/03/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            if (TempData["ProductEvaluationLogPDF"] == null)
            {
                return View("Index");
            }
            StringBuilder sb = new StringBuilder();
            List<ProductEvaluationLogBO> productEvaluationLog = TempData["ProductEvaluationLogPDF"] as List<ProductEvaluationLogBO>;

            if (productEvaluationLog .Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Product Evaluation Report";
            string Fromdate = "From Date : ";
            string Todate = "To Date:";
            string fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");
            if (fromdate=="01-01-0001")
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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Product Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Batch Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Ph</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Tex, Col & Taste</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Acid</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Salt</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Viscosity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Ph</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Tex, Col & Taste</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Acid</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Salt</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Viscosity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Work Order</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Status</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Remark</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            productEvaluationLog.Count();
            foreach (var item in productEvaluationLog )
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PELDate + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BatchCode + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Ph + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TexColTaste + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Acid + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Salt + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Viscosity + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PELDateAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PhAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TexColTasteAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.AcidAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SaltAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ViscosityAfter7Days + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WorkOrder + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Status + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Remark + "</td>");
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
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_Product_Evaluation_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
