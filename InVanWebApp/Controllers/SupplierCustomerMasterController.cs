using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Repository.Interface;

namespace InVanWebApp.Controllers
{
    public class SupplierCustomerMasterController : Controller
    {
        private ISupplierCustomerRepository _supplierCustomerRepository;
        private static ILog log = LogManager.GetLogger(typeof(SupplierCustomerMasterController));

        #region Initializing constructor
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public SupplierCustomerMasterController()
        {
            _supplierCustomerRepository = new SupplierCustomerRepository();
        }

        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="supplierCustomerRepository"></param>
        public SupplierCustomerMasterController(SupplierCustomerRepository supplierCustomerRepository)
        {
            _supplierCustomerRepository = supplierCustomerRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 23 Aug'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Supplier/Customer list
        [HttpGet]
        public ActionResult Index()
        {
            var model = _supplierCustomerRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteSupplierCustomer(int ID)
        {
            CompanyBO model = _supplierCustomerRepository.GetById(ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            _supplierCustomerRepository.Delete(ID);
            //_unitRepository.Save();
            TempData["Success"] = "<script>alert('Supplier/Customer deleted successfully!');</script>";
            return RedirectToAction("Index", "Item");
        }
        #endregion
    }
}