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
using Dapper;

namespace InVanWebApp.Repository
{
    public class ProductEvaluationLogRepository : IProductEvaluationLogRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(ProductEvaluationLogRepository));

        #region  Bind grid for datatable
        /// <summary>
        /// Date: 14 March'23
        /// Snehal: This function is for fecthing list of ProductEvaluationLog.
        /// </summary>
        /// <returns></returns>
        public List<ProductEvaluationLogBO> GetAllProductEvaluationLogList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<ProductEvaluationLogBO> productEvaluationLogList = new List<ProductEvaluationLogBO>();
            try
            {
                if (fromDate == null && toDate == null)
                {
                    fromDate = DateTime.Today;
                    toDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductEvaluationLog_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var productEvaluationLog = new ProductEvaluationLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PELDate = Convert.ToDateTime(reader["PELDate"]),
                            ProductName = reader["ProductName"].ToString(),
                            BatchCode = reader["BatchCode"].ToString(),
                            //Ph = reader["Ph"].ToString(),
                            Ph = Convert.ToDecimal(reader["Ph"]),
                            TexColTaste = reader["TexColTaste"].ToString(),
                            Acid = reader["Acid"].ToString(),
                            Salt = reader["Salt"].ToString(),
                            Viscosity = reader["Viscosity"].ToString(),
                            PELDateAfter7Days = (reader["PELDateAfter7Days"] is DBNull)? Convert.ToDateTime(reader["PELDate"]): Convert.ToDateTime(reader["PELDateAfter7Days"]),
                            PhAfter7Days = reader["PhAfter7Days"] is DBNull ? 0:Convert.ToDecimal(reader["PhAfter7Days"]),
                            TexColTasteAfter7Days = reader["TexColTasteAfter7Days"].ToString(),
                            AcidAfter7Days = reader["AcidAfter7Days"].ToString(),
                            SaltAfter7Days = reader["SaltAfter7Days"].ToString(),
                            ViscosityAfter7Days = reader["ViscosityAfter7Days"].ToString(),
                            WorkOrder = reader["WorkOrder"].ToString(),
                            Status = reader["Status"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        productEvaluationLogList.Add(productEvaluationLog);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return productEvaluationLogList;
        }
        #endregion
        #region Insert function
        /// <summary>
        /// Date: 14 March'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(ProductEvaluationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                model.Status = "Not Complete";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductEvaluationLog_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@PELDate", model.PELDate);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@BatchCode", model.BatchCode);
                    cmd.Parameters.AddWithValue("@Ph", model.Ph);
                    cmd.Parameters.AddWithValue("@TexColTaste", model.TexColTaste);
                    cmd.Parameters.AddWithValue("@Acid", model.Acid);
                    cmd.Parameters.AddWithValue("@Salt", model.Salt);
                    cmd.Parameters.AddWithValue("@Viscosity", model.Viscosity);
                    //cmd.Parameters.AddWithValue("@PELDateAfter7Days", model.PELDateAfter7Days);
                    //cmd.Parameters.AddWithValue("@PhAfter7Days", model.PhAfter7Days);
                    //cmd.Parameters.AddWithValue("@TexColTasteAfter7Days", model.TexColTasteAfter7Days);
                    //cmd.Parameters.AddWithValue("@AcidAfter7Days", model.AcidAfter7Days);
                    //cmd.Parameters.AddWithValue("@SaltAfter7Days", model.SaltAfter7Days);
                    //cmd.Parameters.AddWithValue("@ViscosityAfter7Days", model.ViscosityAfter7Days);
                    cmd.Parameters.AddWithValue("@WorkOrder", model.WorkOrder);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
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
        /// Date: 14 March'23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductEvaluationLogBO GetById(int Id)
        {
            var ProductEvaluationLogBO = new ProductEvaluationLogBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductEvaluationLog_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    //var DataReader = reader.
                    //reader["StudentID"] == DBNull.Value ? null : reader["StudentID"].ToString();
                    while (reader.Read())
                    {
                        if(reader["PELDateAfter7Days"] is DBNull)
                        {
                            ProductEvaluationLogBO = new ProductEvaluationLogBO()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PELDate = Convert.ToDateTime(reader["PELDate"]),
                                ProductName = reader["ProductName"].ToString(),
                                BatchCode = reader["BatchCode"].ToString(),
                                // Ph = reader["Ph"].ToString(),
                                Ph = reader["Ph"] is DBNull?0:Convert.ToDecimal(reader["Ph"]),
                                TexColTaste = reader["TexColTaste"].ToString(),
                                Acid = reader["Acid"].ToString(),
                                Salt = reader["Salt"].ToString(),
                                Viscosity = reader["Viscosity"].ToString(),
                                WorkOrder = reader["WorkOrder"].ToString(),
                                Status = reader["Status"].ToString(),
                                VerifyByName = reader["VerifyByName"].ToString(),
                                Remark = reader["Remark"].ToString(),
                                PELDateAfter7Days = Convert.ToDateTime(reader["PELDate"]).AddDays(7),
                            };
                        }
                        else
                        {
                            ProductEvaluationLogBO = new ProductEvaluationLogBO()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PELDate = Convert.ToDateTime(reader["PELDate"]),
                                ProductName = reader["ProductName"].ToString(),
                                BatchCode = reader["BatchCode"].ToString(),
                                //Ph = reader["Ph"].ToString(),
                                Ph = reader["Ph"] is DBNull ? 0 : Convert.ToDecimal(reader["Ph"]),
                                TexColTaste = reader["TexColTaste"].ToString(),
                                Acid = reader["Acid"].ToString(),
                                Salt = reader["Salt"].ToString(),
                                Viscosity = reader["Viscosity"].ToString(),
                                PELDateAfter7Days = Convert.ToDateTime(reader["PELDateAfter7Days"]),
                                //PhAfter7Days = reader["PhAfter7Days"].ToString(),
                                PhAfter7Days = reader["PhAfter7Days"] is DBNull ? 0 : Convert.ToDecimal(reader["PhAfter7Days"]),
                                TexColTasteAfter7Days = reader["TexColTasteAfter7Days"].ToString(),
                                AcidAfter7Days = reader["AcidAfter7Days"].ToString(),
                                SaltAfter7Days = reader["SaltAfter7Days"].ToString(),
                                ViscosityAfter7Days = reader["ViscosityAfter7Days"].ToString(),
                                WorkOrder = reader["WorkOrder"].ToString(),
                                Status = reader["Status"].ToString(),
                                VerifyByName = reader["VerifyByName"].ToString(),
                                Remark = reader["Remark"].ToString(),
                            };
                        }
                       
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return ProductEvaluationLogBO;
        }

        /// <summary>
        /// Date: 14 March'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(ProductEvaluationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            model.Status = "Complete";
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductEvaluationLog_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@PELDate", model.PELDate);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@BatchCode", model.BatchCode);
                    cmd.Parameters.AddWithValue("@Ph", model.Ph);
                    cmd.Parameters.AddWithValue("@TexColTaste", model.TexColTaste);
                    cmd.Parameters.AddWithValue("@Acid", model.Acid);
                    cmd.Parameters.AddWithValue("@Salt", model.Salt);
                    cmd.Parameters.AddWithValue("@Viscosity", model.Viscosity);
                    cmd.Parameters.AddWithValue("@PELDateAfter7Days", model.PELDateAfter7Days);
                    cmd.Parameters.AddWithValue("@PhAfter7Days", model.PhAfter7Days);
                    cmd.Parameters.AddWithValue("@TexColTasteAfter7Days", model.TexColTasteAfter7Days);
                    cmd.Parameters.AddWithValue("@AcidAfter7Days", model.AcidAfter7Days);
                    cmd.Parameters.AddWithValue("@SaltAfter7Days", model.SaltAfter7Days);
                    cmd.Parameters.AddWithValue("@ViscosityAfter7Days", model.ViscosityAfter7Days);
                    cmd.Parameters.AddWithValue("@WorkOrder", model.WorkOrder);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
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
        /// /// Date: 22 Feb'23
        /// Snehal: This function is for delete record of Daily Monitoring using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_ProductEvaluationLog_Delete]", con);
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