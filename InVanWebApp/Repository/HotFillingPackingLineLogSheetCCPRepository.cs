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

namespace InVanWebApp.Repository
{
    public class HotFillingPackingLineLogSheetCCPRepository : IHotFillingPackingLineLogSheetCCPRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(HotFillingPackingLineLogSheetCCPRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HotFillingPackingLineLogSheetCCPBO> GetAll()
        {
            List<HotFillingPackingLineLogSheetCCPBO> HotFillingPackingLineLogSheetCCPList = new List<HotFillingPackingLineLogSheetCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_HotFillingPackingLineLogSheetCCP_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var HotFillingPackingLineLogSheetCCP = new HotFillingPackingLineLogSheetCCPBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemName = reader["ItemName"].ToString(),
                            ReleaseTime = reader["ReleaseTime"].ToString(),
                            HotLineTemp = reader["HotLineTemp"].ToString(),
                            ProductTemp = reader["ProductTemp"].ToString(),
                            CleaningHygine = reader["CleaningHygine"].ToString(),
                            RandomWeight = Convert.ToDecimal(reader["RandomWeight"]),
                            MonitoringParameters = reader["MonitoringParameters"].ToString(),
                            MonitoringFilling = reader["MonitoringFilling"].ToString(),
                            NoOfPouches = reader["NoOfPouches"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            CorrectiveActions = reader["CorrectiveActions"].ToString()

                        };
                        HotFillingPackingLineLogSheetCCPList.Add(HotFillingPackingLineLogSheetCCP);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return HotFillingPackingLineLogSheetCCPList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(HotFillingPackingLineLogSheetCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_HotFillingPackingLineLogSheetCCP_Insert]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@ReleaseTime", model.ReleaseTime);
                    cmd.Parameters.AddWithValue("@HotLineTemp", model.HotLineTemp);
                    cmd.Parameters.AddWithValue("@ProductTemp", model.ProductTemp);
                    cmd.Parameters.AddWithValue("@CleaningHygine", model.CleaningHygine);
                    cmd.Parameters.AddWithValue("@RandomWeight", model.RandomWeight);
                    cmd.Parameters.AddWithValue("@MonitoringParameters", model.MonitoringParameters);
                    cmd.Parameters.AddWithValue("@MonitoringFilling", model.MonitoringFilling);
                    cmd.Parameters.AddWithValue("@NoOfPouches", model.NoOfPouches);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CorrectiveActions", model.CorrectiveActions);
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
        public HotFillingPackingLineLogSheetCCPBO GetById(int ID)
        {
            var HotFillingPackingLineLogSheetCCP = new HotFillingPackingLineLogSheetCCPBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_HotFillingPackingLineLogSheetCCP_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HotFillingPackingLineLogSheetCCP = new HotFillingPackingLineLogSheetCCPBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            ItemName = reader["ItemName"].ToString(),
                            ReleaseTime = reader["ReleaseTime"].ToString(),
                            HotLineTemp = reader["HotLineTemp"].ToString(),
                            ProductTemp = reader["ProductTemp"].ToString(),
                            CleaningHygine = reader["CleaningHygine"].ToString(),
                            RandomWeight = Convert.ToDecimal(reader["RandomWeight"]),
                            MonitoringParameters = reader["MonitoringParameters"].ToString(),
                            MonitoringFilling = reader["MonitoringFilling"].ToString(),
                            NoOfPouches = reader["NoOfPouches"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            CorrectiveActions = reader["CorrectiveActions"].ToString(),
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return HotFillingPackingLineLogSheetCCP;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(HotFillingPackingLineLogSheetCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_HotFillingPackingLineLogSheetCCP_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@ReleaseTime", model.ReleaseTime);
                    cmd.Parameters.AddWithValue("@HotLineTemp", model.HotLineTemp);
                    cmd.Parameters.AddWithValue("@ProductTemp", model.ProductTemp);
                    cmd.Parameters.AddWithValue("@CleaningHygine", model.CleaningHygine);
                    cmd.Parameters.AddWithValue("@RandomWeight", model.RandomWeight);
                    cmd.Parameters.AddWithValue("@MonitoringParameters", model.MonitoringParameters);
                    cmd.Parameters.AddWithValue("@MonitoringFilling", model.MonitoringFilling);
                    cmd.Parameters.AddWithValue("@NoOfPouches", model.NoOfPouches);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CorrectiveActions", model.CorrectiveActions);
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_HotFillingPackingLineLogSheetCCP_Delete", con);
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