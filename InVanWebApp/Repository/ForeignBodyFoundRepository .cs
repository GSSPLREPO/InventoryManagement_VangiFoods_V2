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
    public class ForeignBodyFoundRepository : IForeignBodyFoundRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(ForeignBodyFoundRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 17 March'23
        /// Yatri: This function is for fecthing list of ForeignBodyFound.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForeignBodyFoundBO> GetAll()
        {
            List<ForeignBodyFoundBO> ForeignBodyFoundList = new List<ForeignBodyFoundBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ForeignBodyFound_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var ForeignBodyFound = new ForeignBodyFoundBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            RawMaterial = reader["RawMaterial"].ToString(),                          
                            OnGoingProcessing = reader["OnGoingProcessing"].ToString(),
                            Batching = reader["Batching"].ToString(),
                            PostProcessing = reader["PostProcessing"].ToString(),
                            CorrectiveAction = reader["CorrectiveAction"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remarks"].ToString(),

                        };
                        ForeignBodyFoundList.Add(ForeignBodyFound);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ForeignBodyFoundList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 17 March'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(ForeignBodyFoundBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ForeignBodyFound_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@RawMaterial", model.RawMaterial);
                    cmd.Parameters.AddWithValue("@OnGoingProcessing", model.OnGoingProcessing);
                    cmd.Parameters.AddWithValue("@Batching", model.Batching);
                    cmd.Parameters.AddWithValue("@PostProcessing", model.PostProcessing);
                    cmd.Parameters.AddWithValue("@CorrectiveAction", model.CorrectiveAction);
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
        /// Date: 17 March'23
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ForeignBodyFoundBO GetById(int Id)
        {
            var ForeignBodyFoundBO = new ForeignBodyFoundBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ForeignBodyFound_GetByID", con);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ForeignBodyFoundBO = new ForeignBodyFoundBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            RawMaterial = reader["RawMaterial"].ToString(),
                            OnGoingProcessing = reader["OnGoingProcessing"].ToString(),
                            Batching = reader["Batching"].ToString(),
                            PostProcessing = reader["PostProcessing"].ToString(),
                            CorrectiveAction = reader["CorrectiveAction"].ToString(),
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

            return ForeignBodyFoundBO;
        }

        /// <summary>
        /// Date: 17 March'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(ForeignBodyFoundBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ForeignBodyFound_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@RawMaterial", model.RawMaterial);
                    cmd.Parameters.AddWithValue("@OnGoingProcessing", model.OnGoingProcessing);
                    cmd.Parameters.AddWithValue("@Batching", model.Batching);
                    cmd.Parameters.AddWithValue("@PostProcessing", model.PostProcessing);
                    cmd.Parameters.AddWithValue("@CorrectiveAction", model.CorrectiveAction);
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
        /// /// Date: 17 March'23
        /// Yatri: This function is for delete record of ForeignBodyFound using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_ForeignBodyFound_Delete]", con);
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
        /// Date: 17 March'23
        /// Yatri: This function is for fecthing list of ForeignBodyFound.
        /// </summary>
        /// <returns></returns>
        public List<ForeignBodyFoundBO> GetAllForeignBodyFoundList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            List<ForeignBodyFoundBO> ForeignBodyFoundList = new List<ForeignBodyFoundBO>();
            try
            {
                if (FromDate == null && ToDate == null)
                {
                    FromDate = DateTime.Today;
                    ToDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ForeignBodyFound_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", FromDate);
                    cmd.Parameters.AddWithValue("@toDate", ToDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var ForeignBodyFound = new ForeignBodyFoundBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            RawMaterial = reader["RawMaterial"].ToString(),
                            OnGoingProcessing = reader["OnGoingProcessing"].ToString(),
                            Batching = reader["Batching"].ToString(),
                            PostProcessing = reader["PostProcessing"].ToString(),
                            CorrectiveAction = reader["CorrectiveAction"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        ForeignBodyFoundList.Add(ForeignBodyFound);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ForeignBodyFoundList;
        }
        #endregion
    }
}