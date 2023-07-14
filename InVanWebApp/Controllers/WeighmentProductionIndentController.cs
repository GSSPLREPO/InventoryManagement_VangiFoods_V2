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
    public class WeighmentProductionIndentController : Controller
    {
        private IWeighmentProductionIndentRepository _weighmentProductionIndentRepository;
        private IProductionIndentRepository _productionIndentRepository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IIndentRepository _repository;
        private IRecipeMaterRepository _productionRecipeRepository;
        private IProductMasterRepository _productMasterRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private IBatchPlanningRepository _batchPlanningRepository;

        private static ILog log = LogManager.GetLogger(typeof(WeighmentProductionIndentController));

        #region Initializing Constructor
        /// <summary>
        /// Date: 14 July'23
        /// Rahul:  Constructor without parameter 
        /// </summary>
        public WeighmentProductionIndentController()
        {
            _weighmentProductionIndentRepository = new WeighmentProductionIndentRepository();
            _productionIndentRepository = new ProductionIndentRepository();
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _repository = new IndentRepository();
            _productionRecipeRepository = new RecipeMaterRepository();
            _productMasterRepository = new ProductMasterRepository();
            _salesOrderRepository = new SalesOrderRepository();
            _batchPlanningRepository = new BatchPlanningRepository();
        }

        /// <summary>
        /// Date: 14 July'23
        /// Rahul:  Constructor with parameters for initializing the interface object.  
        /// </summary>
        ///<param name="itemRepository"></param>
        public WeighmentProductionIndentController(IWeighmentProductionIndentRepository weighmentProductionIndentRepository)
        {
            _weighmentProductionIndentRepository = weighmentProductionIndentRepository;
        }
        #endregion

        #region Bind grid 
        /// <summary>
        /// Date: 14 July'23 
        /// Rahul:  Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: WeighmentProductionIndent
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

        #region Bind Dropdowns  
        public void BindProductionIndentNumber() 
        {
            var result = _weighmentProductionIndentRepository.GetProductionIndentNumber();  
            var resultList = new SelectList(result.ToList(), "ID", "ProductionIndentNo");
            ViewData["PIND_dd"] = resultList;
        }
        public void BindUsers()
        {
            var UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);
            var result = _userDetailsRepository.GetAll(UserId);
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
            var result = _salesOrderRepository.GetAll();            
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONo");
            ViewData["SONumberAndId"] = resultList;

            //Bind WO Number 
            var resultWO = _salesOrderRepository.GetAll();
            var resultListWO = new SelectList(resultWO.ToList(), "SalesOrderId", "WorkOrderNo");
            ViewData["WONumberAndId"] = resultListWO;
        }

        public void BindSONumber()
        {
            var result = _salesOrderRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONo");
            ViewData["SO_dd"] = resultList;
        }

        public void BindProductDropDown()
        {
            var result = _productMasterRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "ProductID", "ProductName");
            ViewData["Product_dd"] = resultList;
        }

        public JsonResult GetWorkOrderNumber(string id)
        {
            var Id = Convert.ToInt32(id);
            var result = _productionIndentRepository.GetWorkOrderNumber(Id);
            return Json(result);
        }

        public JsonResult GetProductionIndentDetails(string id) 
        {
            var Id = Convert.ToInt32(id);
            var result = _weighmentProductionIndentRepository.GetProductionIndentDetails(Id);  
            return Json(result);
        }
        #endregion

        #region Bind all Recipe details 
        public JsonResult GetRecipe(string id, string RecipeID = null)
        {
            int ProductId = Convert.ToInt32(id);
            int recipeID = Convert.ToInt32(RecipeID);

            var result = _productionIndentRepository.GetRecipeDetailsById(ProductId, recipeID);
            return Json(result);
        }
        #endregion

        #region Bind all BindBatch Number details 
        public JsonResult BindBatchNumber(string id, string SO_Id)
        {
            int Item_ID = 0;
            int SO_ID = 0;

            if (id != "" && id != null)
                Item_ID = Convert.ToInt32(id);
            if (SO_Id != "" && SO_Id != null)
                SO_ID = Convert.ToInt32(SO_Id);

            var result = _productionIndentRepository.GetBatchNumberById(Item_ID, SO_ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert funcationality for Weighment Production Indent
        [HttpGet]
        public ActionResult AddWeighmentProductionIndent() 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindProductionIndentNumber();  
                BindUsers();
                BindItemTypeCategory();
                BindSONumber();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=16 i.e. for generating the Production Indent Number(logic is in SP).====//
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

        #endregion
    }
}