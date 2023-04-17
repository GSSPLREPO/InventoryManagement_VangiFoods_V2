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
    public class PestControlLogRepository : IPestControlLogRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(PestControlLogRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 28 Feb'23
        /// Yatri: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PestControlLogBO> GetAll()
        {
            List<PestControlLogBO> pestControlLogList = new List<PestControlLogBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PestControlLog_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var pestControlLog = new PestControlLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = reader["Time"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            TypeOfPest = reader["TypeOfPest"].ToString(),
                            MethodForPestControl = reader["MethodForPestControl"].ToString(),
                            Area = reader["Area"].ToString(),
                            Frequncy = reader ["Frequncy"].ToString(),
                            COARecivedFromPestControl=reader["COARecivedFromPestControl"].ToString(),
                            EffectiveOrNot = reader["EffectiveOrNot"].ToString(),
                            AnyHazardDetectedAfterPest = reader["AnyHazardDetectedAfterPest"].ToString(),
                        };
                        pestControlLogList.Add(pestControlLog);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return pestControlLogList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 28 Feb'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(PestControlLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PestControlLog_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                   // cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@TypeOfPest", model.TypeOfPest);
                    cmd.Parameters.AddWithValue("@MethodForPestControl", model.MethodForPestControl);
                    cmd.Parameters.AddWithValue("@Area", model.Area);
                    cmd.Parameters.AddWithValue("@Frequncy", model.Frequncy);
                    cmd.Parameters.AddWithValue("@COARecivedFromPestControl", model.COARecivedFromPestControl);
                    cmd.Parameters.AddWithValue("@EffectiveOrNot", model.EffectiveOrNot);
                    cmd.Parameters.AddWithValue("@AnyHazardDetectedAfterPest", model.AnyHazardDetectedAfterPest);
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
        /// Date: 28 Feb'23
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PestControlLogBO GetById(int Id)
        {
            var PestControlLogBO = new PestControlLogBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PestControlLog_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PestControlLogBO = new PestControlLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = reader["Time"].ToString(),
                            TypeOfPest = reader["TypeOfPest"].ToString(),
                            MethodForPestControl = reader["MethodForPestControl"].ToString(),
                            Area = reader["Area"].ToString(),
                            Frequncy = reader["Frequncy"].ToString(),
                            COARecivedFromPestControl = reader["COARecivedFromPestControl"].ToString(),
                            EffectiveOrNot = reader["EffectiveOrNot"].ToString(),
                            AnyHazardDetectedAfterPest = reader["AnyHazardDetectedAfterPest"].ToString(),
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

            return PestControlLogBO;
        }

        /// <summary>
        /// Date: 28 Feb'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(PestControlLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PestControlLog_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    //cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@TypeOfPest", model.TypeOfPest);
                    cmd.Parameters.AddWithValue("@MethodForPestControl", model.MethodForPestControl);
                    cmd.Parameters.AddWithValue("@Area", model.Area);
                    cmd.Parameters.AddWithValue("@Frequncy", model.Frequncy);
                    cmd.Parameters.AddWithValue("@COARecivedFromPestControl", model.COARecivedFromPestControl);
                    cmd.Parameters.AddWithValue("@EffectiveOrNot", model.EffectiveOrNot);
                    cmd.Parameters.AddWithValue("@AnyHazardDetectedAfterPest", model.AnyHazardDetectedAfterPest);
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
        /// /// Date: 28 Feb'23
        /// Yatri: This function is for delete record of Daily Monitoring using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_PestControlLog_Delete]", con);
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
        /// Date: 14 April'23
        /// Yatri: This function is for fecthing list of PestControlLog.
        /// </summary>
        /// <returns></returns>
        public List<PestControlLogBO> GetAllPestCntrolLogList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            List<PestControlLogBO> PestControlLogList = new List<PestControlLogBO>();
            try
            {
                if (FromDate == null && ToDate == null)
                {
                    FromDate = DateTime.Today;
                    ToDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PestControlLog_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", FromDate);
                    cmd.Parameters.AddWithValue("@toDate", ToDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var PestControlLog = new PestControlLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TypeOfPest = reader["TypeOfPest"].ToString(),
                            MethodForPestControl = reader["MethodForPestControl"].ToString(),
                            Area = reader["Area"].ToString(),
                            Frequncy = reader["Frequncy"].ToString(),
                            COARecivedFromPestControl = reader["COARecivedFromPestControl"].ToString(),
                            EffectiveOrNot = reader["EffectiveOrNot"].ToString(),
                            AnyHazardDetectedAfterPest = reader["AnyHazardDetectedAfterPest"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        PestControlLogList.Add(PestControlLog);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return PestControlLogList;
        }
        #endregion

    }
}