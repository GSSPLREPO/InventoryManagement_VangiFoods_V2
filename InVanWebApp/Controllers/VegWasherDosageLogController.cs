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

namespace InVanWebApp.Controllers
{
    public class VegWasherDosageLogController : Controller
    {
        private IVegWasherDosageLogRepository _vegWasherDosageLogRepository;
            
        private static ILog log = LogManager.GetLogger(typeof(VegWasherDosageLogController));

        #region Initializing constructor
        /// <summary>
        /// Date: 25/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public VegWasherDosageLogController()
        {
            _vegWasherDosageLogRepository = new VegWasherDosageLogRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 25 Feb'23
        ///Snehal: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _vegWasherDosageLogRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 25 Feb'23
        /// Snehal: Rendered the user to the add VegWasherDosageLog form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddVegWasherDosageLog()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                VegWasherDosageLogBO model = new VegWasherDosageLogBO();
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
        public ActionResult AddVegWasherDosageLog(VegWasherDosageLogBO model)
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
                        response = _vegWasherDosageLogRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Veg Washer Dosage Log Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "VegWasherDosageLog");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "VegWasherDosageLog");
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
        public ActionResult EditVegWasherDosageLog(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                VegWasherDosageLogBO model = _vegWasherDosageLogRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 25 Feb'23
        /// Snehal:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditVegWasherDosageLog(VegWasherDosageLogBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _vegWasherDosageLogRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Veg Washer Dosage Log Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "VegWasherDosageLog");
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "VegWasherDosageLog");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 25 Feb'23
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
                _vegWasherDosageLogRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "VegWasherDosageLog");

            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Siddharth on 17-04-2023
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllVegDoserLogList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;

            var VegDoserLog = _vegWasherDosageLogRepository.GetAllVegDoserLogList(flagdate, fromDate, toDate);
            TempData["VegWasherDoserExcel"] = VegDoserLog;
            TempData["VegWasherDoserExcel"] = VegDoserLog;
            return Json(new { data = VegDoserLog }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Excel Veg Washer Doser
        //public void ExportAsExcel()
        public ActionResult VegWasherDoserExcel()
        {
            if (TempData["VegWasherDoserExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<VegWasherDosageLogBO> vegWashers = TempData["VegWasherDoserExcel"] as List<VegWasherDosageLogBO>;
            DataTable dt = new DataTable();
            //dt.Columns.Add("Sr.No");
            dt.Columns.Add("Date");
            dt.Columns.Add("Veg Washer-1 Solution-A (ml)");
            dt.Columns.Add("Veg Washer-1 Solution-B (ml)");
            dt.Columns.Add("Veg Washer-1 Name of Item");
            dt.Columns.Add("Veg Washer-1 Washing Time");
            dt.Columns.Add("Veg Washer-1 PPM");
            dt.Columns.Add("Veg Washer-2 Solution-A (ml)");
            dt.Columns.Add("Veg Washer-2 Solution-B (ml)");
            dt.Columns.Add("Veg Washer-2 Name of Item");
            dt.Columns.Add("Veg Washer-2 Washing Time");
            dt.Columns.Add("Veg Washer-2 PPM");
            dt.Columns.Add("Verify By");


            int i = 1;
            foreach (VegWasherDosageLogBO st in vegWashers)
            {
                DataRow dr = dt.NewRow();
                // dr["Sr.No"] = i;
                dr["Date"] = st.Date.ToString();
                dr["Veg Washer-1 Solution-A (ml)"] = st.VegWasher1SolutionAMl.ToString();
                dr["Veg Washer-1 Solution-B (ml)"] = st.VegWasher1SolutionBMl.ToString();
                dr["Veg Washer-1 Name of Item"] = st.NameOfItem1.ToString();
                dr["Veg Washer-1 Washing Time"] = st.WashingTime1.ToString();
                dr["Veg Washer-1 PPM"] = st.Ppm1.ToString();
                dr["Veg Washer-2 Solution-A (ml)"] = st.VegWasher2SolutionAMl.ToString();
                dr["Veg Washer-2 Solution-B (ml)"] = st.VegWasher2SolutionBMl.ToString();
                dr["Veg Washer-2 Name of Item"] = st.NameOfItem2.ToString();
                dr["Veg Washer-2 Washing Time"] = st.WashingTime2.ToString();
                dr["Veg Washer-2 PPM"] = st.Ppm1.ToString();
                dr["Verify By"] = st.VerifyByName.ToString();

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
            string filename = "Rpt_Veg_Washer_Dosage_Log_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Veg Washer Doser Report";/* The Daily Monitoring Report name are given here  */
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
              "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
              "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
              "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
               + "<tr><td colspan='6' style='text-align:left; font-size:15px;font-weight:bold'>" + Fromdate + fromdate
               + "</td><td colspan='6' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
              /*+ "</td></tr><tr><td colspan='20'></td></tr>"*/ + "</table>"
              + "<table style='text-align:left'><tr style='text-align:left'><td style='text-align:left'>" + sw.ToString() + "</tr></td></table>";
            }
            else
            {
                content1 = "<table>" + "<tr><td colspan='2' rowspan='4'> <img height='100' width='150' src='" + strPath + "'/></td></td>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:25px;font-weight:bold;color:Red;'>" + ReportName + "</span></td></tr>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-size:15px;font-weight:bold'>" + name + "</td></tr>" +
               "<tr><td colspan='12' style='text-align:center'><span align='center' style='font-weight:bold'>" + address + "</td></tr>"
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
    }
}