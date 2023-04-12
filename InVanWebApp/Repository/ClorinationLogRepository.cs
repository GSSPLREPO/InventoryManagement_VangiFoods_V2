using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InVanWebApp.Common;
using System.IO;
namespace InVanWebApp.Repository
{
    public class ClorinationLogRepository : IClorinationLogRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(ClorinationLogRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 27 Feb'23
        /// Snehal: This function is for fecthing list of Clorination Log.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClorinationLogBO> GetAll()
        {
            List<ClorinationLogBO> clorinationLogList  = new List<ClorinationLogBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ClorinationLog_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var clorinationLog = new ClorinationLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            FootWasher = reader["FootWasher"].ToString(),
                            RoWater = reader["RoWater"].ToString(),
                            SoftWater = reader["SoftWater"].ToString(),
                            CoolingWaterTank = reader["CoolingWaterTank"].ToString(),
                            ProcessingWater = reader["ProcessingWater"].ToString(),
                            CIPWaterTank = reader["CIPWaterTank"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString()
                            
                        };
                        clorinationLogList.Add(clorinationLog);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return clorinationLogList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Feb'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(ClorinationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ClorinationLog_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@FootWasher", model.FootWasher);
                    cmd.Parameters.AddWithValue("@RoWater", model.RoWater);
                    cmd.Parameters.AddWithValue("@SoftWater", model.SoftWater);
                    cmd.Parameters.AddWithValue("@CoolingWaterTank", model.CoolingWaterTank);
                    cmd.Parameters.AddWithValue("@ProcessingWater", model.ProcessingWater);
                    cmd.Parameters.AddWithValue("@CIPWaterTank", model.CIPWaterTank);
                    cmd.Parameters.AddWithValue("@VerifyByName", model.VerifyByName);
                    cmd.Parameters.AddWithValue("@Remark", model.Remark);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
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

        #region Update functions

        /// <summary>
        /// Date: 27 Feb'23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ClorinationLogBO GetById(int Id)
        {
            var ClorinationLogBO = new ClorinationLogBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ClorinationLog_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClorinationLogBO = new ClorinationLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            FootWasher= reader["FootWasher"].ToString(),
                            RoWater = reader["RoWater"].ToString(),
                            SoftWater = reader["SoftWater"].ToString(),
                            CoolingWaterTank = reader["CoolingWaterTank"].ToString(),
                            ProcessingWater = reader["ProcessingWater"].ToString(),
                            CIPWaterTank = reader["CIPWaterTank"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return ClorinationLogBO;
        }

        /// <summary>
        /// Date: 27 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(ClorinationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ClorinationLog_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@FootWasher", model.FootWasher);
                    cmd.Parameters.AddWithValue("@RoWater", model.RoWater);
                    cmd.Parameters.AddWithValue("@SoftWater", model.SoftWater);
                    cmd.Parameters.AddWithValue("@CoolingWaterTank", model.CoolingWaterTank);
                    cmd.Parameters.AddWithValue("@ProcessingWater", model.ProcessingWater);
                    cmd.Parameters.AddWithValue("@CIPWaterTank", model.CIPWaterTank);
                    cmd.Parameters.AddWithValue("@Remark", model.Remark);
                    cmd.Parameters.AddWithValue("@VerifyByName", model.VerifyByName);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));

                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
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
        /// /// Date: 27 Feb'23
        /// Snehal: This function is for delete record of Clorination Log using it's Id
        /// /// </summary>
        /// /// <param name="Id"></param>
        /// /// <param name="userId"></param>
        /// <returns></returns>
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_ClorinationLog_Delete]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}