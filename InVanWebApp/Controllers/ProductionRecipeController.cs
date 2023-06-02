﻿using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class ProductionRecipeController : Controller
    {
        private IProductionRecipeRepository _productionRecipeRepository;
        private IUnitRepository _unitRepository;
        private IProductMasterRepository _productMasterRepository;
        private static ILog log = LogManager.GetLogger(typeof(ProductionRecipeController));

        #region Initializing constructor
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor without parameter 
        /// </summary>
        public ProductionRecipeController() 
        {
            _productionRecipeRepository = new ProductionRecipeRepository();
            _unitRepository = new UnitRepository();
            _productMasterRepository = new ProductMasterRepository();
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="recipeRepository"></param>
        public ProductionRecipeController(ProductionRecipeRepository recipeRepository)
        {
            _productionRecipeRepository = recipeRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 27 Feb '23
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: ProductionRecipe
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _productionRecipeRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region For binding the dropdown of Production Recipe Item Packing Size Unit. 
        public void BindItemTypeCategory()
        {
            var unit = _unitRepository.GetAll();
            var dd3 = new SelectList(unit.ToList(), "UnitID", "UnitName");
            ViewData["UOM"] = dd3;

            var product = _productMasterRepository.GetAll();
            var dd4 = new SelectList(product.ToList(), "ProductID", "ProductCode", "ProductName");
            ViewData["ProductName"] = dd4;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Rendered the user to the add Recipe item form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRecipeItems() 
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
        /// Date: 27 Feb '23 
        /// Rahul: Pass the data to the repository for insertion from it's view. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRecipeItems(RecipeMasterBO model) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionRecipeRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Recipe Item Inserted Successfully!');</script>";
                        else
                            TempData["Success"] = "<script>alert('Duplicate Recipe Item! Can not be inserted!');</script>";

                        return RedirectToAction("Index", "ProductionRecipe");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return RedirectToAction("Index", "ProductionRecipe");
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
        /// Date: 27 Feb '23 
        /// Rahul: Upload multiple Recipe items. 
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadRecipeItems()
        {
            if (Session[ApplicationSession.USERID] != null)
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
                    var listMaterialExcelEntity = new List<RecipeMasterBO>();
                    using (var package = new ExcelPackage(materialExcelFile.InputStream))
                    {
                        // get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        int col = 1;

                        for (int row = 2; worksheet.Cells[row, col].Value != null; row++)
                        {
                            // do something with worksheet.Cells[row, col].Value 
                            var productExcel = new RecipeMasterBO(); 

                            productExcel.RecipeName = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : null;
                            productExcel.Description = worksheet.Cells[row, col + 1].Value != null ? worksheet.Cells[row, col + 1].Value.ToString() : null;
                            productExcel.PackingSize = float.Parse(worksheet.Cells[row, col + 2].Value != null ? worksheet.Cells[row, col + 2].Value.ToString() : null);
                            productExcel.PackingSizeUnit = Convert.ToInt32(worksheet.Cells[row, col + 3].Value != null ? worksheet.Cells[row, col + 3].Value.ToString() : null);
                            var tempCreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);

                            productExcel.CreatedBy = tempCreatedBy;
                            productExcel.CreatedDate = DateTime.UtcNow.AddHours(5.5);
                            productExcel.IsDeleted = false;
                            listMaterialExcelEntity.Add(productExcel);
                        }
                        responsesList = _productionRecipeRepository.SaveRecipeItemData(listMaterialExcelEntity);

                    } // the using 
                      //------------------------ New Code End----------------------------------

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
                            return Json("Few Recipe items are uploaded successfully! And following Recipe items are duplicate: " + ItemList, JsonRequestBehavior.AllowGet);
                        else
                            return Json("No Recipe item inserted! And list of duplicate Recipe items: " + ItemList, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json("All Recipe Items Uploaded Successfully!", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json("Session Out!", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Update function
        /// <summary>
        /// Date: 27 Feb '23 
        ///Rahul: Rendered the user to the edit page with details of a perticular record.  
        /// </summary>
        /// <param name="Recipe_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRecipeItem(int Recipe_ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItemTypeCategory();
                RecipeMasterBO model = _productionRecipeRepository.GetById(Recipe_ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul:  Pass the data to the repository for updating that record. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRecipeItem(RecipeMasterBO model) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionRecipeRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Recipe Item updated successfully!');</script>";

                        else
                            TempData["Success"] = "<script>alert('Duplicate Recipe Item! Can not be updated!');</script>";


                        return RedirectToAction("Index", "ProductionRecipe");
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "ProductionRecipe");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 27 Feb '23 
        /// Rahul: Delete the perticular record 
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRecipeItem(int Recipe_ID)  
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                int userId = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _productionRecipeRepository.Delete(Recipe_ID, userId);
                TempData["Success"] = "<script>alert('Recipe Item deleted successfully!');</script>";
                return RedirectToAction("Index", "ProductionRecipe"); 
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


    }
}