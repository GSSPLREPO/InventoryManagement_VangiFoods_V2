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
    public class RQCCPRepository : IRQCCPRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(RQCCPRepository));

        #region  Bind grid
        /// <summary>
        /// Charmi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RQCCPBO> GetAll()
        {
            List<RQCCPBO> RQCCPList = new List<RQCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var RQCCP = new RQCCPBO()
                        {
                            RQCCPID = Convert.ToInt32(reader["RQCCPID"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Activity = reader["Activity"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            NoBatches = reader["NOBatches"].ToString(),
                            BatchWeight = reader["BatchWeight"].ToString(),
                            MonitoringParameter = reader["MonitoringParameter"].ToString(),
                            BatchReleaseTimeOfRQ = reader["BatchReleaseTimeOfRQ"].ToString(),
                            MandatoryTemp = reader["MandatoryTemp"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            Responsibility = reader["Responsibility"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            Verification = reader["Verification"].ToString()
                        };
                        RQCCPList.Add(RQCCP);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                RQCCPList = null;
            }
            return RQCCPList;

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(RQCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Activity", model.Activity);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@NoBatches", model.NoBatches);
                    cmd.Parameters.AddWithValue("@Batchweight", model.BatchWeight);
                    cmd.Parameters.AddWithValue("@MonitoringParameter", model.MonitoringParameter);
                    cmd.Parameters.AddWithValue("@BatchReleaseTimeOfRQ", model.BatchReleaseTimeOfRQ);
                    cmd.Parameters.AddWithValue("@MandatoryTemp", model.MandatoryTemp);
                    cmd.Parameters.AddWithValue("@Frequency", model.Frequency);
                    cmd.Parameters.AddWithValue("@Responsibility", model.Responsibility);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@Verification", model.Verification);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                       // response.ItemName = dataReader["ItemName"].ToString();
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

        /// <summary>
        /// Date: 02 Sept 2022
        /// Farheen: This function is for uploading companies list (Bulk upload)
        /// </summary>
        /// <param name="model"></param>
        public List<ResponseMessageBO> SaveRQCCPData(List<RQCCPBO> model)
        {
            try
            {
                //var success = false;
                int cnt = 0;
                List<ResponseMessageBO> responsesList = new List<ResponseMessageBO>();
                if (model != null && model.Count > 0)
                {

                    for (int i = 0; i < model.Count; i++)
                    {
                        RQCCPBO material = new RQCCPBO();

                        material.Date = model[i].Date;
                        material.Activity = model[i].Activity;
                        material.ItemName = model[i].ItemName;
                        material.NoBatches = model[i].NoBatches;
                        material.BatchWeight = model[i].BatchWeight;
                        material.MonitoringParameter = model[i].MonitoringParameter;
                        material.BatchReleaseTimeOfRQ = model[i].BatchReleaseTimeOfRQ;
                        material.MandatoryTemp = model[i].MandatoryTemp;
                        material.Frequency = model[i].Frequency;
                        material.Responsibility = model[i].Responsibility;
                        material.Remarks = model[i].Remarks;
                        material.Verification = model[i].Verification;

                        material.IsDeleted = false;
                        material.CreatedBy = model[i].CreatedBy;
                        material.CreatedDate = Convert.ToDateTime(model[i].CreatedDate);

                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Insert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Date", material.Date);
                            cmd.Parameters.AddWithValue("@Activity", material.Activity);
                            cmd.Parameters.AddWithValue("@ItemName", material.ItemName);
                            cmd.Parameters.AddWithValue("@NoBatches", material.NoBatches);
                            cmd.Parameters.AddWithValue("@BatchWeight", material.BatchWeight);
                            cmd.Parameters.AddWithValue("@MonitoringParameter", material.MonitoringParameter);
                            cmd.Parameters.AddWithValue("@BatchReleaseTimeOfRQ", material.BatchReleaseTimeOfRQ);
                            cmd.Parameters.AddWithValue("@MandatoryTemp", material.MandatoryTemp);
                            cmd.Parameters.AddWithValue("@Frequency", material.Frequency);
                            cmd.Parameters.AddWithValue("@Responsibility", material.Responsibility);
                            cmd.Parameters.AddWithValue("@Remarks", material.Remarks);
                            cmd.Parameters.AddWithValue("@Verification", material.Verification);
                            cmd.Parameters.AddWithValue("@CreatedBy", 1);
                            cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                            con.Open();
                            SqlDataReader dataReader = cmd.ExecuteReader();


                            while (dataReader.Read())
                            {
                                ResponseMessageBO response = new ResponseMessageBO();

                                response.ItemName = dataReader["ItemName"].ToString();
                                response.Status = Convert.ToBoolean(dataReader["Status"]);

                                responsesList.Add(response);
                            }
                            con.Close();
                        };

                        //success = true;

                        cnt += 1;
                    }
                }
                //return success;
                return responsesList;
            }
            catch (Exception ex)
            {
                //dbContextTransaction.Dispose();
                throw ex;
            }
        }

        #endregion

        #region Update functions

        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RQCCPBO GetById(int RQCCPID)
        {
            var RQCCP = new RQCCPBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RQCCPID", RQCCPID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RQCCP = new RQCCPBO()
                        {
                            RQCCPID = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Activity = reader["Activity"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            NoBatches = reader["NoBatches"].ToString(),
                            BatchWeight = reader["BatchWeight"].ToString(),
                            MonitoringParameter = reader["MonitoringParameter"].ToString(),
                            BatchReleaseTimeOfRQ = reader["BatchReleaseTimeOfRQ"].ToString(),
                            MandatoryTemp = reader["MandatoryTemp"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            Responsibility = reader["Responsibility"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            Verification = reader["Verification"].ToString(),
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return RQCCP;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(RQCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RQCCPID", model.RQCCPID);
                    //cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@Activity", model.Activity);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@NoBatches", model.NoBatches);
                    cmd.Parameters.AddWithValue("@BatchWeight", model.BatchWeight);
                    cmd.Parameters.AddWithValue("@MonitoringParameter", model.MonitoringParameter);
                    cmd.Parameters.AddWithValue("@BatchReleaseTimeOfRQ", model.BatchReleaseTimeOfRQ);
                    cmd.Parameters.AddWithValue("@MandatoryTemp", model.MandatoryTemp);
                    cmd.Parameters.AddWithValue("@Frequency", model.Frequency);
                    cmd.Parameters.AddWithValue("@Responsibility", model.Responsibility);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@Verification", model.Verification);
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
        public void Delete(int RQCCPID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RQCCP_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RQCCPID", RQCCPID);
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