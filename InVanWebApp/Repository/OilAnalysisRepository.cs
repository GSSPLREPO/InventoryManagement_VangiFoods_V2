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
    public class OilAnalysisRepository : IOilAnalysisRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(OilAnalysisRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 9 March'23
        /// Yatri: This function is for fecthing list of Oil Analysis.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OilAnalysisBO> GetAll()
        {
            List<OilAnalysisBO> oilAnalysisList = new List<OilAnalysisBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OilAnalysis_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var OilAnalysis = new OilAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = (reader["Time"].ToString()),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            LotNo = reader ["LotNo"].ToString(),
                            SampleName = reader ["SampleName"].ToString(),
                            ACIDValue = Convert.ToDecimal(reader["ACIDValue"]),
                            PeroxideValue = reader ["PeroxideValue"].ToString(),
                            Color = reader ["Color"].ToString(),
                            Flavour = reader["Flavour"].ToString(),
                            Odour = reader["Odour"].ToString(),
                        };
                        oilAnalysisList.Add(OilAnalysis);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return oilAnalysisList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 9 March'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(OilAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OilAnalysis_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@LotNo", model.LotNo);
                    cmd.Parameters.AddWithValue("@SampleName", model.SampleName);
                    cmd.Parameters.AddWithValue("@ACIDValue", model.ACIDValue);
                    cmd.Parameters.AddWithValue("@PeroxideValue", model.PeroxideValue);
                    cmd.Parameters.AddWithValue("@Color", model.Color);
                    cmd.Parameters.AddWithValue("@Flavour", model.Flavour);
                    cmd.Parameters.AddWithValue("@Odour", model.Odour);
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
        /// Date: 9 March'23
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OilAnalysisBO GetById(int Id)
        {
            var OilAnalysisBO = new OilAnalysisBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OilAnalysis_GetByID", con);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        OilAnalysisBO = new OilAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"].ToString(),
                            //Time = Convert.ToDateTime(reader["Time"]),
                            LotNo = reader["LotNo"].ToString(),
                            SampleName = reader["SampleName"].ToString(),
                            ACIDValue = Convert.ToDecimal(reader["ACIDValue"]),
                            PeroxideValue = reader["PeroxideValue"].ToString(),
                            Color = reader["Color"].ToString(),
                            Flavour = reader["Flavour"].ToString(),
                            Odour=reader["Odour"].ToString(),
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

            return OilAnalysisBO;
        }

        /// <summary>
        /// Date: 9 March'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(OilAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OilAnalysis_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@LotNo", model.LotNo);
                    cmd.Parameters.AddWithValue("@SampleName", model.SampleName);
                    cmd.Parameters.AddWithValue("@ACIDValue", model.ACIDValue);
                    cmd.Parameters.AddWithValue("@PeroxideValue", model.PeroxideValue);
                    cmd.Parameters.AddWithValue("@Color", model.Color);
                    cmd.Parameters.AddWithValue("@Flavour", model.Flavour);
                    cmd.Parameters.AddWithValue("@Odour", model.Odour);
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
        /// /// Date: 9 March'23
        /// Yatri: This function is for delete record of Oil Analysis using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_OilAnalysis_Delete]", con);
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

        #region  Bind grid for datatable
        /// <summary>
        /// Date: 15 March'23
        /// Yatri: This function is for fecthing list of OilAnalysis.
        /// </summary>
        /// <returns></returns>
        public List<OilAnalysisBO> GetAllOilAnalysisList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null)
        {
           List<OilAnalysisBO> OilAnalysisList = new List<OilAnalysisBO>();
            try
            {
                if (FromDate == null && ToDate == null)
                {
                    FromDate = DateTime.Today;
                    ToDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OilAnalysis_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", FromDate);
                    cmd.Parameters.AddWithValue("@toDate", ToDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var oilAnalysis = new OilAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"].ToString(),
                            LotNo = reader["LotNo"].ToString(),
                            SampleName= reader["SampleName"].ToString(),
                            ACIDValue = Convert.ToDecimal(reader["ACIDValue"]),
                            PeroxideValue = reader["PeroxideValue"].ToString(),
                            Color = reader["Color"].ToString(),
                            Flavour = reader["Flavour"].ToString(),
                            Odour = reader["Odour"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        OilAnalysisList.Add(oilAnalysis);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return OilAnalysisList;
        }
        #endregion
    }
}