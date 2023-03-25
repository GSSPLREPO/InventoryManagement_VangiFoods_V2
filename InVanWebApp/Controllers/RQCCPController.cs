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

namespace InVanWebApp.Controllers
{
    public class RQCCPController : Controller
    {
        private IRQCCPRepository _RQCCPRepository;
        private static ILog log = LogManager.GetLogger(typeof(RQCCPController));

        #region Initializing constructor
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Constructor without parameter
        /// </summary>
        public RQCCPController()
        {
            _RQCCPRepository = new RQCCPRepository();
        }

        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Constructor with parameters for initializing the interface object.
        /// </summary>
        
        public RQCCPController(RQCCPRepository RQCCPRepository)
        {
            _RQCCPRepository = RQCCPRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Supplier/Customer list
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _RQCCPRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Rendered the user to the add company form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRQCCP()
        {
            if (Session[ApplicationSession.USERID] != null)

            {
                RQCCPBO model = new RQCCPBO();
                model.Date = DateTime.Today;
                model.BatchReleaseTimeOfRQ = DateTime.Now.ToString("HH:mm:ss tt");
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Charmi: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRQCCP(RQCCPBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _RQCCPRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('RQ CCP details Inserted Successfully!');</script>";
                        else
                        {
                            if (response.ItemName != null || response.ItemName != "")
                                TempData["Success"] = "<script>alert('Duplicate item details! Can not be inserted!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Error while insertion!');</script>";

                            return View();
                        }

                        return RedirectToAction("Index", "RQCCP");

                    }

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "RQCCP");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 22 Aug 2022
        /// Farheen: Upload multiple items.
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadRQCCP()
        {
            try
            {
                List<ResponseMessageBO> responsesList = new List<ResponseMessageBO>();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase materialExcelFile = files[0];
                string materialExcelFilename;

                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = materialExcelFile.FileName.Split(new char[] { '\\' });
                    materialExcelFilename = testfiles[testfiles.Length - 1];
                }
                else
                {
                    materialExcelFilename = materialExcelFile.FileName;
                }

                // Get the complete folder path and store the file inside it.  
                materialExcelFilename = Path.Combine(Server.MapPath("~/ExcelUploads/"), materialExcelFilename);
                materialExcelFile.SaveAs(materialExcelFilename);

                //------------------------ New Code Start----------------------------------
                var listMaterialExcelEntity = new List<RQCCPBO>();
                using (var package = new ExcelPackage(materialExcelFile.InputStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int col = 1;

                    for (int row = 2; worksheet.Cells[row, col].Value != null; row++)
                    {
                        // do something with worksheet.Cells[row, col].Value 
                        var productExcel = new RQCCPBO();

                        productExcel.Activity = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : null;
                        productExcel.ItemName = worksheet.Cells[row, col + 1].Value != null ? worksheet.Cells[row, col + 1].Value.ToString() : null;
                        productExcel.NoBatches = worksheet.Cells[row, col + 2].Value != null ? worksheet.Cells[row, col + 2].Value.ToString() : null;
                        productExcel.BatchWeight = worksheet.Cells[row, col + 3].Value != null ? worksheet.Cells[row, col + 3].Value.ToString() : null;
                        productExcel.MonitoringParameter = worksheet.Cells[row, col + 4].Value != null ? worksheet.Cells[row, col + 4].Value.ToString() : null;
                        productExcel.BatchReleaseTimeOfRQ = worksheet.Cells[row, col + 5].Value != null ? worksheet.Cells[row, col + 5].Value.ToString() : null;
                        productExcel.MandatoryTemp = worksheet.Cells[row, col + 6].Value != null ? worksheet.Cells[row, col + 6].Value.ToString() : null;
                        productExcel.Frequency = worksheet.Cells[row, col + 7].Value != null ? worksheet.Cells[row, col + 7].Value.ToString() : null;
                        productExcel.Responsibility = worksheet.Cells[row, col + 8].Value != null ? worksheet.Cells[row, col + 8].Value.ToString() : null;
                        productExcel.Remarks = worksheet.Cells[row, col + 9].Value != null ? worksheet.Cells[row, col + 9].Value.ToString() : null;
                        productExcel.Verification = worksheet.Cells[row, col + 10].Value != null ? worksheet.Cells[row, col + 10].Value.ToString() : null;

                        productExcel.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        productExcel.CreatedDate = DateTime.UtcNow.AddHours(5.5);
                        productExcel.IsDeleted = false;
                        listMaterialExcelEntity.Add(productExcel);
                    }
                    responsesList = _RQCCPRepository.SaveRQCCPData(listMaterialExcelEntity);

                }

                int i = 0, flag = 0;
                int count = responsesList.Count;
                string ItemList = "";

                while (i < count)
                {
                    if (responsesList[i].Status == false)
                        ItemList = ItemList + responsesList[i].ItemName + ", ";
                    else
                        flag = 1;
                    i++;
                }
                if (ItemList != "")
                {
                    if (flag == 1)
                        return Json("Few RQ CCP are uploaded successfully! And following RQ CCP's are duplicate: " + ItemList, JsonRequestBehavior.AllowGet);
                    else
                        return Json("Insertion failed! Duplicate item name: " + ItemList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("All RQ CCP Uploaded Successfully!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRQCCP(int RQCCPID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                RQCCPBO model = _RQCCPRepository.GetById(RQCCPID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRQCCP(RQCCPBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _RQCCPRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('RQ CCP updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate RQ CCP!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "RQCCP");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "RQCCP");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRQCCP(int RQCCPID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _RQCCPRepository.Delete(RQCCPID, userID);
                TempData["Success"] = "<script>alert('RQ CCP deleted successfully!');</script>";
                return RedirectToAction("Index", "RQCCP");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}