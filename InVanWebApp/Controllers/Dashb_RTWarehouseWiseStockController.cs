using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;
using Newtonsoft.Json;

namespace InVanWebApp.Controllers
{
    public class Dashb_RTWarehouseWiseStockController : Controller
    {
        private IDashboardRepository _repository;
        private ILocationRepository _repositoryLocation;
        private IItemRepository _itemRepository;
        private IReportRepository _repositoryBatchNumber;
        private IFinishedGoodSeriesRepository _finishedGoodSeriesRepository;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public Dashb_RTWarehouseWiseStockController()
        {
            _repository = new DashboardRepository();
            _repositoryLocation = new LocationRepository();
            _itemRepository = new ItemRepository();
            _repositoryBatchNumber = new ReportRepository();
            _finishedGoodSeriesRepository = new FinishedGoodSeriesRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public Dashb_RTWarehouseWiseStockController(IDashboardRepository dashboardRepository)
        {
            _repository = dashboardRepository;
        }
        #endregion

        #region Bind real time warehouse wise dashboard

        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindLocationDropdown();
            BindItemDropDown();
            return View();
        }

        public JsonResult GetDashboardData(string id, string ItemId)
        {
            int LocationId = 0, itemId = 0;
            if (id != null & id != "")
                LocationId = Convert.ToInt32(id);
            if (ItemId != null & ItemId != "")
                itemId = Convert.ToInt32(ItemId);

            string jsonstring = string.Empty;

            var result = _repository.GetDashboardData(LocationId, itemId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind data reorder point of available total stock

        public ActionResult ReorderPointOfStocks()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindItemDropDown();
            return View();
        }

        public JsonResult GetDashboardDataReorderPointOfStocks(string id = "")
        {
            var ItemId = 0;
            if (id != "")
                ItemId = Convert.ToInt32(id);

            string jsonstring = string.Empty;

            var result = _repository.GetReorderPointDashboardData(ItemId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind Total Production Cost by Batch Number 

        public ActionResult ProductionCostByBatchNumber()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            BindSONumber();
            BindBatchNumberDropDown();
            return View();
        }

        public JsonResult GetDashboardProductionCostByBatchNumber(string soID = "", string batchNumber = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            var BatchNumber = "";
            if (batchNumber != "")
                BatchNumber = batchNumber.ToString();

            var SOID = 0;
            if (soID != "")
                SOID = Convert.ToInt32(soID);

            string jsonstring = string.Empty;

            var result = _repository.GetDashboardProductionCostByBatchNumber(SOID,BatchNumber, fromDate , toDate);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind location dropdown
        public void BindLocationDropdown()
        {
            var ItemType = _repositoryLocation.GetAll();
            var dd = new SelectList(ItemType.ToList(), "ID", "LocationName");
            ViewData["LocationName"] = dd;
        }

        #endregion

        #region Bind Item dropdown
        public void BindItemDropDown()
        {
            var model = _itemRepository.GetAll();
            var Item_dd = new SelectList(model.ToList(), "ID", "Item_Name");
            ViewData["Item"] = Item_dd;
        }
        #endregion

        #region Bind SO Number & Batch Number dropdown
        /// <summary>
        /// Snehal: This function is for fatching the SO Number & batch number
        /// Date:25-03-2023
        /// </summary>
        /// <returns></returns>
        /// 
        public void BindSONumber()
        {
            var soNumber = _finishedGoodSeriesRepository.GetSONUmberForDropDown();
            var soNumberList = new SelectList(soNumber.ToList(), "SalesOrderId", "SONo");
            ViewData["SONumbers"] = soNumberList;
        }

        public JsonResult BatchNumber(string SId)
        {
            var SOId = Convert.ToInt32(SId);
            var result = _finishedGoodSeriesRepository.GetBatchNo(SOId);
            return Json(result);
        }

        public void BindBatchNumberDropDown()
        {
            var model = _repositoryBatchNumber.GetAll();
            var BatchNumberdd = new SelectList(model.ToList(), "ID", "BatchNumber");
            ViewData["BatchNumber"] = BatchNumberdd;
        }
        #endregion

        #region Bind Utility Consumption By Batch dropdown
        public void BindUtilityConsumptionByBatchNumberDropDown()
        {
            var model = _repositoryBatchNumber.GetAllBatchNumber();
            var UtilityConsumptionByBatchNumber = new SelectList(model.ToList(), "ID", "BatchNumber");
            ViewData["UtilityConsumptionByBatchNumber"] = UtilityConsumptionByBatchNumber;
        }
        #endregion

        #region Bind Utility Consumption By Work Order dropdown
        public void BindUtilityConsumptionByWorkOrderNumberDropDown()
        {
            var model = _repositoryBatchNumber.GetAllWorkOrderNumber();
            var UtilityConsumptionByWorkOrder = new SelectList(model.ToList(), "ID", "WorkOrderNumber");
            ViewData["UtilityConsumptionByWorkOrder"] = UtilityConsumptionByWorkOrder;
        }
        #endregion


        #region Bind data Get Production Utility Consumption dashboard by batch  
        public ActionResult ProductionUtilityConsumptionByBatchDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)

                return RedirectToAction("ProductionUtilityConsumptionByBatchDashboard", "Login");

            ReportBO model = new ReportBO();
            model.fromDate = DateTime.Now;
            model.toDate = DateTime.Now;

            BindUtilityConsumptionByBatchNumberDropDown();
            BindUtilityConsumptionByWorkOrderNumberDropDown();

            return View();
        }

        public JsonResult GetProductionUtilityConsumptionByBatchDashboard(DateTime fromDate, DateTime toDate, string BatchNumber = "", string WorkOrderNumber = "")
        {
            var BatchNumberId = 0;
            var WorkOrderNumberId = 0;
            if (BatchNumber != "")
                BatchNumberId = Convert.ToInt32(BatchNumber);
            if (WorkOrderNumber != "")
                WorkOrderNumberId = Convert.ToInt32(WorkOrderNumber);

            string jsonstring = string.Empty;

            var result = _repository.GetProductionUtilityConsumptionByBatchDashboardData(fromDate, toDate, BatchNumberId, WorkOrderNumberId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

    }
}