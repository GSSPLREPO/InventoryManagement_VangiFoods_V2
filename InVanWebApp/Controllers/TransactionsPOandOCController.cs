using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class TransactionsPOandOCController : Controller
    {
        private ITransactionsPOandOCRepository _transactionsPOandOC;

        #region Initializing constructor
        /// <summary>
        /// Date: 02 june'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public TransactionsPOandOCController()
        {
            _transactionsPOandOC = new TransactionsPOandOCRepository(new InVanDBContext());
        }

        /// <summary>
        /// Date: 02 june'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="addItemRepository"></param>
        public TransactionsPOandOCController(ITransactionsPOandOCRepository transactionsPOandOC)
        {
            _transactionsPOandOC = transactionsPOandOC;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 02 june'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public ActionResult Index()
        {
            var companyList = _transactionsPOandOC.GetCompanyNameForDropDown();
            var dd = new SelectList(companyList.ToList(), "Company_ID", "Name");
            ViewData["company"] = dd;
            var model = _transactionsPOandOC.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function for order confirmation
        /// <summary>
        /// Date: 03 june'22
        /// Farheen: Rendered the user to the add order confirmation form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrderConfirmation()
        {
            //var countryList = _transactionsPOandOC.GetCountryForDropDown();
            //var dd = new SelectList(countryList.ToList(), "CountryID", "CountryName");
            //ViewData["country"] = dd;
            return View();
        }

        /// <summary>
        /// Date: 03 june'22
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrderConfirmation(PurchaseOrder model)
        {
            if (ModelState.IsValid)
            {
                _transactionsPOandOC.Insert(model);
                return RedirectToAction("Index", "TransactionsPOandOC");
            }
            return View();
        }
        #endregion




    }
}