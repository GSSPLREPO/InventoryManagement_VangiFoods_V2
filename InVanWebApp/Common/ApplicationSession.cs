using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace InVanWebApp.Common
{
    public class ApplicationSession
    {
        private static HttpSessionState mvarSesion;
        public static void Init(HttpSessionState Session)
        {
            mvarSesion = Session;
        }

        #region onstant declartion of the session Variable 
        public const string USERID = "UserId";
        public const string ROLEID = "RoleId";
        public const string USERNAME = "UserName";
        public const string PASSWORD = "Password";
        public const string PROFILE = "ProfileImage";
        public const string DEVICEPROFILEID = "DeviceProfileId";
        public const string ISSUPERADMIN = "false";
        public const string AREAID = "area";
        public const string ORGANISATIONLOGO = "OrganisationId";
        public const string ORGANISATIONNAME = "OrganisationName";
        #endregion

        public static void ClearAllSessions()
        {
            mvarSesion.Remove(USERNAME);
        }
    }
}