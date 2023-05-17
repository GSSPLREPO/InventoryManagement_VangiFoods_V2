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
    public class MicroAnalysisRepository : IMicroAnalysisRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(MicroAnalysisRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MicroAnalysisBO> GetAll()
        {
            List<MicroAnalysisBO> MicroAnalysisList = new List<MicroAnalysisBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_MicroAnalysis_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var MicroAnalysis = new MicroAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            //Date = Convert.ToDateTime(reader["Date"]),
                            Source = reader["Source"].ToString(),
                            Date = Convert.ToDateTime(reader["DateTime"]),
                            WOPO = reader["WOPO"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            BatchNo = reader["BatchNo"].ToString(),
                            PackingSize = reader["PackingSize"].ToString(),
                            BestBeforeDate = Convert.ToDateTime(reader["BestBeforeDate"]),
                            ClostridiumPerfringens = reader["ClostridiumPerfringens"].ToString(),
                            EscherichiaColi = reader["EscherichiaColi"].ToString(),
                            Salmonella = reader["Salmonella"].ToString(),
                            TotalPlateCountNumber = reader["TotalPlateCountNumber"].ToString(),
                            //TotalPlateCountSpecial = reader["TotalPlateCountSpecial"].ToString(),
                            YeastandMould = reader["YeastMould"].ToString(),
                            Coliform = reader["Coliform"].ToString(), 
                            VerifyByName = reader["Verifiedby"].ToString(),
                            //Remark = reader["Remark"].ToString(),

                        };
                        MicroAnalysisList.Add(MicroAnalysis);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return MicroAnalysisList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(MicroAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_MicroAnalysis_Insert]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@Source", model.Source);
                    cmd.Parameters.AddWithValue("@WOPO", model.WOPO);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@BatchNo", model.BatchNo);
                    cmd.Parameters.AddWithValue("@PackingSize", model.PackingSize);
                    cmd.Parameters.AddWithValue("@BestBeforeDate", model.BestBeforeDate);
                    cmd.Parameters.AddWithValue("@ClostridiumPerfringens", model.ClostridiumPerfringens);
                    cmd.Parameters.AddWithValue("@EscherichiaColi", model.EscherichiaColi);
                    cmd.Parameters.AddWithValue("@Salmonella", model.Salmonella);
                    cmd.Parameters.AddWithValue("@TotalPlateCountNumber", model.TotalPlateCountNumber);
                    //cmd.Parameters.AddWithValue("@TotalPlateCountSpecial", model.TotalPlateCountSpecial);
                    cmd.Parameters.AddWithValue("@YeastandMould", model.YeastandMould);
                    cmd.Parameters.AddWithValue("@Coliform", model.Coliform);
                   
                    cmd.Parameters.AddWithValue("@VerifyByName", model.VerifyByName);
                  
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
        /// Date: 03/03/2023
        /// Maharshi: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MicroAnalysisBO GetById(int Id)
        {
            var MicroAnalysisBO = new MicroAnalysisBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_MicroAnalysis_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MicroAnalysisBO = new MicroAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Source = reader["Source"].ToString(),
                            Date = Convert.ToDateTime(reader["DateTime"]),
                            WOPO = reader["WOPO"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            BatchNo = reader["BatchNo"].ToString(),
                            PackingSize = reader["PackingSize"].ToString(),
                            BestBeforeDate = Convert.ToDateTime(reader["BestBeforeDate"]),
                            ClostridiumPerfringens = reader["ClostridiumPerfringens"].ToString(),
                            EscherichiaColi = reader["EscherichiaColi"].ToString(),
                            Salmonella = reader["Salmonella"].ToString(),
                            TotalPlateCountNumber = reader["TotalPlateCountNumber"].ToString(),
                            //TotalPlateCountSpecial = reader["TotalPlateCountSpecial"].ToString(),
                            YeastandMould = reader["YeastMould"].ToString(),
                            Coliform = reader["Coliform"].ToString(),
                            VerifyByName = reader["Verifiedby"].ToString(),
                            
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return MicroAnalysisBO;
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(MicroAnalysisBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_MicroAnalysis_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Source", model.Source);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@WOPO", model.WOPO);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@BatchNo", model.BatchNo);
                    cmd.Parameters.AddWithValue("@PackingSize", model.PackingSize);
                    cmd.Parameters.AddWithValue("@BestBeforeDate", model.BestBeforeDate);
                    cmd.Parameters.AddWithValue("@ClostridiumPerfringens", model.ClostridiumPerfringens);
                    cmd.Parameters.AddWithValue("@EscherichiaColi", model.EscherichiaColi);
                    cmd.Parameters.AddWithValue("@Salmonella", model.Salmonella);
                    cmd.Parameters.AddWithValue("@TotalPlateCountNumber", model.TotalPlateCountNumber);
                    //cmd.Parameters.AddWithValue("@TotalPlateCountSpecial", model.TotalPlateCountSpecial);
                    cmd.Parameters.AddWithValue("@YeastandMould", model.YeastandMould);
                    cmd.Parameters.AddWithValue("@Coliform", model.Coliform);
                  
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
        /// /// Date: 03/03/2023
        /// Maharshi: This function is for delete record of Daily Monitoring using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_MicroAnalysis_Delete]", con);
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

        #region  Bind grid for Report
        /// <summary>
        /// Date: 09 March'23
        /// Snehal: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        //public List<MicroAnalysisBO> GetAllMicroAnalysisList(DateTime? fromDate = null, DateTime? toDate = null)
        public List<MicroAnalysisBO> GetAllMicroAnalysisList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            
            List<MicroAnalysisBO> microanalysisList = new List<MicroAnalysisBO>();
            try
            {
                if (fromDate == null && toDate == null)
                {
                    fromDate = DateTime.Today;
                    toDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_MicroAnalysis_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var microAnalysis = new MicroAnalysisBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["DateTime"]),
                            Source = reader["Source"].ToString(),
                            WOPO = reader["WOPO"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            BatchNo = reader["BatchNo"].ToString(),
                            PackingSize = reader["PackingSize"].ToString(),
                            BestBeforeDate = Convert.ToDateTime(reader["BestBeforeDate"]),
                            ClostridiumPerfringens = reader["ClostridiumPerfringens"].ToString(),
                            EscherichiaColi = reader["EscherichiaColi"].ToString(),
                            Salmonella = reader["Salmonella"].ToString(),
                            TotalPlateCountNumber = reader["TotalPlateCountNumber"].ToString(),
                            YeastandMould = reader["YeastMould"].ToString(),
                            Coliform = reader["Coliform"].ToString(),
                            VerifyByName = reader["Verifiedby"].ToString(),
                           
                        };
                        microanalysisList.Add(microAnalysis);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return microanalysisList;
        }
        #endregion
    }
}