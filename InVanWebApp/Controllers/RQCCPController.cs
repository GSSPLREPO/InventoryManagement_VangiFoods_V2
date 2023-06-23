using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using log4net;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using System.IO;

namespace InVanWebApp.Controllers
{
    public class RQCCPController : Controller
    {
        private IRQCCPRepository _RQCCPRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(RQCCPController));

        #region Initializing constructor
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Constructor without parameter
        /// </summary>
        public RQCCPController()
        {
            _RQCCPRepository = new RQCCPRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Constructor with parameters for initializing the interface object.
        /// </summary>
        
        public RQCCPController(RQCCPRepository RQCCPRepository)
        {
            _RQCCPRepository = RQCCPRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Supplier/Customer list
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _RQCCPRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 15 March'23
        /// Charmi: Rendered the user to the add company form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRQCCP()
        {
            if (Session[ApplicationSession.USERID] != null)

            {
                BindItem();
                RQCCPBO model = new RQCCPBO();
                model.Date = DateTime.Today;
                model.TansferTimeintoHoldingSilo = DateTime.Now.ToString("HH:mm");
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Charmi: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRQCCP(RQCCPBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                try
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _RQCCPRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('RQ CCP details Inserted Successfully!');</script>";
                        else
                        {
                            //if (response.ItemName != null || response.ItemName != "") {
                            BindItem();
                            //TempData["Success"] = "<script>alert('Duplicate item details! Can not be inserted!');</script>";
                            //}
                            //else
                            TempData["Success"] = "<script>alert('Duplicate product insertion of the same Raw Batches Number!');</script>";
                            //return View();
                            //return RedirectToAction("AddSILOCCP", "SILOCCP");
                            return RedirectToAction("Index", "RQCCP");
                        }

                        //return RedirectToAction("Index", "RQCCP");
                        return RedirectToAction("Index", "SILOCCP");

                    }

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                    return RedirectToAction("Index", "RQCCP");
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRQCCP(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItem();
                RQCCPBO model = _RQCCPRepository.GetById(Id);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRQCCP(RQCCPBO model)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _RQCCPRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('RQ CCP updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate RQ CCP!');</script>";
                            BindItem();
                            return View();
                        }

                        return RedirectToAction("Index", "RQCCP");
                    }
                    else
                        return View(model);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "RQCCP");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRQCCP(int Id)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                //_RQCCPRepository.Delete(Id, userID);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _RQCCPRepository.Delete(Id, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('RQ CCP deleted successfully!');</script>";
                else
                    //TempData["Success"] = "<script>alert('Error while deleting!');</script>";
                    TempData["Success"] = "<script>alert('Stage-2 Already Done! you need to delete entry from Stage-2!');</script>";

                return RedirectToAction("Index", "SILOCCP");

            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Bind DropDown
        public void BindItem() {

            //Binding item grid with sell type item.
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["itemListForDD"] = dd;

        }

        #endregion
    }
}