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
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace InVanWebApp.Controllers
{
    public class PestControlLogController : Controller
    {
        private IPestControlLogRepository _pestControlLogRepository;
        private static ILog log = LogManager.GetLogger(typeof(PestControlLogController));

        #region Initializing constructor
        /// <summary>
        /// Date: 28/02/2023
        /// Yatri: Constructor without parameter
        /// </summary>
        public PestControlLogController()
        {
            _pestControlLogRepository = new PestControlLogRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 28/02/2023
        ///Yatri: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _pestControlLogRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 28 Feb'23
        /// Yatri: Rendered the user to the add PestControlLog form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPestControlLog()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PestControlLogBO model = new PestControlLogBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                //model.Time = DateTime.Now.ToString("HH:mm:ss tt");
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
        public ActionResult AddPestControlLog(PestControlLogBO model)
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
                        response = _pestControlLogRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Pest Control Log Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "PestControlLog");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "PestControlLog");
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
        public ActionResult EditPestControlLog(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PestControlLogBO model = _pestControlLogRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 28 Feb'23
        /// Yatri:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPestControlLog(PestControlLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _pestControlLogRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Pest Control Log Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "PestControlLog");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "PestControlLog");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Feb'23
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
                _pestControlLogRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "PestControlLog");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Bind Datatable and Export to Excel
        /// <summary>
        /// Develop By Yatri on 15 March'23
        /// Calling method for PestControlLog data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllPestCntrolLogList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;

            var PestControlLogBOs = _pestControlLogRepository.GetAllPestCntrolLogList(flagdate, FromDate, ToDate);
            TempData["PestControlLogExcel"] = PestControlLogBOs;
            return Json(new { data = PestControlLogBOs }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Excel PestControlLog
        //public void ExportAsExcel()
        public ActionResult ExportAsExcel()
        {
            if (TempData["PestControlLogExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<PestControlLogBO> pestcontrollog = TempData["PestControlLogExcel"] as List<PestControlLogBO>;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            //dt.Columns.Add("Time");
            dt.Columns.Add("TypeOfPest");
            dt.Columns.Add("MethodForPestControl");
            dt.Columns.Add("Area");
            dt.Columns.Add("Frequncy");
            dt.Columns.Add("COARecivedFromPestControl");
            dt.Columns.Add("EffectiveOrNot");
            dt.Columns.Add("AnyHazardDetectedAfterPest");
            dt.Columns.Add("Verify By");
            dt.Columns.Add("Remark");

            int i = 1;
            foreach (PestControlLogBO st in pestcontrollog)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["Date"] = st.Date.ToString();
                //dr["Time"] = st.Time.ToString();
                dr["TypeOfPest"] = st.TypeOfPest.ToString();
                dr["MethodForPestControl"] = st.MethodForPestControl.ToString();
                dr["Area"] = st.Area.ToString();
                dr["Frequncy"] = st.Frequncy.ToString();
                dr["COARecivedFromPestControl"] = st.COARecivedFromPestControl.ToString();
                dr["EffectiveOrNot"] = st.EffectiveOrNot.ToString();
                dr["AnyHazardDetectedAfterPest"] = st.AnyHazardDetectedAfterPest.ToString();
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
            string filename = "Rpt_PestControlLog_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Pest Control Log Report";/* The Pest Control Log Report name are given here  */
            string Fromdate = "From Date : ";/* The From Date are given here  */
            string Todate = "To Date:";/* The To Date are given here  */
            string name = ApplicationSession.ORGANISATIONTIITLE;/* The Vangi Foods are given here  */
            string address = ApplicationSession.ORGANISATIONADDRESS;/* The Address are given here  */
            String fromdate = Convert.ToDateTime(Session["FromDate"]).ToString("dd/MM/yyyy");
            string todate = Convert.ToDateTime(Session["ToDate"]).ToString("dd/MM/yyyy");

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
              "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='5' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='9' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='5' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
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