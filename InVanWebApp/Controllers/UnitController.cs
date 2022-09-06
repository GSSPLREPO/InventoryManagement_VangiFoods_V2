using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using log4net;

namespace InVanWebApp.Controllers
{
    public class UnitController : Controller
    {
        private IUnitRepository _unitRepository;
        private static ILog log = LogManager.GetLogger(typeof(UnitController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public UnitController()
        {
            _unitRepository = new UnitRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public UnitController(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
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
            var model = _unitRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add unit master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUnit()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUnit(UnitMaster model)
        {
            try
            {
                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    response = _unitRepository.Insert(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Unit inserted successfully!');</script>";
                    else
                        TempData["Success"] = "<script>alert('Duplicate unit!');</script>";

                    return RedirectToAction("Index", "Unit");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                //TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                //return RedirectToAction("Index", "Unit");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="UnitID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUnit(int UnitID)
        {
            UnitMaster model = _unitRepository.GetById(UnitID);
            return View(model);
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUnit(UnitMaster model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _unitRepository.Update(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Unit updated successfully!');</script>";

                    else
                        TempData["Success"] = "<script>alert('Duplicate unit!');</script>";

                    return RedirectToAction("Index", "Unit");
                }
                else
                    return View(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "Unit");
            }
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="UnitId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUnit(int UnitId)
        {
            _unitRepository.Delete(UnitId);
            //_unitRepository.Save();
            TempData["Success"] = "<script>alert('Unit deleted successfully!');</script>";
            return RedirectToAction("Index", "Unit");
        }
        #endregion
    }
}