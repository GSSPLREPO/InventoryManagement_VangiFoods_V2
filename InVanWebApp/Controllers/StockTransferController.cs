using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class StockTransferController : Controller
    {
        private IStockTransferRepository _stockTransferRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockTransferController));

        #region Initializing constructor
        /// <summary>
        /// Rahul: Constructor without parameter
        /// </summary>
        public StockTransferController()
        {
            _stockTransferRepository = new StockTransferRepository();
        }
        /// <summary>
        /// Rahul: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param> 
        public StockTransferController(IStockTransferRepository stockTransferRepository)
        {
            _stockTransferRepository = stockTransferRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: StockTransfer
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _stockTransferRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion
        
        [HttpGet]
        public ActionResult AddStockTransfer()
        {
            //return View();

            if (Session[ApplicationSession.USERID] != null)
            {
                BindFromLocationName();
                BindToLocationName();
                //Binding item grid with sell type item.
                var itemList = _stockTransferRepository.GetItemListForDD();
                var dd = new SelectList(itemList.ToList(), "ItemId", "Item_Code");
                ViewData["itemListForDD"] = dd;

                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        
        #region Bind dropdowns From Location Name 
        public void BindFromLocationName()
        {
            var result = _stockTransferRepository.GetFromLocationNameList();
            var resultList = new SelectList(result.ToList(), "FromLocationId", "FromLocationName");
            ViewData["FromLocationName"] = resultList;
        }
        #endregion

        #region Bind dropdowns Location Master          
        public JsonResult BindFromLocationMaster(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _stockTransferRepository.GetFromLocationMasterList(Id);
            return Json(result);
        }
        #endregion 

        #region Bind dropdowns From Location Name 
        public void BindToLocationName()
        {
            var result = _stockTransferRepository.GetToLocationNameList();
            var resultList = new SelectList(result.ToList(), "ToLocationId", "ToLocationName");
            ViewData["ToLocationName"] = resultList;
        }
        #endregion

        #region Function for get item details
        public JsonResult GetitemDetails(string id, string locationId, string CreatedDate)
        {
            var itemId = Convert.ToInt32(id);
            var locationID = Convert.ToInt32(locationId);
            var createdDate = Convert.ToDateTime(CreatedDate);            ///added 
            var itemDetails = _stockTransferRepository.GetItemDetails(itemId, locationID, createdDate);
            return Json(itemDetails);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStockTransfer(StockTransferBO model, HttpPostedFileBase Signature)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (Signature != null) ///added
                    {
                        UploadSignature(Signature);

                        model.Signature = Signature.FileName.ToString();

                    }
                    else
                        model.Signature = null; ///added

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _stockTransferRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Stock Transfer inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Stock Transfer! Can not be inserted!');</script>";
                            BindFromLocationName();
                            BindToLocationName();
                            UploadSignature(Signature); ///added
                            return View(model);
                        }
                        return RedirectToAction("Index", "StockTransfer");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindFromLocationName();
                        BindToLocationName();
                        UploadSignature(Signature); ///added
                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View();
        }
        #endregion

        #region Bind dropdowns From Location Wise Item Code 
        public JsonResult BindLocationWiseItemCode(string locationId)
        {
            var locationID = Convert.ToInt32(locationId);
            var itemDetails = _stockTransferRepository.GetBindLocationWiseItemCodeList(locationID);
            return Json(itemDetails);
        }
        #endregion

        #region Function for uploading the signature
        /// <summary>
        /// Date: 09 Dec 2022 
        /// Rahul: Upload Signature File.  
        /// </summary>
        /// <returns></returns>

        public void UploadSignature(HttpPostedFileBase Signature)
        {
            if (Signature != null)
            {
                string SignFilename = Signature.FileName;
                SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                Signature.SaveAs(SignFilename);

            }
        }

        #endregion

    }
}