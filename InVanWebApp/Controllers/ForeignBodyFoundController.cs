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
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace InVanWebApp.Controllers
{
    public class ForeignBodyFoundController : Controller
    {
        private IForeignBodyFoundRepository _foreignBodyFoundRepository;
        private static ILog log = LogManager.GetLogger(typeof(ForeignBodyFoundController));

        #region Initializing constructor
        /// <summary>
        /// Date: 17/03/2023
        /// Yatri: Constructor without parameter
        /// </summary>
        public ForeignBodyFoundController()
        {
            _foreignBodyFoundRepository = new ForeignBodyFoundRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 17/03/2023
        ///Yatri: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _foreignBodyFoundRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 17 March'23
        /// Yatri: Rendered the user to the add  ForeignBodyFound form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddForeignBodyFound()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ForeignBodyFoundBO model = new ForeignBodyFoundBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                //model.Time= DateTime.Now.ToString("HH:mm:ss tt");
                //model.fromDate = DateTime.Now;
                //model.toDate = DateTime.Now;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Yatri: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddForeignBodyFound(ForeignBodyFoundBO model)
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
                        response = _foreignBodyFoundRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Foreign Body Found Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "ForeignBodyFound");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "ForeignBodyFound");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Yatri: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditForeignBodyFound(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ForeignBodyFoundBO model = _foreignBodyFoundRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 17 March'23
        /// Yatri:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditForeignBodyFound(ForeignBodyFoundBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _foreignBodyFoundRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Foreign Body Found Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "ForeignBodyFound");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "ForeignBodyFound");
                }


            }


                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 18 March'23
        /// Yatri: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _foreignBodyFoundRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "ForeignBodyFound");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Yatri on 18 March'23
        /// Calling method for ForeignBodyFound data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllForeignBodyFoundList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;

            var foreignBodyFoundBOs = _foreignBodyFoundRepository.GetAllForeignBodyFoundList(flagdate, FromDate, ToDate);
            TempData["ForeignBodyFoundPDF"] = foreignBodyFoundBOs;
            TempData["ForeignBodyFoundExcel"] = foreignBodyFoundBOs;
            return Json(new { data = foreignBodyFoundBOs }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Excel ForeignBodyFound
        //public void ExportAsExcel()
        public ActionResult ExportAsExcel()
        {
            if (TempData["ForeignBodyFoundExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<ForeignBodyFoundBO> foreignBodyFound = TempData["ForeignBodyFoundExcel"] as List<ForeignBodyFoundBO>;
            DataTable dt = new DataTable();

            dt.Columns.Add("Date");
            dt.Columns.Add("Raw Material");
            dt.Columns.Add("On Going Processing");
            dt.Columns.Add("Batching");
            dt.Columns.Add("Post Processing");
            dt.Columns.Add("Corrective Action");
            dt.Columns.Add("Verify By");
            dt.Columns.Add("Remark");

            int i = 1;
            foreach (ForeignBodyFoundBO st in foreignBodyFound)
            {
                DataRow dr = dt.NewRow();
                //dr["Sr.No"] = i;
                //dr["Date"] = st.Date.ToString(); //Rahul removed 'st.Date.ToString();' not in use 19-08-23.
                dr["Date"] = Convert.ToDateTime(st.Date).ToString("dd/MM/yyyy"); ///Rahul added 'Convert.ToDateTime(st.Date).ToString("dd/MM/yyyy");'  19-08-23.              
                dr["Raw Material"] = st.RawMaterial.ToString();
                dr["On Going Processing"] = st.OnGoingProcessing.ToString();
                dr["Batching"] = st.Batching.ToString();
                dr["Post Processing"] = st.PostProcessing.ToString();
                dr["Corrective Action"] = st.CorrectiveAction.ToString();
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
            string filename = "Rpt_ForeignBodyFound_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Foreign Body Found Report";/* The Daily Monitoring Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["ToDate"]).ToString("dd/MM/yyyy");

            if (fromdate == "01-01-0001")
            {
                fromdate = "";
                Fromdate = "From Date : " + DateTime.Today.ToString("dd/MM/yyyy");/*old Code : Fromdate = " "*/
            }
            if (todate == "01-01-0001")
            {
                todate = "";
                Todate = "To Date : " + DateTime.Today.ToString("dd/MM/yyyy");/*old Code : Todate*/
            }


            String content1 = "";
            if (fromdate == "" || fromdate == null && todate == "" || todate == null)
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
              "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
              + "<tr><td colspan='4' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='4' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='4' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='4' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + DateTime.Today.ToString("dd/MM/yyyy")
               + "</td><td colspan='4' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + DateTime.Today.ToString("dd/MM/yyyy")
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

        #region Export PDF ForeignBodyFound
        /// <summary>
        /// Create by Yatri on 18/03/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {

            if (TempData["ForeignBodyFoundPDF"] == null)
            {
                return View("Index");
            }
            StringBuilder sb = new StringBuilder();
            List<ForeignBodyFoundBO> foreignBodyFound = TempData["ForeignBodyFoundPDF"] as List<ForeignBodyFoundBO>;

            if (foreignBodyFound.Count < 0)
                return View("Index");
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Foreign Body Found Report";
            string Fromdate = "From Date : ";
            string Todate = "To Date :";
            string fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["toDate"]).ToString("dd/MM/yyyy");

            if (fromdate == "01-01-0001")
            {
                fromdate = "";
                Fromdate = "From Date : " + DateTime.Today.ToString("dd/MM/yyyy");/*old Code : Fromdate = " "*/
            }

            if (todate == "01-01-0001")
            {
                todate = "";
                Todate = "To Date : " + DateTime.Today.ToString("dd/MM/yyyy");/*old Code : Todate*/
            }

            string name = ApplicationSession.ORGANISATIONTIITLE;
            string address = ApplicationSession.ORGANISATIONADDRESS;
            sb.Append("<div style='padding-top:20px; padding-left:10px;padding-right:10px;padding-bottom:20px; vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: No;page-break-inside: auto;'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left'>" + "<img height='120' width='160' src='" + strPath + "'/></th>");
            sb.Append("<th colspan='5' style='text-align:center'>");
            //sb.Append("<label style='font-size:22px;font-family:Verdana;text-align:center; color:#0e3f6f'>" + ReportName + "</label>");
            sb.Append("<label style='font-size:22px; bottom:20px;font-weight:bold;color:Red;'>" + ReportName + "</label>");
            sb.Append("<br/>");
            sb.Append("<br/><label style='font-size:14px;font-family:Arial'>" + name + "</label>");
            sb.Append("<br/><label style='font-size:10px;font-family:Arial'>" + address + "</label>");
            sb.Append("</th></tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=4 style='text-align:left;font-size:11px;padding-bottom:3px;'>" + Fromdate + " " + fromdate);
            sb.Append("</th>");
            sb.Append("<th colspan=5 style='text-align:right;font-size:11px;'>" + Todate + " " + todate);
            sb.Append("</th></tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");

            sb.Append("<tr style='text-align:center;padding: 1px; font-family:Times New Roman;background-color:#dedede'>");
            sb.Append("<th style='text-align:center;padding: 3px; font-family:Times New Roman;width:1%;;font-size:13px;border: 0.05px  #e2e9f3;'>Sr.No.</th>");
            sb.Append("<th style='text-align:center;padding: 10px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Date</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:4%;;font-size:13px;border: 0.05px  #e2e9f3;'>Raw Material</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>On Going Processing</th>");
            sb.Append("<th style='text-align:center;padding: 3px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Batching</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;;font-size:13px;border: 0.05px  #e2e9f3;'>Post Processing</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;;font-size:13px;border: 0.05px  #e2e9f3;'>Corrective Action</th>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Verify By</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;;font-size:13px;border: 0.05px  #e2e9f3;'>Remark</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreignBodyFound.Count();
            //stockReport.r
            int i = 1;
            foreach (var item in foreignBodyFound)
            {
                sb.Append("<tr style='text-align:center;padding: 10px;'>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:1%;'>" + i + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:3%;'>" + Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy") + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:4%;'>" + item.RawMaterial + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.OnGoingProcessing + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.Batching + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:5%;'>" + item.PostProcessing + "</td>");
                sb.Append("<td style='text-align:center;padding: 10px;border: 0.01px #e2e9f3;font-size:11px; font-family:Times New Roman;width:2%;'>" + item.CorrectiveAction + "</td>");

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
                    //pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "RPT_FoerignBodyFound_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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