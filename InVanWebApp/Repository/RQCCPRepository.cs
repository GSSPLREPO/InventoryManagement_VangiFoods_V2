using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using System.Configuration;
using System.Data.SqlClient;
using InVanWebApp.Common;
using System.Data;

namespace InVanWebApp.Repository
{
    public class RQCCPRepository : IRQCCPRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(RQCCPRepository));

        #region  Bind grid
        /// <summary>
        /// Charmi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RQCCPBO> GetAll()
        {
            List<RQCCPBO> RQCCPList = new List<RQCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var RQCCP = new RQCCPBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),  //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            WeightofRawBatches = reader["WeightofRawBatches"].ToString(),
                            TansferTimeintoHoldingSilo = reader["TansferTimeintoHoldingSilo"].ToString(),
                            Weight = reader["Weight"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                        };
                        RQCCPList.Add(RQCCP);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                RQCCPList = null;
            }
            return RQCCPList;

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(RQCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@WeightofRawBatches", model.WeightofRawBatches);
                    cmd.Parameters.AddWithValue("@TansferTimeintoHoldingSilo", model.TansferTimeintoHoldingSilo);
                    cmd.Parameters.AddWithValue("@Weight", model.Weight);
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                       // response.ItemName = dataReader["ItemName"].ToString();
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
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RQCCPBO GetById(int Id)
        {
            var RQCCP = new RQCCPBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RQCCP = new RQCCPBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),     //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            WeightofRawBatches = reader["WeightofRawBatches"].ToString(),
                            TansferTimeintoHoldingSilo = reader["TansferTimeintoHoldingSilo"].ToString(),
                            Weight = reader["Weight"].ToString(),
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

            return RQCCP;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(RQCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@WeightofRawBatches", model.WeightofRawBatches);
                    cmd.Parameters.AddWithValue("@TansferTimeintoHoldingSilo", model.TansferTimeintoHoldingSilo);
                    cmd.Parameters.AddWithValue("@Weight", model.Weight);
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
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
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Delete", con);
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