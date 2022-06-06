using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class AddItemController : Controller
    {
        private IAddItemRepository _iAddItemRepository;

        #region Initializing constructor
        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Constructor without parameter
        /// </summary>
        public AddItemController()
        {
            _iAddItemRepository = new AddItemRepository(new InVanDBContext());
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="addItemRepository"></param>
        public AddItemController(IAddItemRepository addItemRepository)
        {
            _iAddItemRepository = addItemRepository;
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
            var model = _iAddItemRepository.GetAll();
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
            var itemCategory = _iAddItemRepository.GetItemCategoryForDropDown();
            var dd = new SelectList(itemCategory.ToList(), "ItemCategoryID", "ItemCategoryName");
            ViewData["ItemCategory"] = dd;
            var unit = _iAddItemRepository.GetUnitForDropdown();
            var dd1 = new SelectList(unit.ToList(), "UnitID", "UnitName");
            ViewData["Unit"] = dd1;
            return View();
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItems(Item model)
        {
            if (ModelState.IsValid)
            {
                _iAddItemRepository.Insert(model);
                return RedirectToAction("Index", "AddItem");
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
            var itemCategory = _iAddItemRepository.GetItemCategoryForDropDown();
            var dd = new SelectList(itemCategory.ToList(), "ItemCategoryID", "ItemCategoryName");
            ViewData["ItemCategory"] = dd;
            var unit = _iAddItemRepository.GetUnitForDropdown();
            var dd1 = new SelectList(unit.ToList(), "UnitID", "UnitName");
            ViewData["Unit"] = dd1;
            Item model = _iAddItemRepository.GetById(Item_ID);
            return View(model);
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(Item model)
        {
            if (ModelState.IsValid)
            {
                _iAddItemRepository.Udate(model);
                return RedirectToAction("Index", "AddItem");
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
            Item model = _iAddItemRepository.GetById(Item_ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int Item_ID)
        {
            _iAddItemRepository.Delete(Item_ID);
            //_unitRepository.Save();
            return RedirectToAction("Index", "AddItem");
        }
        #endregion

    }
}