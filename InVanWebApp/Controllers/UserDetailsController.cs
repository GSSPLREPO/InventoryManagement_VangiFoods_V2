using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;

namespace InVanWebApp.Controllers
{
    public class UserDetailsController : Controller
    {
        private IUserDetailsRepository _userDetailsRepository;
        private static ILog log = LogManager.GetLogger(typeof(UserDetailsController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public UserDetailsController()
        {
            _userDetailsRepository = new UserDetailsRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="userRepository"></param>
        public UserDetailsController(IUserDetailsRepository userRepository)
        {
            _userDetailsRepository = userRepository;
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
            var model = _userDetailsRepository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add item type master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUser()
        {
            BindOrganizations();
            BindDesignations();
            BindRoles();
            return View();
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(UserDetailsBO model)
        {
            try
            {
                ResponseMessageBO response = new ResponseMessageBO();
                if (ModelState.IsValid)
                {
                    response = _userDetailsRepository.Insert(model);
                    if (response.Status)
                        TempData["Success"] = "<script>alert('User details Inserted Successfully!');</script>";
                    else
                        TempData["Success"] = "<script>alert('Duplicate user! Can not be inserted!');</script>";

                    return RedirectToAction("Index", "UserDetails");

                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View();
        }
        #endregion

        #region Functions for binding dropdowns
        public void BindOrganizations()
        {
            var organizations = _userDetailsRepository.GetOrganisationForDropDown();
            var organizationsList = new SelectList(organizations.ToList(), "OrganisationId", "Name");
            ViewData["Organizations"] = organizationsList;

        }

        public void BindDesignations()
        {
            var designations = _userDetailsRepository.GetDesignationForDropDown();
            var designationsList = new SelectList(designations.ToList(), "DesignationID", "DesignationName");
            ViewData["Designations"] = designationsList;

        }

        public void BindRoles()
        {
            var roles = _userDetailsRepository.GetRoleForDropDown();
            var rolesList = new SelectList(roles.ToList(), "RoleId", "RoleName");
            ViewData["Roles"] = rolesList;
        }
        #endregion
    }
}