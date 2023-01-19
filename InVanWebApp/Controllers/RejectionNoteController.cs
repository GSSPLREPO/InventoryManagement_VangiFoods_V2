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
    public class RejectionNoteController : Controller
    {
        private IRejectionNoteRepository _RejectionNoteRepository;
        private IInwardQCSortingRepository _QCRepository;
        private IGRNRepository _GRNrepository; ///Rahul Added 13-01-2023.
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Constructor(s)

        /// <summary>
        /// Raj: Constructor without parameters
        /// </summary>
        public RejectionNoteController()
        {
            _RejectionNoteRepository = new RejectionNoteRepository();
            _QCRepository = new InwardQCSortingRepository();
            _GRNrepository = new GRNRepository(); ///Rahul Added 13-01-2023. 
        }

        /// <summary>
        /// Raj: Constructor With Parameters for initalizing objects.
        /// </summary>
        /// <param name="RejectionNoteRepository"></param>
        public RejectionNoteController(IRejectionNoteRepository RejectionNoteRepository,IInwardQCSortingRepository QCRepository)
        {
            _RejectionNoteRepository = RejectionNoteRepository;
            _QCRepository = QCRepository;
            _GRNrepository = new GRNRepository(); ///Rahul Added 13-01-2023. 
        }
        #endregion

        #region Bind Grid
        // GET: RejectionNote 
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _RejectionNoteRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");                        
        }
        #endregion

        #region Insert Rejection Note
        /// <summary>
        /// Raj: Render View for the Add Rejection Note Details. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRejectionNote() 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                //BindInwardNoNumber();
                BindInwardNumber();  ///Rahul Added 13-01-2023.
                RejectionNoteBO model = new RejectionNoteBO();
                model.NoteDate = DateTime.UtcNow;                   
                //==========Document number for Rejection note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                var DocumentNumber = objDocNo.GetDocumentNo(5);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Raj: Bind All Inward QC behalf of Inward Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetInwardQCById(int id)
        {
            var inwardItems = _QCRepository.GetInwDetailsById(id);
            return Json(inwardItems);
        }
        #endregion
        ///Rahul Added 13-01-2023.
        #region Get Inward number details
        public JsonResult BindInwardDetails(string id)
        {
            int IQCId = 0;
            if (id != "" && id != null)
                IQCId = Convert.ToInt32(id);

            var result = _RejectionNoteRepository.GetInwardDetailsById(IQCId);
            return Json(result);
        }

        #endregion
        ///Rahul Added 13-01-2023. 
        #region Bind dropdown of Inward Number
        public void BindInwardNumber()
        {

            var result = _GRNrepository.GetInwardNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwardNumber"] = resultList;
        }
        
        //public void BindInwardNoNumber()
        //{
        //    var result = _QCRepository.GetInwNumberForDropdown();
        //    var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
        //    ViewData["InwNumberAndId"] = resultList;
        //}
        #endregion



        #region 
        /// <summary>
        /// Rahul: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRejectionNote(RejectionNoteBO model) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        response = _RejectionNoteRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Rejection Note insert successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Rejection Note! Can not be completed!');</script>";
                            //BindInwardNoNumber();
                            BindInwardNumber(); ///Rahul Added 13-01-2023.
                            model.NoteDate = DateTime.Today;
                            //==========Document number for Rejection note============//
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                            var DocumentNumber = objDocNo.GetDocumentNo(5);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "RejectionNote"); //

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        //BindInwardNoNumber();
                        BindInwardNumber(); ///Rahul Added 13-01-2023.
                        model.NoteDate = DateTime.Today;
                        //==========Document number for Rejection note============//
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
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

                //BindInwardNoNumber();
                BindInwardNumber(); ///Rahul Added 13-01-2023.
                model.NoteDate = DateTime.Today;
                //==========Document number for Rejection note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=5 i.e. for generating the Rejection Note (logic is in SP).====// 
                var DocumentNumber = objDocNo.GetDocumentNo(5); 
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region This method is for View the Rejection Note Details    
        /// <summary>
        /// Created By: Rahul
        /// Created Date : 05-01-2023. 
        /// Description: This method responsible for View of Rejection Note details. 
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        [HttpGet] 
        public ActionResult ViewRejectionNote(int RejectionID)  
        {
            if (Session[ApplicationSession.USERID] != null)               
                {
                //Binding item grid.             
                RejectionNoteBO model = _RejectionNoteRepository.GetRejectionNoteById(RejectionID);
                return View(model);
            }
            else  
            return RedirectToAction("Index", "Login");
        }
        #endregion


        #region Delete function
        /// <summary>
        /// Date: 18 Jan'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="RejectionID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteRejectionNote(int RejectionID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _RejectionNoteRepository.Delete(RejectionID, userID);   
                TempData["Success"] = "<script>alert('Rejection Note deleted successfully!');</script>";
                return RedirectToAction("Index", "RejectionNote"); 
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion



    }
}