using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using log4net;
//using InVanWebApp.DAL;

namespace InVanWebApp.Controllers
{
    public class LocationController : Controller
    {
        private ILocationRepository _locationRepository;
        private static ILog log = LogManager.GetLogger(typeof(LocationController));

        #region Initializing constructor
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public LocationController()
        {
            _locationRepository = new LocationRepository();
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
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _locationRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
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
            if (Session[ApplicationSession.USERID] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddLocation(LocationMasterBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _locationRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Location inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate location name!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "Location");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return View();
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

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
        /// Date: 26 may'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditLocation(int LocationID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                LocationMasterBO model = _locationRepository.GetById(LocationID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditLocation(LocationMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _locationRepository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Location updated successfully!');</script>";
                        else
                        { 
                            TempData["Success"] = "<script>alert('Duplicate location name!');</script>";
                            return View(model);
                        }

                        return RedirectToAction("Index", "Location");
                    }
                    else
                        return View(model);
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "Location");
            }
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
            if (Session[ApplicationSession.USERID] != null)
            {
                _locationRepository.Delete(LocationID);
                //_unitRepository.Save();
                TempData["Success"] = "<script>alert('Location deleted successfully!');</script>";
                return RedirectToAction("Index", "Location");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        //#region Function for binding state and city dropdown
        ///// <summary>
        ///// Date: 26 may'22
        ///// Farheen: Both the below function are called from javascript function form view.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public JsonResult GetStates(string id)
        //{
        //    var Country_Id = Convert.ToInt32(id);
        //    var StateList_temp = _locationRepository.GetStateForDropdown(Country_Id);
        //    var StateList = new SelectList(StateList_temp.ToList(), "StateID", "StateName");
        //    return Json(StateList);
        //}

        //public JsonResult GetCity(int id)
        //{
        //    //var Country_Id = Convert.ToInt32(id);
        //    var CityList_temp = _locationRepository.GetCityForDropdown(id);
        //    var CityList = new SelectList(CityList_temp.ToList(), "CityID", "CityName");
        //    return Json(CityList);
        //}

        //#endregion
    }
}