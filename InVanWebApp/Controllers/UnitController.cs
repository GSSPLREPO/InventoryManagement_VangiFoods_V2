using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Controllers
{
    public class UnitController : Controller
    {
        private IUnitRepository _unitRepository;

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public UnitController()
        {
            _unitRepository = new UnitRepository(new InVanDBContext());
        }
        /// <summary>
        /// Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public UnitController(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        /// <summary>
        ///Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = _unitRepository.GetAll();
            return View(model);
        }

        /// <summary>
        /// Rendered the user to the add unit master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUnit()
        {
            return View();
        }

        /// <summary>
        /// Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUnit(UnitMaster model)
        {
            if (ModelState.IsValid)
            {
                _unitRepository.Insert(model);
                //_unitRepository.Save();
                return RedirectToAction("Index", "Unit");
            }
            return View();
        }

        /// <summary>
        /// Rendered the user to the edit page with details of a perticular record.
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
        /// Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUnit(UnitMaster model)
        {
            if (ModelState.IsValid)
            {
                _unitRepository.Udate(model);
                //_unitRepository.Save();
                return RedirectToAction("Index", "Unit");
            }
            else
                return View(model);
        }

        
        [HttpGet]
        public ActionResult DeleteUnit(int UnitId)
        {
            UnitMaster model = _unitRepository.GetById(UnitId);
            return View(model);
        }

        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="UnitId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int UnitId)
        {
            _unitRepository.Delete(UnitId);
            //_unitRepository.Save();
            return RedirectToAction("Index", "Unit");
        }
    }
}