using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;

namespace InVanWebApp.Repository
{
    public class RoleRepository : IRolesRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(RoleRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of role master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleBO> GetAll()
        {
            List<RoleBO> RoleMastersList = new List<RoleBO>();
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
                        var RoleMaster = new RoleBO()
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        RoleMastersList.Add(RoleMaster);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return RoleMastersList;
        }
        #endregion

        #region Update functions

        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public RoleBO GetById(int RoleId)
        {
            var roleMaster = new RoleBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Role_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleId", RoleId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        roleMaster = new RoleBO()
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return roleMaster;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="roleMaster"></param>
        public ResponseMessageBO Update(RoleBO roleMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Role_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleId", roleMaster.RoleId);
                    cmd.Parameters.AddWithValue("@RoleName", roleMaster.RoleName);
                    cmd.Parameters.AddWithValue("@Description", roleMaster.Description);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                return response;
            }
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="roleMaster"></param>
        public ResponseMessageBO Insert(RoleBO roleMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Role_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", roleMaster.RoleName);
                    cmd.Parameters.AddWithValue("@Description", roleMaster.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.RoleName = dataReader["RoleName"].ToString();
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

        #region Delete function
        public void Delete(int RoleId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Role_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", RoleId);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }

        #endregion

        //==============================Role rights functions===================================//
        #region Role Rights functions

        #region Bind Screen list
        public IEnumerable<RoleRightBO> GetAllScreens()
        {
            List<RoleRightBO> resultList = new List<RoleRightBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Screens_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new RoleRightBO()
                        {
                            ScreenId = Convert.ToInt32(dataReader["ScreenId"]),
                            ScreenName = dataReader["ScreenName"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                resultList = null;
                log.Error(ex.Message, ex);
            }
            return resultList;
        }
        #endregion

        #region Bind rights of perticular selected role.
        /// <summary>
        /// Date: 06 Sep'22
        /// Created By: Farheen
        /// Description: To get whether list of screens for a perticular role id.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public List<RoleRightBO> GetRightsOfScreenRole(int roleId)
        {
            List<RoleRightBO> resultList = new List<RoleRightBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetScreenNamesByRoleId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleId", roleId);
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new RoleRightBO()
                        {
                            RoleId = Convert.ToInt32(dataReader["RoleId"]),
                            ScreenName = dataReader["ScreenName"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                resultList = null;
            }
            return resultList;
        }

        #endregion

        #region Save role's rights
        /// <summary>
        /// Date: 08 Sep'22
        /// Created by: Farheen
        /// Description: Insert role and rights but before insertion delete that role's rights then 
        /// insert command execute.
        /// </summary>
        /// <param name="screenNames"></param>
        /// <param name="roleId"></param>
        /// <param name="LastModifiedBy"></param>
        /// <returns></returns>
        public bool InsertRoleRights(string[] screenNames, int roleId, int LastModifiedBy)
        {
            ResponseMessageBO result = new ResponseMessageBO();
            bool flagReesult = false;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd1 = new SqlCommand("usp_tbl_RoleRights_Delete", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@RoleId", roleId);
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    for (int i = 1; i < screenNames.Length; i++)
                    {
                        SqlCommand cmd = new SqlCommand("usp_tbl_RoleRights_Save", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", roleId);
                        cmd.Parameters.AddWithValue("@screenName", screenNames[i]);
                        cmd.Parameters.AddWithValue("@LastModifiedUserId", LastModifiedBy);
                        cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            result.Status = Convert.ToBoolean(reader["Status"]);
                            if (result.Status)
                                flagReesult = true;
                            else
                                flagReesult = false;

                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                flagReesult = false;
                log.Error(ex.Message, ex);
            }
            return flagReesult;
        }

        #endregion

        #region Get Role Rights list by role id.
        public List<ScreenNameBO> GetRoleRightList(int roleId)
        {
            List<ScreenNameBO> resultList = new List<ScreenNameBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetScreensNotInRoleRightsById", con);
                    cmd.Parameters.AddWithValue("@RoleId",roleId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new ScreenNameBO()
                        {
                            ScreenId = Convert.ToInt32(dataReader["ScreenId"]),
                            ScreenName = dataReader["ScreenName"].ToString(),
                            DisplayName=dataReader["DisplayName"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                resultList = null;
                log.Error(ex.Message, ex);
            }
            return resultList;
        }
        #endregion

        #endregion
    }
}