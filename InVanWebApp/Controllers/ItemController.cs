using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using log4net;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
//using iTextSharp.text.html.simpleparser;

namespace InVanWebApp.Controllers
{
    public class ItemController : Controller
    {
        private IItemRepository _iItemRepository;
        private ITaxRepository _taxRepository;
        private IUnitRepository _unitRepository;
        private static ILog log = LogManager.GetLogger(typeof(ItemController));

        #region Initializing constructor
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemController()
        {
            _iItemRepository = new ItemRepository();
            _taxRepository = new TaxRepository();
            _unitRepository = new UnitRepository();
        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public ItemController(ItemRepository itemRepository)
        {
            _iItemRepository = itemRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _iItemRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Rendered the user to the add item form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItems()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItemTypeCategory();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItems(ItemBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _iItemRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Item Inserted Successfully!');</script>";
                        else
                            TempData["Success"] = "<script>alert('Duplicate Item! Can not be inserted!');</script>";

                        return RedirectToAction("Index", "Item");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Success"] = "<script>alert('Something went wrong!');</script>";
                log.Error(ex.Message, ex);
            }

            BindItemTypeCategory();
            return View();
        }



        /// <summary>
        /// Date: 22 Aug 2022
        /// Farheen: Upload multiple items.
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadItems()
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
                var listMaterialExcelEntity = new List<ItemBO>();
                using (var package = new ExcelPackage(materialExcelFile.InputStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int col = 1;

                    for (int row = 2; worksheet.Cells[row, col].Value != null; row++)
                    {
                        // do something with worksheet.Cells[row, col].Value 
                        var productExcel = new ItemBO();

                        productExcel.Item_Name = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : null;
                        productExcel.ItemTypeName = worksheet.Cells[row, col + 1].Value != null ? worksheet.Cells[row, col + 1].Value.ToString() : null;
                        productExcel.ItemCategoryName = worksheet.Cells[row, col + 2].Value != null ? worksheet.Cells[row, col + 2].Value.ToString() : null;
                        productExcel.Item_Code = worksheet.Cells[row, col + 3].Value != null ? worksheet.Cells[row, col + 3].Value.ToString() : null;
                        productExcel.HSN_Code = worksheet.Cells[row, col + 4].Value != null ? worksheet.Cells[row, col + 4].Value.ToString() : null;
                        productExcel.MinStock = Convert.ToDouble(worksheet.Cells[row, col + 5].Value != null ? worksheet.Cells[row, col + 5].Value : "0");
                        productExcel.Description = worksheet.Cells[row, col + 6].Value != null ? worksheet.Cells[row, col + 6].Value.ToString() : null;

                        productExcel.CreatedBy = 1;
                        productExcel.CreatedDate = DateTime.UtcNow.AddHours(5.5);
                        productExcel.IsDeleted = false;
                        listMaterialExcelEntity.Add(productExcel);
                    }
                    responsesList = _iItemRepository.SaveItemData(listMaterialExcelEntity);

                } // the using 
                  //------------------------ New Code End----------------------------------

                //List<MaterialEntities> listMaterialEntities = insertMaterialExcelRecords(materialExcelFile);
                //if (listMaterialExcelEntity != null && listMaterialExcelEntity.Count > 0)
                //{
                //    return Json("Data Uploaded Successfully", JsonRequestBehavior.AllowGet);
                //}
                //return Json("", JsonRequestBehavior.AllowGet);
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
                        return Json("Few items are uploaded successfully! And following items are duplicate: " + ItemList, JsonRequestBehavior.AllowGet);
                    else
                        return Json("No item inserted! And list of duplicate items: " + ItemList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("All Items Uploaded Successfully!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Item_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItem(int Item_ID)
        {
            BindItemTypeCategory();
            ItemBO model = _iItemRepository.GetById(Item_ID);
            return View(model);
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(ItemBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _iItemRepository.Update(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Item updated successfully!');</script>";

                    else
                        TempData["Success"] = "<script>alert('Duplicate Item! Can not be updated!');</script>";


                    return RedirectToAction("Index", "Item");
                }
                else
                    return View(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "Item");
            }

        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult DeleteItem(int Item_ID)
        //{
        //    ItemBO model = _iItemRepository.GetById(Item_ID);
        //    return View(model);
        //}

        [HttpGet]
        public ActionResult DeleteItem(int Item_ID)
        {
            _iItemRepository.Delete(Item_ID);
            TempData["Success"] = "<script>alert('Item deleted successfully!');</script>";
            return RedirectToAction("Index", "Item");
        }
        #endregion

        #region Export table in pdf 
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult ExportHTML(string ExportData)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader reader = new StringReader(ExportData);
        //        Document PdfFile = new Document(PageSize.A4);
        //        PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //        PdfFile.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
        //        PdfFile.Close();
        //        return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
        //    }
        //}
        #endregion

        #region For binding the dropdown of Item type and category.
        public void BindItemTypeCategory()
        {
            var itemCategory = _iItemRepository.GetItemCategoryForDropDown();
            var dd = new SelectList(itemCategory.ToList(), "ItemCategoryID", "ItemCategoryName");
            ViewData["ItemCategory"] = dd;
            var itemType = _iItemRepository.GetItemTypeForDropdown();
            var dd1 = new SelectList(itemType.ToList(), "ID", "ItemType");
            ViewData["ItemType"] = dd1;

            var tax = _taxRepository.GetAll();
            var dd2 = new SelectList(tax.ToList(), "ID", "TaxName");
            ViewData["Tax"] = dd2;

            var unit = _unitRepository.GetAll();
            var dd3 = new SelectList(unit.ToList(), "UnitID", "UnitName");
            ViewData["UOM"] = dd3;
        }

        #endregion

    }
}