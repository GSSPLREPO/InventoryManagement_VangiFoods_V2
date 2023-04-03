using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        #region Insert function
        /// <summary>
        /// Date: 2 March '23
        /// Rahul: Rendered the user to the add Product items form 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProductItems() 
        {
            if (Session[ApplicationSession.USERID] != null)
            {                
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 2 March '23
        /// Rahul: Pass the data to the repository for insertion from it's view. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductItems(ProductMasterBO model) 
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productMasterRepository.Insert(model); 
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Product inserted successfully!');</script>";
                        else
                            TempData["Success"] = "<script>alert('Duplicate product! Can not be inserted!');</script>";

                        return RedirectToAction("Index", "ProductMaster");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return RedirectToAction("Index", "ProductMaster"); 
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Success"] = "<script>alert('Something went wrong!');</script>";
                log.Error(ex.Message, ex);
            }            
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 2 March '23 
        ///Rahul: Rendered the user to the edit page with details of a perticular record.  
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductItem(int Product_ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ProductMasterBO model = _productMasterRepository.GetById(Product_ID); 
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul:  Pass the data to the repository for updating that record. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductItem(ProductMasterBO model) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                ResponseMessageBO response = new ResponseMessageBO();
                try
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productMasterRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Product updated successfully!');</script>";

                        else
                            TempData["Success"] = "<script>alert('Duplicate product! Can not be updated!');</script>";

                        return RedirectToAction("Index", "ProductMaster");
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    TempData["Success"] = "<script>alert('Error while update!');</script>";
                    return RedirectToAction("Index", "ProductMaster");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 2 March '23
        /// Rahul: Delete the perticular record 
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteProductItem(int Product_ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                int userId = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _productMasterRepository.Delete(Product_ID, userId);
                TempData["Success"] = "<script>alert('Product deleted successfully!');</script>";
                return RedirectToAction("Index", "ProductMaster");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

    }
}