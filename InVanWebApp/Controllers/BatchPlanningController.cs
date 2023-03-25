using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class BatchPlanningController : Controller
    {
        private IBatchPlanningRepository _repository;
        private ISalesOrderRepository _salesOrderRepository;
        private IProductMasterRepository _productMasterRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(BatchPlanningController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public BatchPlanningController()
        {
            _repository = new BatchPlanningRepository();
            _salesOrderRepository = new SalesOrderRepository();
            _productMasterRepository = new ProductMasterRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="batchPlanningRepository"></param>
        public BatchPlanningController(IBatchPlanningRepository batchPlanningRepository)
        {
            _repository = batchPlanningRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _repository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add inward master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddBatchPlanning()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindSONumber();
                BindProductDropDown();
                BindLocationName();

                BatchPlanningMasterBO model = new BatchPlanningMasterBO();
                //==========Document number for Inward note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=20 i.e. for generating the Batch planning number (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(20);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
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
        public ActionResult AddBatchPlanning(BatchPlanningMasterBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        model.PackingSizeUnit = "KG";

                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Batch plane created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate batch document number! Can not be inserted!');</script>";
                            BindSONumber();
                            BindProductDropDown();
                            BindLocationName();

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(20);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "BatchPlanning");

                    }
                    else
                    {
                        BindSONumber();
                        BindProductDropDown();
                        BindLocationName();

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(20);
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

                BindSONumber();
                BindProductDropDown();
                BindLocationName();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(20);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 Mar'23
        /// Farheen: Delete the perticular record Sales Order 
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteIBatchPlanning(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Batch planning deleted successfully!');</script>";
                return RedirectToAction("Index", "BatchPlanning");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind Dropdowns
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
            var result = _repository.GetWorkOrderNumber(Id);
            return Json(result);
        }

        public JsonResult GetRecipe(string id, string LocationId)
        {
            int ProductId = Convert.ToInt32(id);
            int locationId = Convert.ToInt32(LocationId);
            var result = _repository.GetRecipe(ProductId, locationId);
            return Json(result);
        }

        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        #endregion
    }
}