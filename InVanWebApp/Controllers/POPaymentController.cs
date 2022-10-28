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
    public class POPaymentController : Controller
    {
        private IPOPaymentRepository _POPaymentRepository;
        private static ILog log = LogManager.GetLogger(typeof(POPaymentController));

        #region Initializing Cunstructor(s)
        /// <summary>
        /// Raj: Constructor without parameter
        /// </summary>
        public POPaymentController()
        {
            _POPaymentRepository = new POPaymentRepository();
        }
        /// <summary>
        /// Raj: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="pOPaymentRepository"></param>
        public POPaymentController(IPOPaymentRepository pOPaymentRepository)
        {
            _POPaymentRepository = pOPaymentRepository;
        }
        #endregion



        // GET: POPayment
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }


        public ActionResult AddPOPayment()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindPONumbers();
            return View();
        }
        #region Bind PONumbers DropDown
        private void BindPONumbers()
        {
            var result = _POPaymentRepository.GetPONumbers();
            var resultList = new SelectList(result.ToList(), "Key", "Value");
            ViewData["OPNumbers"] = resultList;
        }
        #endregion
    }
}