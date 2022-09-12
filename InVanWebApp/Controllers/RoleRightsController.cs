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
        public JsonResult AddRoleRights(string[] screenNames)
        {
            var roleId = Convert.ToInt32(screenNames[0]);
            var userId = Convert.ToInt32(Session[ApplicationSession.USERID]);
            BindRoles();
            var roleRightsScreen = _rolesRepository.InsertRoleRights(screenNames, roleId,userId);

            if (roleRightsScreen)
            {
                ViewBag.roleRights += "Successfully Rights assigned";
            }
            else
            {
                ViewBag.roleRights += "Some error occurred! Please try again";
            }

            var result = GetRoleRightsJSon(roleId);
            return result;
        }

        #endregion

    }
}