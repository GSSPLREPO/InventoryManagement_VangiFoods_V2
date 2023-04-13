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
    public class WaterAnalysisRepository : IWaterAnalysisRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(WaterAnalysisRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri: This function is for fecthing list of Water Analysis.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WaterAnalysisBO> GetAll()
        {
            List<WaterAnalysisBO> waterAnalysisList = new List<WaterAnalysisBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_WaterAnalysis_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var waterAnalysis = new WaterAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            //dateGridBinding = Convert.ToDateTime(reader["Date"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = Convert.ToDateTime(reader["Time"]),
                            Time = (reader["Time"].ToString()),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            //PAPH = reader["PAPH"].ToString(),
                            PAPH = Convert.ToDecimal(reader["PAPH"]),
                            PATDS = reader["PATDS"].ToString(),
                            PAHardness = reader["PAHardness"].ToString(),
                            PASaltAdded = reader["PASaltAdded"].ToString(),
                            //SWPH=reader["SWPH"].ToString(),
                            SWPH= Convert.ToDecimal(reader["SWPH"]),
                            SWTDS = reader["SWTDS"].ToString(),
                            SWHardness = reader["SWHardness"].ToString(),
                            ETPTEM = reader["ETPTEM"].ToString(),
                            //ETPPH = reader["ETPPH"].ToString(),
                            ETPPH = Convert.ToDecimal(reader["ETPPH"]),
                            ETPTDS = reader["ETPTDS"].ToString(),
                            TEM = reader["TEM"].ToString(),
                            GasReading = reader["GasReading"]==DBNull.Value?0:Convert.ToDecimal( reader["GasReading"])
                        };
                        waterAnalysisList.Add(waterAnalysis);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return waterAnalysisList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(WaterAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_WaterAnalysis_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@PAPH", model.PAPH);
                    cmd.Parameters.AddWithValue("@PATDS", model.PATDS);
                    cmd.Parameters.AddWithValue("@PAHardness", model.PAHardness);
                    cmd.Parameters.AddWithValue("@PASaltAdded", model.PASaltAdded);
                    cmd.Parameters.AddWithValue("@SWPH", model.SWPH);
                    cmd.Parameters.AddWithValue("@SWTDS", model.SWTDS);
                    cmd.Parameters.AddWithValue("@SWHardness", model.SWHardness);
                    cmd.Parameters.AddWithValue("@ETPTEM", model.ETPTEM);
                    cmd.Parameters.AddWithValue("@ETPPH", model.ETPPH);
                    cmd.Parameters.AddWithValue("@ETPTDS", model.ETPTDS);
                    cmd.Parameters.AddWithValue("@TEM", model.TEM);
                    cmd.Parameters.AddWithValue("@GasReading", model.GasReading);
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
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WaterAnalysisBO GetById(int Id)
        {
            var WaterAnalysisBO = new WaterAnalysisBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_WaterAnalysis_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        WaterAnalysisBO = new WaterAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = Convert.ToDateTime(reader["Time"]),
                            Time = (reader["Time"].ToString()),
                            //PAPH = reader["PAPH"].ToString(),
                            PAPH = Convert.ToDecimal(reader["PAPH"]),
                            PATDS = reader["PATDS"].ToString(),
                            PAHardness = reader["PAHardness"].ToString(),
                            PASaltAdded = reader["PASaltAdded"].ToString(),
                            //SWPH = reader["SWPH"].ToString(),
                            SWPH = Convert.ToDecimal(reader["SWPH"]),
                            SWTDS = reader["SWTDS"].ToString(),
                            SWHardness=reader["SWHardness"].ToString(),
                            ETPTEM = reader["ETPTEM"].ToString(),
                            //ETPPH = reader["ETPPH"].ToString(),
                            ETPPH = Convert.ToDecimal(reader["ETPPH"]),
                            ETPTDS = reader["ETPTDS"].ToString(),
                            TEM = reader["TEM"].ToString(),
                            GasReading = Convert.ToDecimal(reader["GasReading"]),
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

            return WaterAnalysisBO;
        }

        /// <summary>
        /// Date: 27 Feb'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(WaterAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_WaterAnalysis_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@PAPH", model.PAPH);
                    cmd.Parameters.AddWithValue("@PATDS", model.PATDS);
                    cmd.Parameters.AddWithValue("@PAHardness", model.PAHardness);
                    cmd.Parameters.AddWithValue("@PASaltAdded", model.PASaltAdded);
                    cmd.Parameters.AddWithValue("@SWPH", model.SWPH);
                    cmd.Parameters.AddWithValue("@SWTDS", model.SWTDS);
                    cmd.Parameters.AddWithValue("@SWHardness", model.SWHardness);
                    cmd.Parameters.AddWithValue("@ETPTEM", model.ETPTEM);
                    cmd.Parameters.AddWithValue("@ETPPH", model.ETPPH);
                    cmd.Parameters.AddWithValue("@ETPTDS", model.ETPTDS);
                    cmd.Parameters.AddWithValue("@TEM", model.TEM);
                    cmd.Parameters.AddWithValue("@GasReading", model.GasReading);
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
        /// Yatri: This function is for delete record of Water Analysis using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_WaterAnalysis_Delete]", con);
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