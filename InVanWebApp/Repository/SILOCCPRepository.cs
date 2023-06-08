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
                            Time = reader["Time"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),  //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            Weight = reader["Weight"].ToString(),
                            Temperature = reader["Temperature"].ToString(),
                            Pressure = reader["Pressure"].ToString(),
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
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@Weight", model.Weight);
                    cmd.Parameters.AddWithValue("@Temperature", model.Temperature);
                    cmd.Parameters.AddWithValue("@Pressure", model.Pressure);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@RQCCP_Id", model.RQCCP_Id); //Rahul added 'RQCCP_Id' 08-06-2023.
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        response.Flag = Convert.ToBoolean(dataReader["Flag"]); //Rahul added 08-06-23.
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
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),     //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            Weight = reader["Weight"].ToString(),
                            Temperature = reader["Temperature"].ToString(),
                            Pressure = reader["Pressure"].ToString(),
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
                    cmd.Parameters.AddWithValue("@Id", model.ID);
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@Weight", model.Weight);
                    cmd.Parameters.AddWithValue("@Temperature", model.Temperature);
                    cmd.Parameters.AddWithValue("@Pressure", model.Pressure);
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