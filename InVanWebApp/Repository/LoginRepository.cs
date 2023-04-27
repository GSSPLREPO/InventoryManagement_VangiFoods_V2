using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private static ILog log = LogManager.GetLogger(typeof(LoginRepository));
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());

        #region Function for authenticating user.
        /// <summary>
        /// Date: 04 Sept'22
        /// Created by: Farheen
        /// Function for authenticating user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UsersBO AuthenticateUser(string userName, string password)
        {
            UsersBO result = new UsersBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_LoginAuthentication", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cmd.Parameters.AddWithValue("@password", password);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        result = new UsersBO()
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion
    }
}