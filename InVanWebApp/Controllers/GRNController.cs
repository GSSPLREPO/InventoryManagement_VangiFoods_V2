using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class GRNController : Controller
    {
        private IGRNRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public GRNController()
        {
            _repository = new GRNRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public GRNController(IGRNRepository gRNRepository)
        {
            _repository = gRNRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _repository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add GRN master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddGRN()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindInwardNumber();
                BindLocationName();
                GRN_BO model = new GRN_BO();
                model.GRNDate = DateTime.Today;
                //==========Document number for GRN note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=5 i.e. for generating the GRN note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(5);
                ViewData["DocumentNo"] = DocumentNumber;

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
        public ActionResult AddGRN(GRN_BO model)
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
                            TempData["Success"] = "<script>alert('GRN created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate GRN note! Can not be inserted!');</script>";
                            BindInwardNumber();
                            BindLocationName();
                            model.GRNDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(5);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "GRN");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindInwardNumber();
                        BindLocationName();
                        model.GRNDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(5);
                        ViewData["DocumentNo"] = DocumentNumber;

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

                BindInwardNumber();
                BindLocationName();
                model.GRNDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(5);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region Get Inward number details
        public JsonResult BindInwardDetails(string id)
        {
            int InwId = 0;
            if (id != "" && id != null)
                InwId = Convert.ToInt32(id);
            
            var result = _repository.GetInwardDetailsById(InwId);
            return Json(result);
        }

        #endregion

        #region Bind dropdowns 
        public JsonResult BindLocationMaster(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _purchaseOrderRepository.GetLocationMasterList(Id);
            return Json(result);
        }
        public void BindInwardNumber()
        {

            var result = _repository.GetInwardNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwardNumber"] = resultList;
        }

        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        #endregion

    }
}