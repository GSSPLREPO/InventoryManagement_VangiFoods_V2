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
    public class RoleController : Controller
    {
        private IRolesRepository _rolesRepository;
        private static ILog log = LogManager.GetLogger(typeof(RoleController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public RoleController()
        {
            _rolesRepository = new RoleRepository();
        }

        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="rolesRepository"></param>
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
        public ActionResult AddRole(RoleBO model)
        {
            try
            {
                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    response = _rolesRepository.Insert(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Role Inserted Successfully!');</script>";
                    else
                    {
                        TempData["Success"] = "<script>alert('Duplicate role! Can not be inserted!');</script>";
                        return View();
                    }

                    return RedirectToAction("Index", "Role");

                }
                else
                    TempData["Success"] = "<script>alert('Data is not inserted cirrectly!');</script>";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
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
            RoleBO model = _rolesRepository.GetById(RoleId);
            return View(model);
        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRole(RoleBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _rolesRepository.Update(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('Role updated successfully!');</script>";

                    else
                    {
                        TempData["Success"] = "<script>alert('Duplicate role! Can not be updated!');</script>";
                        return View();
                    }


                    return RedirectToAction("Index", "Role");
                }
                else
                {
                    TempData["Success"] = "<script>alert('Data is not inserted cirrectly!');</script>";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "Role");
            }
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
            RoleBO model = _rolesRepository.GetById(RoleId);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int RoleId)
        {
            _rolesRepository.Delete(RoleId);
            //_unitRepository.Save();
            TempData["Success"] = "<script>alert('Role deleted successfully!');</script>";
            return RedirectToAction("Index", "Role");
        }
        #endregion
    }
}