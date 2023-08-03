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
    public class MaterialReturnNoteController : Controller
    {
        IMaterialReturnNoteRepository _materialReturnNoteRepository;
        private IIssueNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IStockAdjustmentRepository _stockAdjustmentRepository;
        private static ILog log = LogManager.GetLogger(typeof(MaterialReturnNoteController));

        #region Initializing Constructor
        /// <summary>
        /// Created By: Rahul
        /// Created Date: 03 Aug'23
        /// </summary>
        public MaterialReturnNoteController()
        {
            _materialReturnNoteRepository = new MaterialReturnNoteRepository();
            _repository = new IssueNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _stockAdjustmentRepository = new StockAdjustmentRepository();
        }

        /// <summary>
        /// Created By: Rahul
        /// Created Date: 03 Aug'23
        /// </summary>
        /// <param name="materialReturnNoteRepository"></param> 
        public MaterialReturnNoteController(IMaterialReturnNoteRepository materialReturnNoteRepository) 
        {
            _materialReturnNoteRepository = materialReturnNoteRepository;
        }
        #endregion

        #region Bind Grid
        // GET: MaterialReturnNote
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _materialReturnNoteRepository.GetAll();
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
            //==========Document number for MaterialReturnNote============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=24 i.e. for generating the MaterialReturnNote (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(24);
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
        /// Date: 3 Aug'23 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddMaterialReturnNote() 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindUserName();

                MaterialReturnNoteBO model = new MaterialReturnNoteBO();
                model.MaterialReturnNoteDate = DateTime.Today;
                return View(model); 
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// Date: 3 Aug'23 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMaterialReturnNote(MaterialReturnNoteBO model) 
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

                            model.MaterialReturnNoteDate = DateTime.Today;
                            return View(model);
                        }
                        else
                        {
                            response = _materialReturnNoteRepository.Insert(model); 
                            if (response.Status)
                                TempData["Success"] = "<script>alert('Material Return Note is created successfully!');</script>";
                            else
                            {
                                TempData["Success"] = "<script>alert('Error! Material Return Note cannot be created!');</script>";
                                BindLocationName();
                                GenerateDocumentNo();
                                BindUserName();

                                model.MaterialReturnNoteDate = DateTime.Today;
                                return View(model);
                            }
                        }

                        return RedirectToAction("Index", "MaterialReturnNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindUserName();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.MaterialReturnNoteDate = DateTime.Today;
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

                model.MaterialReturnNoteDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Raahul: Delete the perticular record 
        /// Date: 3 Aug'23
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteMaterialReturnNote(int ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _materialReturnNoteRepository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Material Return Note deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "MaterialReturnNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

    }
}