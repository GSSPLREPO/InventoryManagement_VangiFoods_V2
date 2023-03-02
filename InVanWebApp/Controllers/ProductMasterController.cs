using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class ProductMasterController : Controller
    {
        private IProductMasterRepository _productMasterRepository;        
        private static ILog log = LogManager.GetLogger(typeof(ProductMasterRepository));

        #region Initializing constructor
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor without parameter 
        /// </summary>
        public ProductMasterController()
        {
            _productMasterRepository = new ProductMasterRepository(); 
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductMasterController(ProductMasterRepository productRepository)
        {
            _productMasterRepository = productRepository; 
        }
        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 27 Feb '23
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: ProductMaster
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _productMasterRepository.GetAll(); 
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


    }
}