using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class TermsConditionRepository: ITermsConditionRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(TaxRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: This function is for fecthing list of terms and condition master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TermsAndConditionMasterBO> GetAll()
        {
            List<TermsAndConditionMasterBO> resultList = new List<TermsAndConditionMasterBO>();
            try
            {

                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new TermsAndConditionMasterBO()
                        {
                            SrNo=Convert.ToInt32(reader["SrNo"]),
                            TermsConditionID = Convert.ToInt32(reader["TermsAndConditionID"]),
                            TermName = reader["Terms"].ToString(),
                            TermDescription = reader["TermDescription"].ToString()
                        };
                        resultList.Add(result);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;            
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="tax"></param>
        public ResponseMessageBO Insert(TermsAndConditionMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TermName", model.TermName);
                    cmd.Parameters.AddWithValue("@TermDescription", model.TermDescription);
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
        /// Date: 27 Nov'22
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TermsAndConditionMasterBO GetById(int ID)
        {
            var result = new TermsAndConditionMasterBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new TermsAndConditionMasterBO()
                        {
                            TermsConditionID = Convert.ToInt32(reader["TermsConditionID"]),
                            TermName = reader["TermName"].ToString(),
                            TermDescription = reader["TermDescription"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// Date: 27 Nov'22
        /// Farheen: Update record
        /// </summary>
        /// <param name="tax"></param>
        public ResponseMessageBO Update(TermsAndConditionMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.TermsConditionID);
                    cmd.Parameters.AddWithValue("@TermName", model.TermName);
                    cmd.Parameters.AddWithValue("@TermDescription", model.TermDescription);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                return response;
            }
        }
        #endregion

        #region Delete function
        public void Delete(int Id, int LastModifiedId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", LastModifiedId);
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