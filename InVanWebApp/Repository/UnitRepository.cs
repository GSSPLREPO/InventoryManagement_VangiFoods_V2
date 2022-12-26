using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using log4net;

namespace InVanWebApp.Repository
{
    public class UnitRepository : IUnitRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(UnitRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of unit master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnitMaster> GetAll()
        {
            List<UnitMaster> unitMastersList = new List<UnitMaster>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Unit_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var UnitMasters = new UnitMaster()
                        {
                            UnitID = Convert.ToInt32(reader["UnitID"]),
                            UnitName = reader["UnitName"].ToString(),
                            UnitCode = reader["UnitCode"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        unitMastersList.Add(UnitMasters);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return unitMastersList;
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="UnitID"></param>
        /// <returns></returns>

        public UnitMaster GetById(int UnitID)
        {
            var unitMaster = new UnitMaster();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Unit_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitID", UnitID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        unitMaster = new UnitMaster()
                        {
                            UnitID = Convert.ToInt32(reader["UnitID"]),
                            UnitName = reader["UnitName"].ToString(),
                            UnitCode = reader["UnitCode"].ToString(),
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
            return unitMaster;
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="unitMaster"></param>
        public ResponseMessageBO Update(UnitMaster unitMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitID", unitMaster.UnitID);
                cmd.Parameters.AddWithValue("@UnitName", unitMaster.UnitName);
                cmd.Parameters.AddWithValue("@UnitCode", unitMaster.UnitCode);
                cmd.Parameters.AddWithValue("@Description", unitMaster.Description);
                cmd.Parameters.AddWithValue("@LastModifiedBy", unitMaster.LastModifiedBy);
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
        /// <param name="unitMaster"></param>
        public ResponseMessageBO Insert(UnitMaster unitMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitName", unitMaster.UnitName);
                    cmd.Parameters.AddWithValue("@UnitCode", unitMaster.UnitCode);
                    cmd.Parameters.AddWithValue("@Description", unitMaster.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.UnitName = dataReader["UnitName"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                }
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

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="UnitID"></param>
        public void Delete(int UnitID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitID", UnitID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            
            //    UnitMaster unitMaster = _context.UnitMasters.Find(UnitID);
            //_context.UnitMasters.Remove(unitMaster);
        }
        #endregion
    }
}