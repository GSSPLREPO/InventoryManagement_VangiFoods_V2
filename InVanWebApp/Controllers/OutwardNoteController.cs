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
                        if (model.txtItemDetails == null || model.txtItemDetails == "[]]")
                        {
                            TempData["Success"] = "<script>alert('No item is isssued! Material issue note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();

                            model.OutwardDate = DateTime.Today;
                            return View(model);
                        }
                        else
                        {
                            response = _repository.Insert(model);
                            if (response.Status)
                                TempData["Success"] = "<script>alert('Material issue note is created successfully!');</script>";
                            else
                            {
                                TempData["Success"] = "<script>alert('Error! Material issue note cannot be created!');</script>";
                                BindLocationName();
                                GenerateDocumentNo();

                                model.OutwardDate = DateTime.Today;
                                return View(model);
                            }
                        }

                        return RedirectToAction("Index", "IssueNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();

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

                model.OutwardDate = DateTime.Today;
                return View(model);
            }
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
    }
}