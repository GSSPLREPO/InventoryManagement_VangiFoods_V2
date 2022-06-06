using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Controllers
{
    public class ItemCategoryController : Controller
    {
        private IItemCategoryRepository _itemCategoryRepository;

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemCategoryController()
        {
            _itemCategoryRepository = new ItemCategoryRepository(new InVanDBContext());
        }

        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemCategoryRepository"></param>
        public ItemCategoryController(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item category
        [HttpGet]
        public ActionResult Index()
        {
            var model = _itemCategoryRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add item category master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItemCategory()
        {
            var model = _itemCategoryRepository.GetItemTypeForDropDown();
            var dd = new SelectList(model.ToList(),"ItemID","ItemName");
            ViewData["ItemType"] = dd;
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItemCategory(ItemCategoryMaster model)
        {
            if (ModelState.IsValid)
            {
                _itemCategoryRepository.Insert(model);
                //_unitRepository.Save();
                return RedirectToAction("Index", "ItemCategory");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ItemCategoryID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItemCategory(int ItemCategoryID)
        {
            var ItemType = _itemCategoryRepository.GetItemTypeForDropDown();
            var dd = new SelectList(ItemType.ToList(), "ItemID", "ItemName");
            ViewData["ItemType"] = dd;
            ItemCategoryMaster model = _itemCategoryRepository.GetById(ItemCategoryID);
            return View(model);
        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItemCategory(ItemCategoryMaster model)
        {
            if (ModelState.IsValid)
            {
                _itemCategoryRepository.Udate(model);
                return RedirectToAction("Index", "ItemCategory");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ItemCategoryID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteItemCategory(int ItemCategoryID)
        {
            ItemCategoryMaster model = _itemCategoryRepository.GetById(ItemCategoryID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int ItemCategoryID)
        {
            _itemCategoryRepository.Delete(ItemCategoryID);
            //_unitRepository.Save();
            return RedirectToAction("Index", "ItemCategory");
        }
        #endregion

    }
}