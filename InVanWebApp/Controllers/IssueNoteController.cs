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
    public class IssueNoteController : Controller
    {
        private IIssueNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IStockAdjustmentRepository _stockAdjustmentRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 24 Jan'23
        /// </summary>
        public IssueNoteController()
        {
            _repository = new IssueNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _stockAdjustmentRepository = new StockAdjustmentRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 24 Jan'23
        /// </summary>
        /// <param name="stockAdjustmentRepository"></param>
        public IssueNoteController(IIssueNoteRepository issueNoteRepository)
        {
            _repository = issueNoteRepository;
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
        /// Farheen: Rendered the user to the add Stock adjustment note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddIssueNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindUserName();

                IssueNoteBO model = new IssueNoteBO();
                model.IssueNoteDate = DateTime.Today;
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
        public ActionResult AddIssueNote(IssueNoteBO model)
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
                            TempData["Success"] = "<script>alert('No item is issued! Material issue note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindUserName();

                            model.IssueNoteDate = DateTime.Today;
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
                                BindUserName();

                                model.IssueNoteDate = DateTime.Today;
                                return View(model);
                            }
                        }

                        return RedirectToAction("Index", "IssueNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindUserName();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.IssueNoteDate = DateTime.Today;
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

                model.IssueNoteDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 24 Jan'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteIssueNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Issue note deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "IssueNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Issue Note
        [HttpGet]
        public ActionResult ViewIssueNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                IssueNoteBO model = _repository.GetById(ID);
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
        public void BindUserName()
        {
            var result = _repository.GetUserNameList();
            var resultList = new SelectList(result.ToList(), "UserId", "Username");
            ViewData["UserList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=13 i.e. for generating the Issue Note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(13);
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
    }
}