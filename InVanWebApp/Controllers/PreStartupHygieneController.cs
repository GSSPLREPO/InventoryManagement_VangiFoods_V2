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
using System.Web.UI;
using System.Data;

namespace InVanWebApp.Controllers
{
    public class PreStartupHygieneController : Controller
    {
        private IPreStartupHygieneRepository _preStartupHygieneRepository;
        private static ILog log = LogManager.GetLogger(typeof(PreStartupHygieneController));

        #region Initializing constructor
        /// <summary>
        /// Date: 20/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public PreStartupHygieneController()
        {
            _preStartupHygieneRepository = new PreStartupHygieneRepository();
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
                var model = _preStartupHygieneRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Rendered the user to the add PreStartupHygiene form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPreStartupHygiene()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PreStartupHygieneBO model = new PreStartupHygieneBO();
                model.VerifyBy = Session[ApplicationSession.USERNAME].ToString();
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
        public ActionResult AddPreStartupHygiene(PreStartupHygieneBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.VerifyBy = Session[ApplicationSession.USERNAME].ToString();
                        response = _preStartupHygieneRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Pre Startup Hygiene Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "PreStartupHygiene");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "PreStartupHygiene");
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
        public ActionResult EditPreStartupHygiene(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PreStartupHygieneBO model = _preStartupHygieneRepository.GetById(Id);
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
        public ActionResult EditPreStartupHygiene(PreStartupHygieneBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _preStartupHygieneRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Pre Startup Hygiene details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "PreStartupHygiene");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "PreStartupHygiene");
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
                _preStartupHygieneRepository.Delete(Id, userID);

                //_preStartupHygieneRepository.Delete(Id);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "PreStartupHygiene");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Report

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Snehal on 09 Feb'23
        /// Calling method for Daily Monitoring data
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreStartupHygieneList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;


            var MicroAnalysisBO = _preStartupHygieneRepository.GetPreStartupHygieneList(flagdate, fromDate, toDate);
            //TempData["PreStartupHygienePDF"] = MicroAnalysisBO;
            TempData["PreStartupHygieneExcel"] = MicroAnalysisBO;
            return Json(new { data = MicroAnalysisBO }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Excel micro analysis
        //public void ExportAsExcel()
        public ActionResult PreStartupHygieneExcel()
        {
            if (TempData["PreStartupHygieneExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<PreStartupHygieneBO> dailyMonitoring = TempData["PreStartupHygieneExcel"] as List<PreStartupHygieneBO>;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("RM Receving Area");
            dt.Columns.Add("Crates Blue");
            dt.Columns.Add("Crates Yellow");
            dt.Columns.Add("Crates Red");
            dt.Columns.Add("Weighting Area");
            dt.Columns.Add("Water");
            dt.Columns.Add("Hygine Area");
            dt.Columns.Add("Raw Material");
            dt.Columns.Add("Finished goods");
            dt.Columns.Add("Walk Way");
            dt.Columns.Add("Vegetable Washing Area");
            dt.Columns.Add("Peeling Machine");
            dt.Columns.Add("Cold Storage");
            dt.Columns.Add("Roboqubos");
            dt.Columns.Add("Silo");
            dt.Columns.Add("Packaging Line");
            dt.Columns.Add("Chiller DC");
            dt.Columns.Add("Verify By");



            int i = 1;
            foreach (PreStartupHygieneBO st in dailyMonitoring)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["Date"] = st.Date.ToString();
                dr["RM Receving Area"] = st.RMRecevingArea.ToString();
                dr["Crates Blue"] = st.CratesBlue.ToString();
                dr["Crates Yellow"] = st.CratesYellow.ToString();
                dr["Crates Red"] = st.CratesRed.ToString();
                dr["Weighting Area"] = st.WeightingArea.ToString();
                dr["Water"] = st.Water.ToString();
                dr["Hygine Area"] = st.HygineArea.ToString();
                dr["Raw Material"] = st.RawMaterial.ToString();
                dr["Finished goods"] = st.FinishGoods.ToString();
                dr["Walk Way"] = st.WalkWay.ToString();
                dr["Vegetable Washing Area"] = st.VegetableWashingArea.ToString();
                dr["Peeling Machine"] = st.PeelingMachine.ToString();
                dr["Cold Storage"] = st.ColdStorage.ToString();
                dr["Roboqubos"] = st.Roboqubos.ToString();
                dr["Silo"] = st.Silo.ToString();
                dr["Packaging Line"] = st.PackagingLine.ToString();
                dr["Chiller DC"] = st.Chiller.ToString();
                dr["Verify By"] = st.VerifyBy.ToString();

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
            string filename = "Rpt_Pre_Startup_Hygiene_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Pre Startup Hygiene Report";/* The Daily Monitoring Report name are given here  */
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

        #endregion
    }
}