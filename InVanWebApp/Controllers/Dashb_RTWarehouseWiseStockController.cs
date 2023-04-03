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

        #region 

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
            int LocationId = 0, itemId=0;
            if (id != null && id != "")
                LocationId = Convert.ToInt32(id);
            if(ItemId!=null && ItemId!="")
                itemId = Convert.ToInt32(ItemId);

            string jsonstring = string.Empty;

            var result = _repository.GetDashboardData(LocationId,itemId);
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

        public JsonResult GetDashboardDataReorderPointOfStocks(string id="")
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

        #region Bind FIFO System

        public ActionResult FIFOSystem()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("FIFOSystem", "Login");
            DashboardBO model = new DashboardBO();
            model.toDate = DateTime.Now;
            model.fromDate = /*DateTime.Now;*/Convert.ToDateTime("01-11-2022");
            BindLocationDropdown();
            BindItemDropDown();
            return View(model);
        }

        public JsonResult GetFIFOSystem(string id, string ItemId, DateTime fromDate, DateTime toDate )
        {
            int LocationId = 0, itemId = 0;
            if (id != null && id != "")
                LocationId = Convert.ToInt32(id);
            if (ItemId != null && ItemId != "")
                itemId = Convert.ToInt32(ItemId);

            DashboardBO model = new DashboardBO();
            model.fromDate = DateTime.Today;
            model.toDate = DateTime.Today;

            string jsonstring = string.Empty;

            var result = _repository.GetFIFOSystem(fromDate, toDate, itemId, LocationId);
            jsonstring = JsonConvert.SerializeObject(result);

            var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #endregion

    }
}