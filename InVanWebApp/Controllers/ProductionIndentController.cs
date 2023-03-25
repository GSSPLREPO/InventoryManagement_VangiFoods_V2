using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class ProductionIndentController : Controller
    {
        private IProductionIndentRepository _productionIndentRepository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IIndentRepository _repository;
        private IRecipeMaterRepository _productionRecipeRepository;
        private IProductMasterRepository _productMasterRepository;

        private static ILog log = LogManager.GetLogger(typeof(ProductionIndentController));

        #region Initializing constructor
        /// <summary>
        /// Date: 24 Feb'23
        /// Rahul:  Constructor without parameter 
        /// </summary>
        public ProductionIndentController()
        {
            _productionIndentRepository = new ProductionIndentRepository();
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _repository = new IndentRepository();
            _productionRecipeRepository = new RecipeMaterRepository();
            _productMasterRepository = new ProductMasterRepository();
        }

        /// <summary>
        /// Date: 24 Feb'23
        /// Rahul:  Constructor with parameters for initializing the interface object. 
        /// </summary>
        ///<param name="itemRepository"></param>
        public ProductionIndentController(IProductionIndentRepository productionIndentRepository)
        {
            _productionIndentRepository = productionIndentRepository; 
        }
        #endregion

        #region Bind grid 
        /// <summary>
        /// Date: 24 Feb 2023
        /// Rahul:  Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: ProductionIndent
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _productionIndentRepository.GetAll();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion

        #region Bind dorpdowns
        public void BindUsers()
        {
            var result = _userDetailsRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "EmployeeID", "EmployeeName");
            ViewData["EmployeeName"] = resultList;
        }        
        public void BindItemTypeCategory()
        {
            var product = _productionRecipeRepository.GetAll();
            var dd4 = new SelectList(product.ToList(), "RecipeID", "RecipeName");
            ViewData["ProductName"] = dd4;

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");
            ViewData["Ingredients"] = dd;

            //Bind SO Number 
            var result = _productionIndentRepository.GetSONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONumber");
            ViewData["SONumberAndId"] = resultList;

            //Bind WO Number 
            var resultWO = _productionIndentRepository.GetSONumberForDropdown();
            var resultListWO = new SelectList(resultWO.ToList(), "SalesOrderId", "WorkOrderNo");
            ViewData["WONumberAndId"] = resultListWO;

        }
        #endregion

        #region Bind all Recipe details 
        public JsonResult BindRecipeDetails(string id, string RecipeID=null)
        {
            int ProductId = 0;  
            int RecipeId = 0;  
            
            if (id != "" && id != null)
                ProductId = Convert.ToInt32(id);

            if (RecipeID != "" && RecipeID != null)
                RecipeId = Convert.ToInt32(RecipeID);
            
            var result = _productionIndentRepository.GetRecipeDetailsById(ProductId,RecipeId);
            return Json(result);
        }
        #endregion

        #region Insert functionality of Production Indent
        [HttpGet]
        public ActionResult AddProductionIndent()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindItemTypeCategory();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=16 i.e. for generating the Production Indent Number(logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(16);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding Production Indent Batch Numer. 
                //var batchNumberList = _productionIndentRepository.GetItemDetailsForBatchNumber(); 
                //var BatchNumber = new SelectList(batchNumberList.ToList(), "BatchNumber");
                //ViewData["BatchNo"] = BatchNumber;  

                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                ProductionIndentBO model = new ProductionIndentBO();
                model.IssueDate = DateTime.Today;
                model.ProductionDate = DateTime.Today;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Create By:Rahul 
        /// Dscription: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductionIndent(ProductionIndentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionIndentRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Production Indent inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            BindUsers();
                            BindItemTypeCategory();
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                            var DocumentNumber = objDocNo.GetDocumentNo(16);
                            ViewData["DocumentNo"] = DocumentNumber;

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            ViewData["itemListForDD"] = dd;

                            return View(model);
                        }

                        return RedirectToAction("Index", "ProductionIndent");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindItemTypeCategory();
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                        var DocumentNumber = objDocNo.GetDocumentNo(16);
                        ViewData["DocumentNo"] = DocumentNumber;

                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

                BindUsers();
                BindItemTypeCategory();
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(16);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
        }
        #endregion

        #region Update Functions
        /// <summary>
        ///Create By: Rahul 
        ///Description: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductionIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindItemTypeCategory();

                ProductionIndentBO model = _productionIndentRepository.GetById(ID);
                model.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(ID); 
                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                string itemListForDD = "itemListForDD";

                if (model != null)
                {
                    var ItemCount = model.indent_Details.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        itemListForDD = "itemListForDD";
                        itemListForDD = itemListForDD + i;
                        dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.indent_Details[i].ItemId);
                        ViewData[itemListForDD] = dd;
                        i++;
                    }

                }

                ViewData[itemListForDD] = dd;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }

        /// <summary>
        /// Rahul:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductionIndent(ProductionIndentBO model) 
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionIndentRepository.Update(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Production Indent updated successfully!');</script>";


                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindUsers();
                            BindItemTypeCategory();

                            ProductionIndentBO model1 = _productionIndentRepository.GetById(model.ID);
                            model1.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(model.ID);

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model1 != null)
                            {
                                var ItemCount = model1.indent_Details.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }

                        return RedirectToAction("Index", "ProductionIndent");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindItemTypeCategory();

                        ProductionIndentBO model1 = _productionIndentRepository.GetById(model.ID);
                        model1.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(model.ID);

                        //Binding item grid with sell type item.
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        string itemListForDD = "itemListForDD";

                        if (model1 != null)
                        {
                            var ItemCount = model1.indent_Details.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                itemListForDD = "itemListForDD";
                                itemListForDD = itemListForDD + i;
                                dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
                                ViewData[itemListForDD] = dd;
                                i++;
                            }

                        }

                        ViewData[itemListForDD] = dd;

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
                return RedirectToAction("Index", "ProductionIndent");
            }
        }
        #endregion


        #region Delete function
        /// <summary>
        /// Create By: Rahul
        /// Description: This function is for deleting the Production Indent and 
        /// Production Indent Ingredients details by using Production Indent ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteProductionIndent(int ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _productionIndentRepository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Production Indent deleted successfully!');</script>";
                return RedirectToAction("Index", "ProductionIndent");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


    }
}