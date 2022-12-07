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
    public class IndentController : Controller
    {
        private IIndentRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(IndentController));

        #region Initializing constructor
        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public IndentController()
        {
            _repository = new IndentRepository();
        }

        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public IndentController(IndentRepository indentRepository)
        {
            _repository = indentRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
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