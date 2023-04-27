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
    public class SILOCCPRepository : ISILOCCPRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(SILOCCPRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 20 March'23
        /// Yatri: This function is for fecthing list of SILOCCP.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SILOCCPBO> GetAll()
        {
            List<SILOCCPBO> SILOCCPList = new List<SILOCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SILOCCP_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var SILOCCP = new SILOCCPBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemName = (reader["ItemName"].ToString()),
                            Activity = reader["Activity"].ToString(),
                            MonitoringParameter = reader["MonitoringParameter"].ToString(),
                            TranseferedTimeFromRQS = reader["TranseferedTimeFromRQS"].ToString(),
                            MandatoryRange = reader["MandatoryRange"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            WeightOfHoldingMaterial = reader["WeightOfHoldingMaterial"].ToString(),
                            Time = reader["Time"].ToString(),
                            CorrectiveActions = reader["CorrectiveActions"].ToString(),
                            Responsibility = reader["Responsibility"].ToString(),
                            Verification = reader["Verification"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                            
                        };
                        SILOCCPList.Add(SILOCCP);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return SILOCCPList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 20 March'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(SILOCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SILOCCP_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@Activity", model.Activity);
                    cmd.Parameters.AddWithValue("@MonitoringParameter", model.MonitoringParameter);
                    cmd.Parameters.AddWithValue("@TranseferedTimeFromRQS", model.TranseferedTimeFromRQS);
                    cmd.Parameters.AddWithValue("@MandatoryRange", model.MandatoryRange);
                    cmd.Parameters.AddWithValue("@Frequency", model.Frequency);
                    cmd.Parameters.AddWithValue("@WeightOfHoldingMaterial", model.WeightOfHoldingMaterial);
                    cmd.Parameters.AddWithValue("@CorrectiveActions", model.CorrectiveActions);
                    cmd.Parameters.AddWithValue("@Responsibility", model.Responsibility);
                    cmd.Parameters.AddWithValue("@Verification", model.Verification);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
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
        /// Date: 20 March '23
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SILOCCPBO GetById(int Id)
        {
            var SILOCCPBO = new SILOCCPBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SILOCCP_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SILOCCPBO = new SILOCCPBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemName = (reader["ItemName"].ToString()),
                            Activity = reader["Activity"].ToString(),
                            MonitoringParameter = reader["MonitoringParameter"].ToString(),
                            TranseferedTimeFromRQS = reader["TranseferedTimeFromRQS"].ToString(),
                            MandatoryRange = reader["MandatoryRange"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            WeightOfHoldingMaterial = reader["WeightOfHoldingMaterial"].ToString(),
                            Time = reader["Time"].ToString(),
                            CorrectiveActions = reader["CorrectiveActions"].ToString(),
                            Responsibility = reader["Responsibility"].ToString(),
                            Verification = reader["Verification"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return SILOCCPBO;
        }

        /// <summary>
        /// Date: 20 March'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(SILOCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SILOCCP_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@Activity", model.Activity);
                    cmd.Parameters.AddWithValue("@MonitoringParameter", model.MonitoringParameter);
                    cmd.Parameters.AddWithValue("@TranseferedTimeFromRQS", model.TranseferedTimeFromRQS);
                    cmd.Parameters.AddWithValue("@MandatoryRange", model.MandatoryRange);
                    cmd.Parameters.AddWithValue("@Frequency", model.Frequency);
                    cmd.Parameters.AddWithValue("@WeightOfHoldingMaterial", model.WeightOfHoldingMaterial);
                    cmd.Parameters.AddWithValue("@CorrectiveActions", model.CorrectiveActions);
                    cmd.Parameters.AddWithValue("@Responsibility", model.Responsibility);
                    cmd.Parameters.AddWithValue("@Verification", model.Verification);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
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
        /// /// Date: 20 March'23
        /// Yatri: This function is for delete record of SILOCCP using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_SILOCCP_Delete]", con);
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