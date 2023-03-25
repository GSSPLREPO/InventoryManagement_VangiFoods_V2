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
        public const string ORGANISATIONTIITLE = "Vangi Foods";
        public const string ORGANISATIONADDRESS = "Sr No 673, Opp Surya Gate, Gana Rd, Karamsad, Gujarat 388325";
        #endregion

        public static void ClearAllSessions()
        {
            mvarSesion.Remove(USERNAME);
            mvarSesion.Remove(USERID);
        }
    }
}