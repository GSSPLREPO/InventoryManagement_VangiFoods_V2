using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;
using InVanWebApp_BO;
using System.IO;

namespace InVanWebApp.Controllers
{
    public class DeliveryChallanController : Controller
    {
        private IDeliveryChallanRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(DeliveryChallanController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 27 Jan'23
        /// </summary>
        public DeliveryChallanController()
        {
            _repository = new DeliveryChallanRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 27 Jan'23
        /// </summary>
        /// <param name="deliveryChallanRepository"></param>
        public DeliveryChallanController(IDeliveryChallanRepository deliveryChallanRepository)
        {
            _repository = deliveryChallanRepository;
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
        /// Farheen: Rendered the user to the add outward note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDeliveryChallan()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindSONumber();

                DeliveryChallanBO model = new DeliveryChallanBO();
                model.DeliveryChallanDate = DateTime.Today;
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
        public ActionResult AddDeliveryChallan(DeliveryChallanBO model, HttpPostedFileBase Signature)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (Signature != null)
                    {
                        UploadSignature(Signature);
                        model.Signature = Signature.FileName.ToString();
                    }
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Delivery Challan is created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Delivery Challan cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindSONumber();

                            model.DeliveryChallanDate = DateTime.Today;
                            return View(model);
                        }


                        return RedirectToAction("Index", "DeliveryChallan");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindSONumber();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.DeliveryChallanDate = DateTime.Today;
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
                BindSONumber();
                model.DeliveryChallanDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 29 Jan'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteDeliveryChallan(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Delivery challan deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "DeliveryChallan");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Delivery Challan
        [HttpGet]
        public ActionResult ViewDeliveryChallan(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                DeliveryChallanBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Bind dropdowns 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }
        public void BindSONumber()
        {
            var result = _repository.GetSONumberList();
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONo");
            ViewData["SONumberList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=14 i.e. for generating the Delivery challan (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(14);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        #endregion

        #region Fetch SO details for DeliveryChallan
        public JsonResult GetSODetails(string id)
        {
            int SOId = 0;
            if (id != "" && id != null)
                SOId = Convert.ToInt32(id);

            var result = _repository.GetSODetailsById(SOId);
            return Json(result);
        }
        #endregion

        #region Function for uploading the signature
        /// <summary>
        /// Date: 30 Jan 2023
        /// Farheen: Upload Signature File items.
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