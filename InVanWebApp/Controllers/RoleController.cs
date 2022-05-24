using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Controllers
{
    public class RoleController : Controller
    {
        private IRolesRepository _rolesRepository;

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public RoleController()
        {
            _rolesRepository = new RoleRepository(new InVanDBContext());
        }

        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public RoleController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Role
        [HttpGet]
        public ActionResult Index()
        {
            var model = _rolesRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add role master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRole(Role model)
        {
            if (ModelState.IsValid)
            {
                _rolesRepository.Insert(model);
                //_unitRepository.Save();
                return RedirectToAction("Index", "Role");
            }
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRole(int RoleId)
        {
            Role model = _rolesRepository.GetById(RoleId);
            return View(model);
        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRole(Role model)
        {
            if (ModelState.IsValid)
            {
                _rolesRepository.Udate(model);
                return RedirectToAction("Index", "Role");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="RoleId">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRole(int RoleId)
        {
            Role model = _rolesRepository.GetById(RoleId);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int RoleId)
        {
            _rolesRepository.Delete(RoleId);
            //_unitRepository.Save();
            return RedirectToAction("Index", "Role");
        }
        #endregion
    }
}