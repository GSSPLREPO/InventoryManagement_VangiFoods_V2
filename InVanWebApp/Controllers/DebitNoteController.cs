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
    public class DebitNoteController : Controller
    {
        private IDebitNoteRepository _DebitNoteRepository;
        private IInwardQCSortingRepository _QCRepository;
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Constructor(s)

        /// <summary>
        /// Raj: Constructor without parameters
        /// </summary>
        public DebitNoteController()
        {
            _DebitNoteRepository = new DebitNoteRepository();
            _QCRepository = new InwardQCSortingRepository();
        }

        /// <summary>
        /// Raj: Constructor With Parameters for initalizing objects.
        /// </summary>
        /// <param name="debitNoteRepository"></param>
        public DebitNoteController(IDebitNoteRepository debitNoteRepository,IInwardQCSortingRepository QCRepository)
        {
            _DebitNoteRepository = debitNoteRepository;
            _QCRepository = QCRepository;
        }
        #endregion

        #region Bind Grid
        // GET: DebitNote
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
        #endregion

        #region Add Debit/Rejection Note
        /// <summary>
        /// Raj: Render View for the Add Debit/Rejection Note Details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDebitNote()
        {
            DebitNoteBO model = new DebitNoteBO();
            model.NoteDate = DateTime.UtcNow;
            BindInwardNumbers();
            return View(model);
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

        #region Bind Inward Note Number DropDown
        public void BindInwardNumbers()
        {
            var result = _DebitNoteRepository.GetInwardNoteNumbers();
            var resultList = new SelectList(result.ToList(), "Key", "Value");
            ViewData["InwardNoteNumbers"] = resultList;
        }
        #endregion
    }
}