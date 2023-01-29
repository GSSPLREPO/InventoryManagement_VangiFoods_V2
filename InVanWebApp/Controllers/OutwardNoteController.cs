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

namespace InVanWebApp.Controllers
{
    public class OutwardNoteController : Controller
    {
        private IOutwardNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(OutwardNoteController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 27 Jan'23
        /// </summary>
        public OutwardNoteController()
        {
            _repository = new OutwardNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 27 Jan'23
        /// </summary>
        /// <param name="outwardNoteRepository"></param>
        public OutwardNoteController(IOutwardNoteRepository outwardNoteRepository)
        {
            _repository = outwardNoteRepository;
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
        public ActionResult AddOutwardNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindSONumber();

                OutwardNoteBO model = new OutwardNoteBO();
                model.OutwardDate = DateTime.Today;
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
        public ActionResult AddOutwardNote(OutwardNoteBO model)
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
                            TempData["Success"] = "<script>alert('Outward note is created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Outward note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindSONumber();

                            model.OutwardDate = DateTime.Today;
                            return View(model);
                        }


                        return RedirectToAction("Index", "OutwardNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindSONumber();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.OutwardDate = DateTime.Today;
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
                model.OutwardDate = DateTime.Today;
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
        public ActionResult DeleteOutwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Outward note deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "OutwardNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Outward Note
        [HttpGet]
        public ActionResult ViewOutwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                OutwardNoteBO model = _repository.GetById(ID);
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
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=14 i.e. for generating the Outward Note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(14);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        //public JsonResult GetItemList(string id)
        //{
        //    int Location_Id = 0;
        //    if (id != "" && id != null)
        //        Location_Id = Convert.ToInt32(id);

        //    var result = _stockAdjustmentRepository.GetItemListByLocationId(Location_Id);
        //    return Json(result);
        //}
        #endregion

        #region Fetch SO details for Outward note
        public JsonResult GetSODetails(string id)
        {
            int SOId = 0;
            if (id != "" && id != null)
                SOId = Convert.ToInt32(id);

            var result = _repository.GetSODetailsById(SOId);
            return Json(result);
        }
        #endregion
    }
}