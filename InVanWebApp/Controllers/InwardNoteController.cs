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
    public class InwardNoteController : Controller
    {
        private IInwardNoteRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public InwardNoteController()
        {
            _repository = new InwardNoteRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public InwardNoteController(IInwardNoteRepository inwardNoteRepository)
        {
            _repository = inwardNoteRepository;
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
    }
}