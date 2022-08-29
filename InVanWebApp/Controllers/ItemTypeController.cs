using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using InVanWebApp.DAL;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using log4net;

namespace InVanWebApp.Controllers
{
    public class ItemTypeController : Controller
    {
        private IItemTypeRepository _itemTypeRepository;
        private static ILog log = LogManager.GetLogger(typeof(ItemTypeController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemTypeController()
        {
            _itemTypeRepository = new ItemTypeRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public ItemTypeController(IItemTypeRepository itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
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
            var model = _itemTypeRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add item type master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItemType()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItemType(ItemTypeBO model)
        {
            try
            {
                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    response = _itemTypeRepository.Insert(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Item type Inserted Successfully!');</script>";
                    else
                        TempData["Success"] = "<script>alert('Duplicate Item type! Can not be inserted!');</script>";

                    return RedirectToAction("Index", "ItemType");

                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItemType(int ID)
        {
            ItemTypeBO model = _itemTypeRepository.GetById(ID);
            return View(model);
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItemType(ItemTypeBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _itemTypeRepository.Update(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Item type updated successfully!');</script>";

                    else
                        TempData["Success"] = "<script>alert('Duplicate Item type! Can not be updated!');</script>";


                    return RedirectToAction("Index", "ItemType");
                }
                else
                    return View(model);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "ItemType");
            }
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteItemType(int ID)
        {
            ItemTypeBO model = _itemTypeRepository.GetById(ID);
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int ID)
        {
            _itemTypeRepository.Delete(ID);
            //_unitRepository.Save();
            TempData["Success"] = "<script>alert('Item type deleted successfully!');</script>";
            return RedirectToAction("Index", "ItemType");
        }
        #endregion

    }
}