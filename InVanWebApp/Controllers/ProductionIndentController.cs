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

        #region MyRegion
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
            var product = _productMasterRepository.GetAll();
            var dd4 = new SelectList(product.ToList(), "ProductID", "ProductCode", "ProductName");
            ViewData["ProductName"] = dd4;

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");
            ViewData["Ingredients"] = dd;
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
                //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(16);
                ViewData["DocumentNo"] = DocumentNumber;

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

        ///// <summary>
        ///// Create By:Farheen
        ///// Dscription: Pass the data to the repository for insertion from it's view.
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult AddIndent(IndentBO model)
        //{
        //    try
        //    {
        //        if (Session[ApplicationSession.USERID] != null)
        //        {
        //            ResponseMessageBO response = new ResponseMessageBO();

        //            if (ModelState.IsValid)
        //            {
        //                model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
        //                response = _repository.Insert(model);
        //                if (response.Status)
        //                    TempData["Success"] = "<script>alert('Indent inserted successfully!');</script>";
        //                else
        //                {
        //                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
        //                    BindUsers();
        //                    BindLocation();
        //                    BindDesignations();
        //                    GetDocumentNumber objDocNo = new GetDocumentNumber();
        //                    //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
        //                    var DocumentNumber = objDocNo.GetDocumentNo(9);
        //                    ViewData["DocumentNo"] = DocumentNumber;

        //                    //Binding item grid with sell type item.
        //                    var itemList = _repository.GetItemDetailsForDD();
        //                    var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
        //                    ViewData["itemListForDD"] = dd;

        //                    return View(model);
        //                }

        //                return RedirectToAction("Index", "Indent");

        //            }
        //            else
        //            {
        //                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
        //                BindUsers();
        //                BindLocation();
        //                BindDesignations();
        //                var itemList = _repository.GetItemDetailsForDD();
        //                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
        //                ViewData["itemListForDD"] = dd;
        //                GetDocumentNumber objDocNo = new GetDocumentNumber();
        //                //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
        //                var DocumentNumber = objDocNo.GetDocumentNo(9);
        //                ViewData["DocumentNo"] = DocumentNumber;

        //                return View(model);
        //            }
        //        }
        //        else
        //            return RedirectToAction("Index", "Login");

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error", ex);
        //        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

        //        BindUsers();
        //        BindLocation();
        //        BindDesignations();
        //        var itemList = _repository.GetItemDetailsForDD();
        //        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
        //        ViewData["itemListForDD"] = dd;
        //        GetDocumentNumber objDocNo = new GetDocumentNumber();
        //        //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
        //        var DocumentNumber = objDocNo.GetDocumentNo(9);
        //        ViewData["DocumentNo"] = DocumentNumber;

        //        return View(model);
        //    }
        //}
        #endregion



    }
}