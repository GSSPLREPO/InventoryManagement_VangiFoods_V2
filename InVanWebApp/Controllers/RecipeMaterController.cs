using InVanWebApp.Common;
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
    public class RecipeMaterController : Controller
    {
        private IRecipeMaterRepository _productionRecipeRepository;        
        private IProductMasterRepository _productMasterRepository;
        //private IIndentRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(RecipeMaterController));

        #region Initializing constructor
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor without parameter 
        /// </summary>
        public RecipeMaterController()
        {
            _productionRecipeRepository = new RecipeMaterRepository();            
            _productMasterRepository = new ProductMasterRepository();
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="recipeRepository"></param>
        public RecipeMaterController(RecipeMaterRepository recipeRepository)
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
            var product = _productMasterRepository.GetAll();
            var dd4 = new SelectList(product.ToList(), "ProductID", "ProductCode", "ProductName");
            ViewData["ProductName"] = dd4;

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");
            ViewData["Ingredients"] = dd;
        }
        #endregion

        #region Function for get Recipe item details 
        public JsonResult GetRecipeitemDetails(string id)
        {
            var itemId = Convert.ToInt32(id);
            var recipeDetails = _productionRecipeRepository.GetRecipeDetails(itemId);
            return Json(recipeDetails);
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

                        return RedirectToAction("Index", "RecipeMater");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return RedirectToAction("Index", "RecipeMater");
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
                model.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(Recipe_ID);

                //Binding item grid with Recipe. 
                var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                string Ingredients = "Ingredients";

                if (model != null)
                {
                    var ItemCount = model.recipe_Details.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        Ingredients = "Ingredients";
                        Ingredients = Ingredients + i;
                        dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model.recipe_Details[i].ItemId);
                        ViewData[Ingredients] = dd;
                        i++;
                    }
                }

                ViewData[Ingredients] = dd;

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
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionRecipeRepository.Update(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Recipe Item updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

                            BindItemTypeCategory();
                            RecipeMasterBO model1 = _productionRecipeRepository.GetById(model.RecipeID);
                            model1.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(model.RecipeID);

                            //Binding item grid with Recipe. 
                            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                            string Ingredients = "Ingredients";

                            if (model1 != null)
                            {
                                var ItemCount = model1.recipe_Details.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    Ingredients = "Ingredients";
                                    Ingredients = Ingredients + i;
                                    dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model1.recipe_Details[i].ItemId);
                                    ViewData[Ingredients] = dd;
                                    i++;
                                }
                            }

                            ViewData[Ingredients] = dd;

                            return View(model1);
                        }
                        return RedirectToAction("Index", "RecipeMater");
                    }

                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindItemTypeCategory();
                        RecipeMasterBO model1 = _productionRecipeRepository.GetById(model.RecipeID);
                        model1.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(model.RecipeID);

                        //Binding item grid with Recipe. 
                        var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                        var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                        string Ingredients = "Ingredients";

                        if (model1 != null)
                        {
                            var ItemCount = model1.recipe_Details.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                Ingredients = "Ingredients";
                                Ingredients = Ingredients + i;
                                dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model1.recipe_Details[i].ItemId);
                                ViewData[Ingredients] = dd;
                                i++;
                            }
                        }

                        ViewData[Ingredients] = dd;

                        return View(model1);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "RecipeMater");
            }
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
                return RedirectToAction("Index", "RecipeMater");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


    }
}