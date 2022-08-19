using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp_BO;
//using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class ItemController : Controller
    {
        private IItemRepository _iItemRepository;

        #region Initializing constructor
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemController()
        {
            _iItemRepository = new ItemRepository();
        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public ItemController(ItemRepository itemRepository)
        {
            _iItemRepository = itemRepository;
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
            var model = _iItemRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Rendered the user to the add item form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItems()
        {
            var itemCategory = _iItemRepository.GetItemCategoryForDropDown();
            var dd = new SelectList(itemCategory.ToList(), "ItemCategoryID", "ItemCategoryName");
            ViewData["ItemCategory"] = dd;
            var itemType = _iItemRepository.GetItemTypeForDropdown();
            var dd1 = new SelectList(itemType.ToList(), "ID", "ItemType");
            ViewData["ItemType"] = dd1;
            return View();
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItems(ItemBO model)
        {
            if (ModelState.IsValid)
            {
                _iItemRepository.Insert(model);
                return RedirectToAction("Index", "Item");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="Item_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItem(int Item_ID)
        {
            var itemCategory = _iItemRepository.GetItemCategoryForDropDown();
            var dd = new SelectList(itemCategory.ToList(), "ItemCategoryID", "ItemCategoryName");
            ViewData["ItemCategory"] = dd;
            var itemType = _iItemRepository.GetItemTypeForDropdown();
            var dd1 = new SelectList(itemType.ToList(), "ID", "ItemType");
            ViewData["ItemType"] = dd1;
            ItemBO model = _iItemRepository.GetById(Item_ID);
            return View(model);
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(ItemBO model)
        {
            if (ModelState.IsValid)
            {
                _iItemRepository.Udate(model);
                return RedirectToAction("Index", "Item");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="Item_ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteItem(int Item_ID)
        {
            ItemBO model = _iItemRepository.GetById(Item_ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            _iItemRepository.Delete(ID);
            //_unitRepository.Save();
            return RedirectToAction("Index", "Item");
        }
        #endregion

    }
}