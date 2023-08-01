using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class IntermediateRejectionNoteController : Controller
    {
        private IIntermediateRejectionNoteRepository _intermediateRejectionNoteRepository;
        private IIssueNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IStockAdjustmentRepository _stockAdjustmentRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor
        /// <summary>
        /// Created By: Rahul
        /// Created Date: 01 Aug'23
        /// </summary>
        public IntermediateRejectionNoteController() 
        {
            _intermediateRejectionNoteRepository = new IntermediateRejectionNoteRepository();
            _repository = new IssueNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository(); 
            _stockAdjustmentRepository = new StockAdjustmentRepository();
        }

        /// <summary>
        /// Created By: Rahul
        /// Created Date: 01 Aug'23
        /// </summary>
        /// <param name="intermediateRejectionNoteRepository"></param> 
        public IntermediateRejectionNoteController(IIntermediateRejectionNoteRepository intermediateRejectionNoteRepository)
        {
            _intermediateRejectionNoteRepository = intermediateRejectionNoteRepository; 
        }
        #endregion


        #region Bind Grid
        // GET: IntermediateRejectionNote
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _intermediateRejectionNoteRepository.GetAll();
            return View(model); 
        }
        #endregion

        #region Bind dropdowns 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }
        public void BindUserName()
        {
            var result = _repository.GetUserNameList();
            var resultList = new SelectList(result.ToList(), "UserId", "Username");
            ViewData["UserList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            //==========Document number for IntermediateRejectionNote============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=23 i.e. for generating the IntermediateRejectionNote (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(23);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        public JsonResult GetItemList(string id)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);

            var result = _stockAdjustmentRepository.GetItemListByLocationId(Location_Id);
            return Json(result);
        }
        #endregion


        #region Insert functions
        /// <summary>
        /// Rahul: Rendered the user to the add Stock adjustment note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddIntermediateRejectionNote()  
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindUserName();

                IntermediateRejectionNoteBO model = new IntermediateRejectionNoteBO();
                model.Intermediate_Rej_NoteDate = DateTime.Today;
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
        public ActionResult AddIntermediateRejectionNote(IntermediateRejectionNoteBO model) 
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
                            TempData["Success"] = "<script>alert('No item is rejected! Intermediate rejection note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindUserName();

                            model.Intermediate_Rej_NoteDate = DateTime.Today;
                            return View(model);
                        }
                        else
                        {
                            response = _intermediateRejectionNoteRepository.Insert(model); 
                            if (response.Status)
                                TempData["Success"] = "<script>alert('Intermediate Rejection note is created successfully!');</script>";
                            else
                            {
                                TempData["Success"] = "<script>alert('Error! Intermediate Rejection note cannot be created!');</script>";
                                BindLocationName();
                                GenerateDocumentNo();
                                BindUserName();

                                model.Intermediate_Rej_NoteDate = DateTime.Today;
                                return View(model);
                            }
                        }

                        return RedirectToAction("Index", "IntermediateRejectionNote"); 

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindUserName();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.Intermediate_Rej_NoteDate = DateTime.Today;
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
                BindUserName();

                model.Intermediate_Rej_NoteDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

    }
}