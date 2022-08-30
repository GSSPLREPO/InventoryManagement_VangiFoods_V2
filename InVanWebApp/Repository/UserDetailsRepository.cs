using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(ItemTypeRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDetailsBO> GetAll()
        {
            List<UserDetailsBO> userList = new List<UserDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_UserDetails_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var user = new UserDetailsBO()
                        {
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            EmployeeName = reader["EmployeeName"].ToString(),
                            Designation = reader["Designation"].ToString(),
                            EmployeeMobileNo = reader["EmployeeMobileNo"].ToString(),
                            EmailId = reader["EmailId"].ToString()
                        };
                        userList.Add(user);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return userList;
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(UserDetailsBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_UserDetails_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                    cmd.Parameters.AddWithValue("@RoleId", model.RoleId);
                    cmd.Parameters.AddWithValue("@OrganisationID", model.OrganizationID);
                    cmd.Parameters.AddWithValue("@DesignationID", model.DesignationID);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@EmployeeMobileNo", model.EmployeeMobileNo);
                    cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
                    cmd.Parameters.AddWithValue("@EmployeeGender", model.EmployeeGender);
                    cmd.Parameters.AddWithValue("@EmployeeJoingDate", model.EmployeeJoingDate);
                    cmd.Parameters.AddWithValue("@EmployeeAddress", model.EmployeeAddress);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.EmployeeName = dataReader["EmployeeName"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Functions for dropdown binding
        public IEnumerable<OrganisationsBO> GetOrganisationForDropDown()
        {
            List<OrganisationsBO> organizationsList = new List<OrganisationsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var organisation = new OrganisationsBO()
                        {
                            OrganisationId = Convert.ToInt32(reader["OrganisationId"]),
                            Name = reader["Name"].ToString()
                        };
                        organizationsList.Add(organisation);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return organizationsList;
        }
        public IEnumerable<DesignationBO> GetDesignationForDropDown()
        {
            List<DesignationBO> designationList = new List<DesignationBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Designation_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var designation = new DesignationBO()
                        {
                            DesignationID = Convert.ToInt32(reader["DesignationID"]),
                            DesignationName = reader["DesignationName"].ToString()
                        };
                        designationList.Add(designation);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return designationList;
        }
        public IEnumerable<RoleBO> GetRoleForDropDown()
        {
            List<RoleBO> roleList = new List<RoleBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Role_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var role = new RoleBO()
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString()
                        };
                        roleList.Add(role);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return roleList;
        }

        #endregion

    }
}