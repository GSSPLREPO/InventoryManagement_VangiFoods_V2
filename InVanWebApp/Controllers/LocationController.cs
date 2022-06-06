using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class LocationController : Controller
    {
        private ILocationRepository _locationRepository;

        #region Initializing constructor
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public LocationController()
        {
            _locationRepository = new LocationRepository(new InVanDBContext());
        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="addItemRepository"></param>
        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 26 may'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
        [HttpGet]
        public ActionResult Index()
        {
            var model = _locationRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Rendered the user to the add location form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddLocation()
        {
            var countryList = _locationRepository.GetCountryForDropDown();
            var dd = new SelectList(countryList.ToList(), "CountryID", "CountryName");
            ViewData["country"] = dd;
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddLocation(LocationMaster model)
        {
            if (ModelState.IsValid)
            {
                _locationRepository.Insert(model);
                return RedirectToAction("Index", "Location");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 26 may'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditLocation(int LocationID)
        {
            var countryList = _locationRepository.GetCountryForDropDown();
            var dd = new SelectList(countryList.ToList(), "CountryID", "CountryName");
            ViewData["Country"] = dd;
            var stateList = _locationRepository.GetStateForDropdown(0);
            var dd1 = new SelectList(stateList.ToList(), "StateID", "StateName");
            ViewData["State"] = dd1;
            var CityList = _locationRepository.GetCityForDropdown(0);
            var dd2 = new SelectList(CityList.ToList(), "CityID", "CityName");
            ViewData["City"] = dd2;
            LocationMaster model = _locationRepository.GetById(LocationID);
            return View(model);
        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditLocation(LocationMaster model)
        {
            if (ModelState.IsValid)
            {
                _locationRepository.Udate(model);
                return RedirectToAction("Index", "Location");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="Location_ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteLocation(int LocationID)
        {
            LocationMaster model = _locationRepository.GetById(LocationID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int LocationID)
        {
            _locationRepository.Delete(LocationID);
            //_unitRepository.Save();
            return RedirectToAction("Index", "Location");
        }
        #endregion

        #region Function for binding state and city dropdown
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Both the below function are called from javascript function form view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetStates(string id)
        {
            var Country_Id = Convert.ToInt32(id);
            var StateList_temp = _locationRepository.GetStateForDropdown(Country_Id);
            var StateList = new SelectList(StateList_temp.ToList(), "StateID", "StateName");
            return Json(StateList);
        }

        public JsonResult GetCity(int id)
        {
            //var Country_Id = Convert.ToInt32(id);
            var CityList_temp = _locationRepository.GetCityForDropdown(id);
            var CityList = new SelectList(CityList_temp.ToList(), "CityID", "CityName");
            return Json(CityList);
        }

        #endregion
    }
}