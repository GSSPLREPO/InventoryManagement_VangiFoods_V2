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


namespace InVanWebApp.Controllers
{
    public class InwardQCSortingController : Controller
    {
        private IInwardQCSortingRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(InwardQCSortingController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public InwardQCSortingController()
        {
            _repository = new InwardQCSortingRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public InwardQCSortingController(InwardQCSortingRepository inwardQCSortingRepository)
        {
            _repository = inwardQCSortingRepository;
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

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add inward QC sorting.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInwardQC()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindInwardNoNumber();

                InwardQCBO model = new InwardQCBO();
                model.InwardQCDate = DateTime.Today;
                //==========Document number for Inward note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=4 i.e. for generating the Inward QC sorting (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(4);
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
        public ActionResult AddInwardQC(InwardQCBO model)
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
                            TempData["Success"] = "<script>alert('Inward QC done successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Inward sorting! Can not be completed!');</script>";
                            BindInwardNoNumber();
                            model.InwardQCDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(4);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "InwardQCSorting");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindInwardNoNumber();
                        model.InwardQCDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(4);
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

                BindInwardNoNumber();
                model.InwardQCDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(4);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion


        #region Bind dropdown of Inward Number
        public void BindInwardNoNumber()
        {
            var result = _repository.GetInwNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwNumberAndId"] = resultList;
        }
        #endregion

        #region Bind all Inward note and it's details including which item inward
        public JsonResult BindInwDetails(string id)
        {
            int InwdId = 0;
            if (id != "" && id != null)
                InwdId = Convert.ToInt32(id);
                
            var result = _repository.GetInwDetailsById(InwdId);
            return Json(result);
        }
        #endregion

    }
}