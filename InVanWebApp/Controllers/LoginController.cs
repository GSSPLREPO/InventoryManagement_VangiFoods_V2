using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using log4net;
using InVanWebApp.Common;
using System.Web.Services;

namespace InVanWebApp.Controllers
{
    public class LoginController : Controller
    {
        private ILoginRepository _loginRepository;
        public static ILog log = LogManager.GetLogger(typeof(LoginController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public LoginController()
        {
            _loginRepository = new LoginRepository();
        }

        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="rolesRepository"></param>
        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        #endregion

        #region Page load function
        // GET: Login
        public ActionResult Index()
        {
            Session.Abandon(); //End the current session, also remaove all the objects stored in the session.
            return View();
        }
        #endregion

        #region Authenticate the user
        /// <summary>
        /// Date: 30-08-2022
        /// Created by: Farheen
        /// Description: Authenticate user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(UsersBO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = _loginRepository.AuthenticateUser(model.Username, model.Password);
                    if (userData != null && userData.Username!=null && userData.RoleId!=null 
                        && userData.Username!="" && userData.RoleId!=0)
                    {
                        Session[ApplicationSession.USERID] = userData.UserId;
                        Session[ApplicationSession.USERNAME] = userData.Username;
                        Session[ApplicationSession.ROLEID] = userData.RoleId;
                        ViewData["message"] = "1";
                        ViewData["Username"] = Session[ApplicationSession.USERNAME].ToString();
                        //GetRollRightList();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Login = "Login Failed ! Invalid Username or Password";
                        return View();

                        // return View();
                        //ViewData["message"] = "Login Failed !\n Invalid Username or Password";
                        //return RedirectToAction("Login", "Login");
                    }
                }
                else
                {
                    TempData["Success"] = "Please enter proper data!";
                    return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "Error:" + ex.Message;
                return View();
            }

        }
        #endregion

    }
}