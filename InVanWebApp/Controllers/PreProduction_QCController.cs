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
    public class PreProduction_QCController : Controller
    {
        private IPreProduction_QCRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(PreProduction_QCController));

        #region Initializing constructor
        /// <summary>
        /// Snehal: Constructor without parameter
        /// </summary>
        public PreProduction_QCController()
        {
            _repository = new PreProduction_QCRepository();
        }
        /// <summary>
        /// Snehal: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public PreProduction_QCController(PreProduction_QCRepository preProduction_QCRepository)
        {
            _repository = preProduction_QCRepository;
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
        /// Snehal: Rendered the user to the add Pre Production QC .
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPreProductionQC()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindPMINoteNo();

                PreProduction_QCBO model = new PreProduction_QCBO();
                model.QCDate = DateTime.Today;
                //==========Document number for Material Issue note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=4 i.e. for generating the Material Issue QC sorting (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(19);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Snehal: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPreProductionQC(PreProduction_QCBO model)
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
                            TempData["Success"] = "<script>alert('Pre Production QC done successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Pre Production QC! Can not be completed!');</script>";
                            BindPMINoteNo();
                            model.QCDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(19);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "PreProduction_QC");

                    }
                    else
                    {
                        //TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindPMINoteNo();
                        model.QCDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(19);
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

                BindPMINoteNo();
                model.QCDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(19);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 24-03-2023
        /// Snehal: Delete the particular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeletePreProductionQC(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Pre Production QC deleted successfully!');</script>";
                return RedirectToAction("Index", "PreProduction_QC");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dropdown of Note Number
        public void BindPMINoteNo()
        {
            var result = _repository.GetQCNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "ProductionMaterialIssueNoteNo");
            ViewData["PONumberAndId"] = resultList;
        }
        #endregion

     
        #region Bind all Production material note and it's details including which item Pre Production
        public JsonResult ProdIndent_NoDeatils(string id, string PPINote_Id = null)
        {
            int PPQCId = 0;
            int PPNote_Id = 0;
            if (id != "" && id != null)
                PPQCId = Convert.ToInt32(id);
            if (PPINote_Id != "" && PPINote_Id != null)
                PPNote_Id = Convert.ToInt32(PPINote_Id);

            var result = _repository.GetProdIndent_NoDeatils(PPQCId, PPNote_Id);
            return Json(result);
        }
        #endregion

        #region This method is for View the Pre Production note
        [HttpGet]
        public ActionResult ViewPreProduction(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                PreProduction_QCBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}