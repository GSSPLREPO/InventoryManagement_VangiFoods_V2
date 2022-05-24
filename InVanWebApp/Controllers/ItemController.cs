using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Controllers
{
    public class ItemController : Controller
    {
        private IItemRepository _itemRepository;

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemController()
        {
            _itemRepository = new ItemRepository(new InVanDBContext());
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
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
            var model = _itemRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add item master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItem()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItem(ItemMaster model)
        {
            if (ModelState.IsValid)
            {
                _itemRepository.Insert(model);
                //_unitRepository.Save();
                return RedirectToAction("Index", "Item");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItem(int ItemID)
        {
            ItemMaster model = _itemRepository.GetById(ItemID);
            return View(model);
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(ItemMaster model)
        {
            if (ModelState.IsValid)
            {
                _itemRepository.Udate(model);
                return RedirectToAction("Index", "Item");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="ItemId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteItem(int ItemId)
        {
            ItemMaster model = _itemRepository.GetById(ItemId);
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int ItemId)
        {
            _itemRepository.Delete(ItemId);
            //_unitRepository.Save();
            return RedirectToAction("Index", "Item");
        }
        #endregion

    }
}