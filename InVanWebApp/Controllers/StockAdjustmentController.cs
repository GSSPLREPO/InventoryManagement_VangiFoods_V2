using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using InVanWebApp_BO;

namespace InVanWebApp.Controllers
{
    public class StockAdjustmentController : Controller
    {
        private IStockAdjustmentRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 17 Jan'23
        /// </summary>
        public StockAdjustmentController()
        {
            _repository = new StockAdjustmentRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 17 Jan'23
        /// </summary>
        /// <param name="stockAdjustmentRepository"></param>
        public StockAdjustmentController(IStockAdjustmentRepository stockAdjustmentRepository)
        {
            _repository = stockAdjustmentRepository;
        }
        #endregion

        #region Bind Grid
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _repository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add Stock adjustment note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddStockadjustment()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                StockAdjustmentBO model = new StockAdjustmentBO();
                model.DocumentDate = DateTime.Today;
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
        public ActionResult AddStockadjustment(StockAdjustmentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Stock adjusted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Can not be inserted!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            model.DocumentDate = DateTime.Today;
                            return View(model);
                        }

                        return RedirectToAction("Index", "StockAdjustment");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.DocumentDate = DateTime.Today;
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
                model.DocumentDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Nov'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteStockAdjustment(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Stock adjustment doument is deleted successfully!');</script>";
                return RedirectToAction("Index", "StockAdjustment");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Stock adjustment
        [HttpGet]
        public ActionResult ViewStockAdjustment(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                StockAdjustmentBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Fetch location stocks details for stock adjustment
        public JsonResult GetLocationStocksDetails(string id)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);

            var result = _repository.GetLocationStocksDetailsById(Location_Id);
            return Json(result);
        }
        #endregion

        #region Bind dropdowns 
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

            //=========here document type=12 i.e. for generating the Stock adjustment (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(12);
            ViewData["DocumentNo"] = DocumentNumber;
        }
        #endregion
    }
}