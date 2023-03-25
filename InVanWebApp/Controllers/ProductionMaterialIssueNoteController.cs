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
    public class ProductionMaterialIssueNoteController : Controller
    {
        private IProductionMaterialIssueNoteRepository _productionMaterialIssueNoteRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IStockAdjustmentRepository _stockAdjustmentRepository;
        private IProductionIndentRepository _productionIndentRepository; 
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Rahul
        /// Created Date: 21 March'23
        /// </summary>
        public ProductionMaterialIssueNoteController()
        {
            _productionMaterialIssueNoteRepository = new ProductionMaterialIssueNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _stockAdjustmentRepository = new StockAdjustmentRepository();
            _productionIndentRepository = new ProductionIndentRepository();
        }

        /// <summary>
        /// Created By: Rahul
        /// Created Date: 21 March'23
        /// </summary>
        /// <param name="stockAdjustmentRepository"></param>
        public ProductionMaterialIssueNoteController(IProductionMaterialIssueNoteRepository productionMaterialIssueNoteRepository)
        {
            _productionMaterialIssueNoteRepository = productionMaterialIssueNoteRepository;  
        }
        #endregion

        #region Bind Grid   
        // GET: ProductionMaterialIssueNote
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)            
                return RedirectToAction("Index", "Login");
            var model = _productionMaterialIssueNoteRepository.GetAll();
            return View(model); 
        }
        #endregion

        #region Bind dropdowns 
        public void BindProductionIndentNumber() 
        {
            var result = _productionMaterialIssueNoteRepository.GetProductionIndentNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "ProductionIndentNo");
            ViewData["ProductionIndentNumberAndId"] = resultList;
        }
        
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=13 i.e. for generating the Production Material Issue Note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(17);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        //Bind SO and WO Number 
        public void GetSOWONumber()
        {
            var resultWO = _productionIndentRepository.GetSONumberForDropdown();
            var resultListWO = new SelectList(resultWO.ToList(), "SalesOrderId", "WorkOrderNo");
            ViewData["WONumberAndId"] = resultListWO;
        }
        //Bind Get Item List
        public JsonResult GetItemList(string id)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);

            var result = _stockAdjustmentRepository.GetItemListByLocationId(Location_Id);
            return Json(result);
        }
        //Bind Production Indent
        public JsonResult GetProductionIndentItemList(string id) 
        {
            int ProductionIndent_Id = 0;
            if (id != "" && id != null)
                ProductionIndent_Id = Convert.ToInt32(id);

            var result = _productionMaterialIssueNoteRepository.GetItemListByProductionIndentId(ProductionIndent_Id);
            return Json(result);
        }
        #endregion

        #region Fetch Get Location Stocks Details for Production Material IssueNote
        public JsonResult GetLocationStocksDetails(string id, string locationId, string itemId = null)
        {
            int ProductionIndent_Id = 0;
            if (id != "" && id != null)
                ProductionIndent_Id = Convert.ToInt32(id);
            int Location_Id = 0;
            if (locationId != "" && locationId != null)
                Location_Id = Convert.ToInt32(locationId);
            int Item_Id = 0;
            if (itemId != null && itemId != "")
                Item_Id = Convert.ToInt32(itemId);

            var result = _productionMaterialIssueNoteRepository.GetProductionIndentIngredientsDetailsById(ProductionIndent_Id, Location_Id, Item_Id);
            return Json(result);
        }
        #endregion


        #region Insert functions
        /// <summary>
        /// Rahul: Rendered the user to the add Stock adjustment note. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProductionMaterialIssueNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                GetSOWONumber();
                BindProductionIndentNumber();

                ProductionMaterialIssueNoteBO model = new ProductionMaterialIssueNoteBO();
                model.ProductionMaterialIssueNoteDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductionMaterialIssueNote(ProductionMaterialIssueNoteBO model) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        if (model.txtItemDetails == null || model.txtItemDetails == "[]]")
                        {
                            TempData["Success"] = "<script>alert('No item is issued! Production material issue note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            GetSOWONumber();
                            BindProductionIndentNumber();

                            model.ProductionMaterialIssueNoteDate = DateTime.Today;
                            return View(model);
                        }
                        else
                        {
                            response = _productionMaterialIssueNoteRepository.Insert(model);
                            if (response.Status)
                                TempData["Success"] = "<script>alert('Production material issue note is created successfully!');</script>";
                            else
                            {
                                TempData["Success"] = "<script>alert('Error! Production material issue note cannot be created!');</script>";
                                BindLocationName();
                                GenerateDocumentNo();
                                GetSOWONumber();
                                BindProductionIndentNumber();

                                model.ProductionMaterialIssueNoteDate = DateTime.Today;
                                return View(model);
                            }
                        }

                        return RedirectToAction("Index", "ProductionMaterialIssueNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        GetSOWONumber();
                        BindProductionIndentNumber();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.ProductionMaterialIssueNoteDate = DateTime.Today;
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

                BindLocationName();
                GenerateDocumentNo();
                GetSOWONumber();
                BindProductionIndentNumber();

                model.ProductionMaterialIssueNoteDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region This method is for View the Production Material Issue Note
        [HttpGet]
        public ActionResult ViewProductionMaterialIssueNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ProductionMaterialIssueNoteBO model = _productionMaterialIssueNoteRepository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion



        #region Delete function
        /// <summary>
        /// Date: 24 Jan'23
        /// Rahul: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteProductionMaterialIssueNote(int ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _productionMaterialIssueNoteRepository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Production Material Issue note deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "ProductionMaterialIssueNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


    }
}