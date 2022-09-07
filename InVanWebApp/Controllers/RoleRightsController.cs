using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;
using System.Web.Services;

namespace InVanWebApp.Controllers
{
    public class RoleRightsController : Controller
    {
        private IRolesRepository _rolesRepository;
        private static ILog log = LogManager.GetLogger(typeof(RoleRightsController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public RoleRightsController()
        {
            _rolesRepository = new RoleRepository();
        }

        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="rolesRepository"></param>
        public RoleRightsController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        #endregion

        #region Bind role dropdown

        /// <summary>
        /// Date: 04 Sept'22
        /// Created by: Farheen
        /// Descripiton: To bind all roles to drop down list
        /// </summary>
        public void BindRoles()
        {
            var roles = _rolesRepository.GetAll();
            var rolesList = new SelectList(roles.ToList(), "RoleId", "RoleName");
            ViewData["Roles"] = rolesList;
        }
        #endregion

        #region Bind Role Rights page
        public ActionResult Index()
        {
            //bind roles to dropdownlist
            BindRoles();
            var Screens = _rolesRepository.GetAllScreens();
            List<RoleRightBO> roleRights = new List<RoleRightBO>();
            foreach (var screen in Screens)
            {
                var rights = new RoleRightBO()
                {
                    RoleId = screen.RoleId,
                    ScreenId = screen.ScreenId,
                    ScreenName = screen.ScreenName
                };
                roleRights.Add(rights);
            }
            return View(roleRights);
        }
        #endregion

        #region Bind rights of perticular selected role.
        /// <summary>
        /// Date: 06 Sep'22
        /// Created by: Farheen
        /// Description: To get role rights of selected role and display
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>

        public JsonResult GetRoleRightsJSon(int roleId)
        {

            RoleRightBO roles = new RoleRightBO();
            Session["roleIdRights"] = roleId;
            List<RoleRightBO> roleRights = new List<RoleRightBO>();
            roleRights = _rolesRepository.GetRightsOfScreenRole(roleId);
            return Json(roleRights, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Save role's rights

       // [WebMethod]
        public JsonResult AddRoleRights(string[] roleRights)
        {
            //if (Session[ApplicationSession.USERID] == null)
            //    return RedirectToAction("Index", "Login");
            int roleId = 1;
            if (Session["roleIdRights"] == null)
                roleId = 1;
            else
                roleId = Convert.ToInt32(Session["roleIdRights"]);

            //var roleRightsScreen = _rolesRepository.InsertRoleRights(roleRights, roleId);
            BindRoles();
            //if (roleRightsScreen == true)
            if (true)
            {
                ViewBag.roleRights += "Successfully Rights assigned";
            }
            else
            {
                ViewBag.roleRights += "Some error occurred! Please try again";
            }

            //List<SelectListItem> roleRights = GetRoleRightsJSon(0);

            //return View(items);
            return Json(roleRights, JsonRequestBehavior.AllowGet);
        }

        #endregion

        ///// <summary>
        ///// Date: 05 Sept 2022
        ///// Created by: Farheen 
        /////Description: To add Rights of a screens to a role
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Index(RoleRightBO roleRights)
        //{
        //    if (Session[ApplicationSession.USERID] == null)
        //        return RedirectToAction("Index", "Login");
        //    else
        //    {
        //        //bind roles to dropdownlist
        //        BindRoles();
        //        var Screens = _rolesRepository.GetAllScreens();
        //        return View(roleRights);
        //    }
        //}
    }
}