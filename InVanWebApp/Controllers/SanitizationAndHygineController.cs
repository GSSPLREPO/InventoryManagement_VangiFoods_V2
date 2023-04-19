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
    public class SanitizationAndHygineController : Controller
    {
        private ISanitizationAndHygineRepository _sanitizationAndHygineRepository;
        private static ILog log = LogManager.GetLogger(typeof(SanitizationAndHygineController));

        #region Initializing constructor
        /// <summary>
        /// Date: 22/02/2023
        /// Snehal: Constructor without parameter
        /// </summary>
        public SanitizationAndHygineController()
        {
            _sanitizationAndHygineRepository = new SanitizationAndHygineRepository();
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        ///Maharshi: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _sanitizationAndHygineRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Rendered the user to the add SanitizationAndHygine form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSanitizationAndHygine()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                SanitizationAndHygineBO model = new SanitizationAndHygineBO();
                model.VerifyByName = Session[ApplicationSession.USERNAME].ToString();
                model.Date = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Maharshi: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSanitizationAndHygine(SanitizationAndHygineBO model)
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
                        response = _sanitizationAndHygineRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Sanitization and Hygiene Details Inserted Successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            return View();
                        }
                        return RedirectToAction("Index", "SanitizationAndHygine");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "SanitizationAndHygine");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Maharshi: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSanitizationAndHygine(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                SanitizationAndHygineBO model = _sanitizationAndHygineRepository.GetById(Id);
                model.Date= DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Maharshi:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSanitizationAndHygine(SanitizationAndHygineBO model)
         {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                   
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.Date = DateTime.Today;
                        response = _sanitizationAndHygineRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Sanitization and Hygiene Details updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while updating!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "SanitizationAndHygine");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "SanitizationAndHygine");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Feb'23
        /// Maharshi: Delete the particular record
        /// </summary>
        /// <param name="Id">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _sanitizationAndHygineRepository.Delete(Id, userID);
                TempData["Success"] = "<script>alert('Record deleted successfully!');</script>";
                return RedirectToAction("Index", "SanitizationAndHygine");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Bind Datatable and Export Pdf & Excel
        /// <summary>
        /// Develop By Siddharth on 18-04-2023
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult SanitizationAndHygineList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {

            Session["FromDate"] = fromDate;
            Session["ToDate"] = toDate;


            var SanitizationLog = _sanitizationAndHygineRepository.SanitizationAndHygineList(flagdate, fromDate, toDate);
            TempData["SanitizationAndHygineExcel"] = SanitizationLog;
            TempData["SanitizationAndHygineExcel"] = SanitizationLog;
            return Json(new { data = SanitizationLog }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Excel Sanitization And Hygine
        //public void ExportAsExcel()
        public ActionResult SanitizationAndHygineExcel()
        {
            if (TempData["SanitizationAndHygineExcel"] == null)
            {
                return View("Index");
            }

            GridView gv = new GridView();

            List<SanitizationAndHygineBO> sanitizations = TempData["SanitizationAndHygineExcel"] as List<SanitizationAndHygineBO>;
            DataTable dt = new DataTable();
            //dt.Columns.Add("Sr.No");
            dt.Columns.Add("Name Of Employee");
            dt.Columns.Add("Department");
            dt.Columns.Add("Body Temprature");
            dt.Columns.Add("Hand Wash");
            dt.Columns.Add("Any Cuts & Wounds");
            dt.Columns.Add("Clean Uniform");
            dt.Columns.Add("Clean Nails");
            dt.Columns.Add("Wear Any Jewellery");
            dt.Columns.Add("Clean Shoes");
            dt.Columns.Add("Fully Coverd Hair");
            dt.Columns.Add("illness/Seakness");
            dt.Columns.Add("No Tobaco,Chewingum");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("Verify By Name");




            int i = 1;
            foreach (SanitizationAndHygineBO st in sanitizations)
            {
                DataRow dr = dt.NewRow();
                // dr["Sr.No"] = i;
                dr["Name Of Employee"] = st.NameOfEmpolyee.ToString();
                dr["Department"] = st.Department.ToString();
                dr["Body Temprature"] = st.BodyTemperature.ToString();
                dr["Hand Wash"] = st.HandWash.ToString();
                dr["Any Cuts & Wounds"] = st.AppearAnyCutsandWounds.ToString();
                dr["Clean Uniform"] = st.CleanUniform.ToString();
                dr["Clean Nails"] = st.CleanNails.ToString();
                dr["Wear Any Jewellery"] = st.WearAnyJwellery.ToString();
                dr["Clean Shoes"] = st.CleanShoes.ToString();
                dr["Fully Coverd Hair"] = st.FullyCoverdHair.ToString();
                dr["illness/Seakness"] = st.AnyKindOfIllnessSeakness.ToString();
                dr["No Tobaco,Chewingum"] = st.NoTobacoChewingum.ToString();
                dr["Remarks"] = st.Remark.ToString();
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
            string filename = "Rpt_Sanitization_And_Hygiene_Report_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.AllowPaging = false;
            gv.GridLines = GridLines.Both;
            gv.RenderControl(hw);

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";/* The logo are used  */
            string ReportName = "Sanitization And Hyginee";/* The Daily Monitoring Report name are given here  */
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
               + "</td><td colspan='8' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
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
               + "</td><td colspan='8' style='text-align:right; font-size:15px;font-weight:bold'>" + Todate + todate
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