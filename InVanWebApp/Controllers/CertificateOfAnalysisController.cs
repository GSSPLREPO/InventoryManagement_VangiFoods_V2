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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;

namespace InVanWebApp.Controllers
{
    public class CertificateOfAnalysisController : Controller
    {

        private ICertificateOfAnalysisRepository _CertificateOfAnalysisRepository;
        private static ILog log = LogManager.GetLogger(typeof(CertificateOfAnalysisController));
        string FileNameLabReport;
        #region Initializing constructor
        /// <summary>
        /// Date: 20/03/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public CertificateOfAnalysisController()
        {
            _CertificateOfAnalysisRepository = new CertificateOfAnalysisRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 20/03/2023
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _CertificateOfAnalysisRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 20/03/2023
        /// Snehal: Rendered the user to the add CertificateOfAnalysis form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCertificateOfAnalysis()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CertificateOfAnalysisBO model = new CertificateOfAnalysisBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                model.BestBeforeDate = DateTime.Today;
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
        public ActionResult AddCertificateOfAnalysis(CertificateOfAnalysisBO model, HttpPostedFileBase LabAnalysisReport)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (LabAnalysisReport != null)
                    {
                        UploadLabAnalysisReport(LabAnalysisReport);
                        model.LabAnalysisReport = FileNameLabReport.ToString();
                    }
                    else
                        model.LabAnalysisReport = null;
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                        response = _CertificateOfAnalysisRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Certificate of Analysis Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('COA is done for the entered batch number! Cannot perform insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "CertificateOfAnalysis");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "CertificateOfAnalysis");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Snehal: Rendered the user to the edit page with details of a particular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCertificateOfAnalysis(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CertificateOfAnalysisBO model = _CertificateOfAnalysisRepository.GetById(Id);
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 20-03-2023
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCertificateOfAnalysis(CertificateOfAnalysisBO model, HttpPostedFileBase LabAnalysisReport)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                //if (LabAnalysisReport != null)
                //{
                //    UploadLabAnalysisReport(LabAnalysisReport);
                //    model.LabAnalysisReport = FileNameLabReport.ToString();
                //}
                //else
                //    model.LabAnalysisReport = null;


                try
                {
                    //if (LabAnalysisReport != null && LabAnalysisReport.ContentLength > 1000)
                    //{
                    //    UploadLabAnalysisReport(LabAnalysisReport);
                    //    model.LabAnalysisReport = FileNameLabReport.ToString();
                    //}
                    //else if (LabAnalysisReport.ContentLength < 1000 && LabAnalysisReport != null)
                    //    model.LabAnalysisReport = LabAnalysisReport.FileName.ToString();
                    //else
                    //    model.LabAnalysisReport = null;

                    if (LabAnalysisReport != null)
                    {
                        UploadLabAnalysisReport(LabAnalysisReport);
                        model.LabAnalysisReport = FileNameLabReport.ToString();
                    }
                    else
                    {
                        model.LabAnalysisReportNo = null;
                        model.LabAnalysisReport = null;
                    }

                    if (ModelState.IsValid)
                    {
                            model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.Date = DateTime.Today;
                        response = _CertificateOfAnalysisRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Certificate of Analysis Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate batch number! Cannot perform update!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "CertificateOfAnalysis");
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "CertificateOfAnalysis");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 20-03-2023
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
                _CertificateOfAnalysisRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "CertificateOfAnalysis");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Function for Uploading the Lab Analysis Report
        /// <summary>
        /// Date: 20-03-2023
        /// Snehal: Upload Lab Analysis Report.
        /// </summary>
        /// <returns></returns>

        public void UploadLabAnalysisReport(HttpPostedFileBase LabAnalysisReport)
        {
            string path = Server.MapPath("~/LabAnalysisReport/");
            //string FileName= Path.GetFileNameWithoutExtension(LabAnalysisReport.FileName.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (LabAnalysisReport != null)
            {
                
                var LabFileName = Path.GetFileNameWithoutExtension(LabAnalysisReport.FileName);
                var Extension = Path.GetExtension(LabAnalysisReport.FileName);
                var LabFileNameNew = LabFileName +"_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + Extension;
                string Filename = LabFileNameNew;
                FileNameLabReport = LabFileNameNew;
                Filename = Path.Combine(Server.MapPath("~/LabAnalysisReport/"), Filename);
                LabAnalysisReport.SaveAs(Filename);
            }
        }

        #endregion
        /* Report */
        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Snehal on 20-03-2023
        /// Calling method for COA data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCertificateOfAnalysisList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;
            var CertificateOfAnalysisBO = _CertificateOfAnalysisRepository.GetAllCertificateOfAnalysisList(flagdate, fromDate, toDate);
            TempData["CertificateOfAnalysisPDF"] = CertificateOfAnalysisBO;
            TempData["CertificateOfAnalysisExcel"] = CertificateOfAnalysisBO;
            return Json(new { data = CertificateOfAnalysisBO }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export PDF COA
        /// <summary>
        /// Create by Snehal on 20/03/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult CertificateOfAnalysisPDF()
        {
            if (TempData["CertificateOfAnalysisPDF"] == null)
            {
                return View("Index");
            }
            StringBuilder sb = new StringBuilder();
            List<CertificateOfAnalysisBO> certificateOfAnalysis = TempData["CertificateOfAnalysisPDF"] as List<CertificateOfAnalysisBO>;

            if (certificateOfAnalysis.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Certificate of Analysis Report";
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
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:12%;font-size:13px;border: 0.05px  #e2e9f3;width:50px;'>Sr. No.</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Source</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>WO/PO</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Product Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Batch No</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Packing Size</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Best Before</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Clostridium Perfringens(/100gm)</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Escherichia Coli (/gm)</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Salmonella(/25gm0) L= ABSENT</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Total Plate Count (cfu/gm) L=1 x 10³</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Yeast & Mould(cfu/gm) L=1 x 10^2</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Coliform (cfu/gm)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Verify By Name</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Escherichia Coli (/gm)</th>");
            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:15%;font-size:13px;border: 0.05px  #e2e9f3;'>Escherichia Coli (/gm)</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            certificateOfAnalysis.Count();
            //stockReport.r
            foreach (var item in certificateOfAnalysis)
            {

                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.SrNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Date + "</td>");  //Rahul updated PurchaseOrderDate 06-01-2023. 
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Source + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.WOPO + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ProductName + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BatchNo + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.PackingSize + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.BestBeforeDate + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.ClostridiumPerfringens + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.EscherichiaColi + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Salmonella + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.TotalPlateCountNumber + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.YeastandMould + "</td>");
                //sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.Coliform + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;'>" + item.VerifyByName + "</td>");

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

        #region Excel Certificate of Analysis
        public ActionResult CertificateOfAnalysisExcel()
        {
            if (TempData["CertificateOfAnalysisExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<CertificateOfAnalysisBO> certificateOfAnalysis = TempData["CertificateOfAnalysisExcel"] as List<CertificateOfAnalysisBO>;
            DataTable dt = new DataTable();
            //dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Source");
            dt.Columns.Add("WO/PO");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Batch No");
            dt.Columns.Add("Packing Size");
            dt.Columns.Add("Best Before");
            dt.Columns.Add("Clostridium Perfringens(/100gm)");
            dt.Columns.Add("Escherichia Coli (/gm)");
            dt.Columns.Add("Salmonella(/25gm0) L= ABSENT");
            dt.Columns.Add("Total Plate Count (cfu/gm) L=1 x 10³");
            dt.Columns.Add("Yeast & Mould(cfu/gm) L=1 x 10^2");
            dt.Columns.Add("Coliform (cfu/gm)");
            dt.Columns.Add("Acidity (%)");
            dt.Columns.Add("PH");
            dt.Columns.Add("Total Soluble Solids (%)");
            dt.Columns.Add("Peroxide Value (meq/kg)");
            dt.Columns.Add("Salt Content (%)");
            dt.Columns.Add("Lab Analysis Report No");
            dt.Columns.Add("Lab Analysis Report");
            dt.Columns.Add("Verify By Name");
            

            int i = 1;
            foreach (CertificateOfAnalysisBO st in certificateOfAnalysis)
            {
                DataRow dr = dt.NewRow();
               // dr["Sr.No"] = i;
                dr["Date"] = st.Date.ToString();
                dr["Source"] = st.Source.ToString();
                dr["WO/PO"] = st.WOPO.ToString();
                dr["Product Name"] = st.ProductName.ToString();
                dr["Batch No"] = st.BatchNo.ToString();
                dr["Packing Size"] = st.PackingSize.ToString();
                dr["Best Before"] = st.BestBeforeDate.ToString();
                dr["Clostridium Perfringens(/100gm)"] = st.ClostridiumPerfringens.ToString();
                dr["Escherichia Coli (/gm)"] = st.EscherichiaColi.ToString();
                dr["Salmonella(/25gm0) L= ABSENT"] = st.Salmonella.ToString();
                dr["Total Plate Count (cfu/gm) L=1 x 10³"] = st.TotalPlateCountNumber.ToString();
                dr["Yeast & Mould(cfu/gm) L=1 x 10^2"] = st.YeastandMould.ToString();
                dr["Coliform (cfu/gm)"] = st.Coliform.ToString();
                dr["Acidity (%)"] = st.Acidity.ToString();
                dr["PH"] = st.PH.ToString();
                dr["Total Soluble Solids (%)"] = st.TotalSolubleSolids.ToString();
                dr["Peroxide Value (meq/kg)"] = st.PeroxideValue.ToString();
                dr["Salt Content (%)"] = st.SaltContent.ToString();
                dr["Lab Analysis Report No"] = st.LabAnalysisReportNo == null ? "" : st.LabAnalysisReportNo.ToString();
                dr["Lab Analysis Report"] = st.LabAnalysisReport == null ? "" : st.LabAnalysisReport.ToString();
                dr["Verify By Name"] = st.VerifyByName.ToString();
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
            string filename = "Rpt_Certificate_of_Analysis_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Certificate of Analysis Report";/* The CertificateOfAnalysis Report name are given here  */
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
              "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"+
              "<tr><td colspan='21' style='text-align:right'><span align='center' style='font-weight:bold'>" + "Date & Time: "+ DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</td></tr>"
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='19' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='11' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
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

    }
}