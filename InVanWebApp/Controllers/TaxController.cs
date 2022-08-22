using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;

namespace InVanWebApp.Controllers
{
    public class TaxController : Controller
    {
        private ITaxRepository _taxRepository;

        #region Initializing constructor
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public TaxController()
        {
            _taxRepository = new TaxRepository();
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="taxRepository"></param>
        public TaxController(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 19 Aug'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
        [HttpGet]
        public ActionResult Index()
        {
            var model = _taxRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Rendered the user to the add location form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddTax()
        {
            return View();
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTax(TaxBO model)
        {
            if (ModelState.IsValid)
            {
                _taxRepository.Insert(model);
                TempData["Success"] = "<script>alert('Tax inserted successfully!');</script>";
                return RedirectToAction("Index", "Tax");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 19 Aug'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTax(int ID)
        {
            TaxBO model = _taxRepository.GetById(ID);
            return View(model);
        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTax(TaxBO model)
        {
            if (ModelState.IsValid)
            {
                _taxRepository.Update(model);
                TempData["Success"] = "<script>alert('Tax updated successfully!');</script>";
                return RedirectToAction("Index", "Tax");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTax(int ID)
        {
            TaxBO model = _taxRepository.GetById(ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            _taxRepository.Delete(ID);
            //_unitRepository.Save();
            TempData["Success"] = "<script>alert('Tax deleted successfully!');</script>";
            return RedirectToAction("Index", "Tax");
        }
        #endregion

    }
}