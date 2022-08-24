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
    public class CompanyController : Controller
    {
        private ICompanyRepository _companyRepository;
        private static ILog log = LogManager.GetLogger(typeof(CompanyController));

        #region Initializing constructor
        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public CompanyController()
        {
            _companyRepository = new CompanyRepository();
        }

        /// <summary>
        /// Date: 23 Aug'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="supplierCustomerRepository"></param>
        public CompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
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
            var model = _companyRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 24 Aug'22
        /// Farheen: Rendered the user to the add company form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCompany()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompany(CompanyBO model)
        {
            try
            {
                var flag = false;
                if (ModelState.IsValid)
                {
                    flag = _companyRepository.Insert(model);
                    if (flag)
                        TempData["Success"] = "<script>alert('Company inserted successfully!');</script>";
                    else
                        TempData["Success"] = "<script>alert('Error while insertion!');</script>";

                    return RedirectToAction("Index", "Company");
                }
                return View();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                return RedirectToAction("Index", "Location");
            }
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
            //CompanyBO model = _companyRepository.GetById(ID);
            //return View(model);
            _companyRepository.Delete(ID);
            TempData["Success"] = "<script>alert('Supplier/Customer deleted successfully!');</script>";
            return RedirectToAction("Index", "Item");
        }

        #endregion
    }
}