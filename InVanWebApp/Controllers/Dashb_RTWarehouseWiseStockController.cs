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
        private IReportRepository _repositoryRR;
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
            _repositoryRR = new ReportRepository();
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

        #region Bind data ActualYeild And ExpectedYeild

        public ActionResult YeildDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            ReportBO model = new ReportBO();
            model.fromDate = DateTime.Now;
            model.toDate = DateTime.Now;
            BindBatchNumberDropDown();
            BindWorkOrderNumberDropDown();
            return View();
        }

        public JsonResult GetYeildDashboardData(DateTime fromDate, DateTime toDate, string BatchNumber = "", string WorkOrderNumber = "")
        {

            var BatchNumberId = "0";
            var WorkOrderNumberId = 0;
            if (BatchNumber != "")
                BatchNumberId = BatchNumber;
            //BatchNumberId = Convert.ToInt32(BatchNumber);
            if (WorkOrderNumber != "")
                WorkOrderNumberId = Convert.ToInt32(WorkOrderNumber);

            string jsonstring = string.Empty;

            var result = _repository.GetYeildDashboardData(fromDate, toDate, BatchNumberId, WorkOrderNumberId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind FIFO System

        public ActionResult FIFOSystem()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            DashboardBO model = new DashboardBO();
            BindLocationDropdown();
            BindItemDropDown();
            return View(model);
        }

        public JsonResult GetFIFOSystem(string id, string ItemId)
        {
            int LocationId = 0, itemId = 0;
            if (id != null && id != "")
                LocationId = Convert.ToInt32(id);
            if (ItemId != null && ItemId != "")
                itemId = Convert.ToInt32(ItemId);

            DashboardBO model = new DashboardBO();
            
            string jsonstring = string.Empty;

            var result = _repository.GetFIFOSystem(itemId, LocationId);
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

            var result = _repository.GetDashboardProductionCostByBatchNumber(SOID, BatchNumber, fromDate, toDate);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind data reorder point of available Opening And Closing

        public ActionResult OrderSummaryDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            OrderSummaryBO model = new OrderSummaryBO();
            model.fromDate = DateTime.Now;
            model.toDate = DateTime.Now;
            return View(model);
        }

        public JsonResult GetOrderSummaryDashboardData(DateTime fromDate, DateTime toDate, string Duration = "")
        {
            var DurationID = 2;
            if (Duration != " ")
                DurationID = Convert.ToInt32(Duration);



            string jsonstring = string.Empty;

            var result = _repository.GetOrderSummaryDashboardData(fromDate, toDate, DurationID);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind data reorder point of available Total Production Cost by SO or Work Order wise dashboard

        public ActionResult WorkOrderwiseProductionCostDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");
            DashboardBO model = new DashboardBO();
            model.fromDate = DateTime.Now;
            model.toDate = DateTime.Now;
            //BindBatchNumberDropDown();
            BindWorkOrderNumberDropDown();
            return View();
        }

        public JsonResult GetWorkOrderwiseProductionCostData(DateTime FromDate, DateTime ToDate, String SalesOrderId = "")
        {

            var SaleOrderid = 0;

            if (SalesOrderId != "")
                SaleOrderid = Convert.ToInt32(SalesOrderId);


            string jsonstring = string.Empty;

            var result = _repository.GetWorkOrderwiseProductionCost(FromDate, ToDate, SaleOrderid);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind data Get Production Utility Consumption dashboard by batch  
        public ActionResult ProductionUtilityConsumptionByBatchDashboard()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            ReportBO model = new ReportBO();
            model.fromDate = DateTime.Now;
            model.toDate = DateTime.Now;

            BindBatchNumberDropDown();
            BindWorkOrderNumberDropDown();

            //BindUtilityConsumptionByBatchNumberDropDown();
            //BindUtilityConsumptionByWorkOrderNumberDropDown();

            return View();
        }

        public JsonResult GetProductionUtilityConsumptionByBatchDashboard(DateTime fromDate, DateTime toDate, string BatchNumber = "", string WorkOrderNumber = "")
        {
            string jsonstring = string.Empty;

            var result = _repository.GetProductionUtilityConsumptionByBatchDashboardData(fromDate, toDate, BatchNumber, WorkOrderNumber);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region Bind data Total Inventory Value Warehouse Wise

        public ActionResult TotalInventoryValueWarehouseWise()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public JsonResult GetTotalInventoryValue()
        {


            string jsonstring = string.Empty;

            var result = _repository.GetTotalInventoryValue();
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region Bind dropdowns
        public void BindLocationDropdown()
        {
            var ItemType = _repositoryLocation.GetAll();
            var dd = new SelectList(ItemType.ToList(), "ID", "LocationName");
            ViewData["LocationName"] = dd;
        }
        public void BindItemDropDown()
        {
            var model = _itemRepository.GetAll();
            //var Item_dd = new SelectList(model.ToList(), "ID", "Item_Name");
            var Item_dd = new SelectList(model.ToList(), "ID", "ItemNameWithCode");
            ViewData["Item"] = Item_dd;
        }
        public void BindBatchNumberDropDown()
        {
            var model = _repositoryRR.GetAll();
            var BatchNumberdd = new SelectList(model.ToList(), "ID", "BatchNumber");
            ViewData["BatchNumberdd"] = BatchNumberdd;
        }
        public void BindWorkOrderNumberDropDown()
        {
            var model = _repositoryRR.Getall();
            var WorkOrderNumberdd = new SelectList(model.ToList(), "ID", "WorkOrderNumber");
            ViewData["WorkOrderNumberdd"] = WorkOrderNumberdd;
        }

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

        public JsonResult BatchNumber(string WOId)
        {
            var BatchNumberID = Convert.ToInt32(WOId);
            var result = _finishedGoodSeriesRepository.GetBatchNo(BatchNumberID);
            return Json(result);
        }

        #endregion

    }
}