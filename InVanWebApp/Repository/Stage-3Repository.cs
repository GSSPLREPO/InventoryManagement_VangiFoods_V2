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

namespace InVanWebApp.Repository
{
    public class Stage3Repository : IStage3Repository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(Stage3Repository));

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stage3BO> GetAll()
        {
            List<Stage3BO> Stage3List = new List<Stage3BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Stage-3_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var Stage3 = new Stage3BO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemId= Convert.ToInt32(reader["ItemId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),  //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            PackingHopperTemp = reader["PackingHopperTemp"].ToString(),
                            ChillerTemp = reader["ChillerTemp"].ToString(),
                            Consistency = reader["Consistency"].ToString(),
                            PackingSize = reader["PackingSize"].ToString(),
                            NoOfPackets = reader["NoOfPackets"].ToString(),
                            PackingUnit = reader["PackingUnit"].ToString(),
                            FinalWeight = reader["FinalWeight"].ToString(),
                            FinalPackets = reader["FinalPackets"].ToString(),
                            RejectedPackets = reader["RejectedPackets"].ToString(),
                            Remarks = reader["Remarks"].ToString(),

                        };
                        Stage3List.Add(Stage3);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return Stage3List;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(Stage3BO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Stage-3_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@PackingHopperTemp", model.PackingHopperTemp);
                    cmd.Parameters.AddWithValue("@ChillerTemp", model.ChillerTemp);
                    cmd.Parameters.AddWithValue("@Consistency", model.Consistency);
                    cmd.Parameters.AddWithValue("@PackingSize", model.PackingSize);
                    cmd.Parameters.AddWithValue("@NoOfPackets", model.NoOfPackets);
                    cmd.Parameters.AddWithValue("@PackingUnit", model.PackingUnit);
                    cmd.Parameters.AddWithValue("@FinalWeight", model.FinalWeight);
                    cmd.Parameters.AddWithValue("@FinalPackets", model.FinalPackets);
                    cmd.Parameters.AddWithValue("@RejectedPackets", model.RejectedPackets);
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
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Stage3BO GetById(int ID)
        {
            var Stage3 = new Stage3BO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Stage-3_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Stage3 = new Stage3BO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNumber"].ToString(),     //Rahul added 'LotNumber' 05-06-2023.
                            RawBatchesNo = reader["RawBatchesNo"].ToString(),
                            PackingHopperTemp = reader["PackingHopperTemp"].ToString(),
                            ChillerTemp = reader["ChillerTemp"].ToString(),
                            Consistency = reader["Consistency"].ToString(),
                            PackingSize = reader["PackingSize"].ToString(),
                            NoOfPackets = reader["NoOfPackets"].ToString(),
                            PackingUnit = reader["PackingUnit"].ToString(),
                            FinalWeight = reader["FinalWeight"].ToString(),
                            FinalPackets = reader["FinalPackets"].ToString(),
                            RejectedPackets = reader["RejectedPackets"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return Stage3;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(Stage3BO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Stage-3_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ItemId", model.ItemId);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@LotNumber", model.LotNumber); //Rahul added 'LotNumber' 05-06-2023.
                    cmd.Parameters.AddWithValue("@RawBatchesNo", model.RawBatchesNo);
                    cmd.Parameters.AddWithValue("@PackingHopperTemp", model.PackingHopperTemp);
                    cmd.Parameters.AddWithValue("@ChillerTemp", model.ChillerTemp);
                    cmd.Parameters.AddWithValue("@Consistency", model.Consistency);
                    cmd.Parameters.AddWithValue("@PackingSize", model.PackingSize);
                    cmd.Parameters.AddWithValue("@NoOfPackets", model.NoOfPackets);
                    cmd.Parameters.AddWithValue("@PackingUnit", model.PackingUnit);
                    cmd.Parameters.AddWithValue("@FinalWeight", model.FinalWeight);
                    cmd.Parameters.AddWithValue("@FinalPackets", model.FinalPackets);
                    cmd.Parameters.AddWithValue("@RejectedPackets", model.RejectedPackets);
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
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Stage-3_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
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