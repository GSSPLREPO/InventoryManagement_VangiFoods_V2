using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;
using Newtonsoft.Json;

namespace InVanWebApp.Controllers
{
    public class NotificationsController : Controller
    {
        private INotificationsRepository _notificationsRepository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(NotificationsController)); 

        #region Initializing constructor
        /// <summary>
        /// Date: 26 Apr'22
        /// Rahul: Constructor without parameter
        /// </summary>
        public NotificationsController() 
        {
            _notificationsRepository = new NotificationsRepository(); 
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Date: 26 Apr'22
        /// Rahul: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public NotificationsController(NotificationsRepository notificationsRepository) 
        {
            _notificationsRepository = notificationsRepository; 
        }
        #endregion

        #region GetNotification 
        //Added 'GetNotification' 25-04-23 start.
        //[HttpGet]
        //public JsonResult GetNotifications()
        //{
        //    List<StockMasterBO> lstDataSubmit = new List<StockMasterBO>();

        //    /// Should update from DB  
        //    ///  
        //    ///e.g. Generating Notification manually  
        //    var No = 10;
        //    while (No != 0)
        //    {
        //        lstDataSubmit.Add(new StockMasterBO()
        //        {
        //            Notification = "This is dynamic notification..." + No,
        //            LastUpdated = DateTime.Now.ToString("ss") + " seconds ago..."

        //        });
        //        No--;
        //    }
        //    return Json(lstDataSubmit, JsonRequestBehavior.AllowGet);
        //}
        //Added 'GetNotification' 25-04-23 end.

        [HttpGet]
        public JsonResult GetNotifications(string id = "")
        {
            var ItemId = 0;
            if (id != "")
                ItemId = Convert.ToInt32(id);

            string jsonstring = string.Empty;

            var result = _notificationsRepository.GetReorderPointOnMinStock(ItemId);
            //jsonstring = JsonConvert.SerializeObject(result);

            //var jsonResult = Json(jsonstring, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}