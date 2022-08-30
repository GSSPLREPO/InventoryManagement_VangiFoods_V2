using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;

namespace InVanWebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }

        /// <summary>
        /// Date: 30-08-2022
        /// Created by: Farheen
        /// Description: Authenticate user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UsersBO userModel)
        {


            //using (Db_DeconEntities db_DeconEntities = new Db_DeconEntities())
            //{
            //    var userdata = db_DeconEntities.usp_tbl_GetUserDetailsByNamePwd(userModel.UserName, userModel.Password).FirstOrDefault();

            //    if (userdata != null)
            //    {
            //        Session[ApplicationSession.USERID] = userdata.UserId;
            //        Session[ApplicationSession.USERNAME] = userdata.UserName;
            //        Session[ApplicationSession.ROLEID] = userdata.RoleId;
            //        ViewData["message"] = "1";
            //        //GetOrgLogo();
            //        GetRollRightList();
            //        return RedirectToAction("ValveTestEntry", "ValveTest");
            //    }
            //    else
            //    {
            //        ViewBag.Login = "Login Failed ! Invalid Username or Password";
            //        return View();

            //        // return View();
            //        //ViewData["message"] = "Login Failed !\n Invalid Username or Password";
            //        //return RedirectToAction("Login", "Login");
            //    }
            //}
            return RedirectToAction("Index", "Dashboard");
        }


    }
}