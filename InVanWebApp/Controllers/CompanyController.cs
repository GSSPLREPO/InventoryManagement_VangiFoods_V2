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
    public class CompanyController : Controller
    {
        private ICompanyRepository _companyRepository;
        private static ILog log = LogManager.GetLogger(typeof(CompanyController));

        #region Initializing constructor
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public CompanyController()
        {
            _companyRepository = new CompanyRepository();
        }

        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="supplierCustomerRepository"></param>
        public CompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 23 Aug'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Supplier/Customer list
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _companyRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 24 Aug'22
        /// Farheen: Rendered the user to the add company form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCompany()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompany(CompanyBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        response = _companyRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Company details Inserted Successfully!');</script>";
                        else
                        {
                            if (response.CompanyName != null || response.CompanyName != "")
                                TempData["Success"] = "<script>alert('Duplicate company details! Can not be inserted!');</script>";
                            else
                                TempData["Success"] = "<script>alert('Error while insertion!');</script>";

                            return View();
                        }

                        return RedirectToAction("Index", "Company");

                    }

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "Company");
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
        public JsonResult UploadCompany()
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
                var listMaterialExcelEntity = new List<CompanyBO>();
                using (var package = new ExcelPackage(materialExcelFile.InputStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int col = 1;

                    for (int row = 2; worksheet.Cells[row, col].Value != null; row++)
                    {
                        // do something with worksheet.Cells[row, col].Value 
                        var productExcel = new CompanyBO();

                        productExcel.CompanyName = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : null;
                        productExcel.CompanyType = worksheet.Cells[row, col + 1].Value != null ? worksheet.Cells[row, col + 1].Value.ToString() : null;
                        productExcel.EmailId = worksheet.Cells[row, col + 2].Value != null ? worksheet.Cells[row, col + 2].Value.ToString() : null;
                        productExcel.ContactPersonName = worksheet.Cells[row, col + 3].Value != null ? worksheet.Cells[row, col + 3].Value.ToString() : null;
                        productExcel.ContactPersonNo = worksheet.Cells[row, col + 4].Value != null ? worksheet.Cells[row, col + 4].Value.ToString() : null;
                        productExcel.Address = worksheet.Cells[row, col + 5].Value != null ? worksheet.Cells[row, col + 5].Value.ToString() : null;
                        productExcel.GSTNumber = worksheet.Cells[row, col + 6].Value != null ? worksheet.Cells[row, col + 6].Value.ToString() : null;
                        productExcel.Remarks = worksheet.Cells[row, col + 7].Value != null ? worksheet.Cells[row, col + 7].Value.ToString() : null;
                        productExcel.IsActive = worksheet.Cells[row, col + 8].Value != null ? Convert.ToBoolean(worksheet.Cells[row, col + 8].Value) : true;
                        productExcel.IsBlackListed = worksheet.Cells[row, col + 9].Value != null ? Convert.ToBoolean(worksheet.Cells[row, col + 9].Value) : false;

                        productExcel.CreatedBy = 1;
                        productExcel.CreatedDate = DateTime.UtcNow.AddHours(5.5);
                        productExcel.IsDeleted = false;
                        listMaterialExcelEntity.Add(productExcel);
                    }
                    responsesList = _companyRepository.SaveCompanyData(listMaterialExcelEntity);

                }

                int i = 0, flag = 0;
                int count = responsesList.Count;
                string ItemList = "";

                while (i < count)
                {
                    if (responsesList[i].Status == false)
                        ItemList = ItemList + responsesList[i].CompanyName + ", ";
                    else
                        flag = 1;
                    i++;
                }
                if (ItemList != "")
                {
                    if (flag == 1)
                        return Json("Few company are uploaded successfully! And following companies are duplicate: " + ItemList, JsonRequestBehavior.AllowGet);
                    else
                        return Json("Insertion failed! Duplicate company name: " + ItemList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("All Company Uploaded Successfully!", JsonRequestBehavior.AllowGet);
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
        public ActionResult EditCompany(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                CompanyBO model = _companyRepository.GetById(ID);
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
        public ActionResult EditCompany(CompanyBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        response = _companyRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Company updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Company!');</script>";
                            return View();
                        }

                        return RedirectToAction("Index", "Company");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "Company");
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
        public ActionResult DeleteSupplierCustomer(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                _companyRepository.Delete(ID);
                TempData["Success"] = "<script>alert('Supplier/Customer deleted successfully!');</script>";
                return RedirectToAction("Index", "Company");
            }
            else
                return RedirectToAction("Index","Login");
        }

        #endregion
    }
}