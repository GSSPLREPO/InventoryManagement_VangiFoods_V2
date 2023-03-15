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
    public class CalibrationLogRepository : ICalibrationLogRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(CalibrationLogRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 11 March'23
        /// Snehal: This function is for fecthing list of CalibrationLog.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalibrationLogBO> GetAll()
        {
            List<CalibrationLogBO> calibrationLogList = new List<CalibrationLogBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CalibrationLog_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var calibrationLog = new CalibrationLogBO()
                        {
                            Id = Convert.ToInt32(reader["SrNo"]),
                            NameOfEquipment = reader["NameOfEquipment"].ToString(),
                            IdNo = reader["IdNo"].ToString(),
                            Department = reader["Department"].ToString(),
                            Range = reader["Range"].ToString(),
                            RangeFrom = reader["RangeFrom"].ToString(),
                            RangeTo = reader["RangeTo"].ToString(),
                            FrequencyOfCalibration = reader["FrequencyOfCalibration"].ToString(),
                            CalibrationDoneDate=Convert.ToDateTime(reader["CalibrationDoneDate"]),
                            CalibrationDueDate=Convert.ToDateTime(reader["CalibrationDueDate"]),
                            Remark = reader["Remark"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString()
                        };
                        calibrationLogList.Add(calibrationLog);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return calibrationLogList;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 11 March'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(CalibrationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CalibrationLog_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NameOfEquipment", model.NameOfEquipment);
                    cmd.Parameters.AddWithValue("@IdNo", model.IdNo);
                    cmd.Parameters.AddWithValue("@Department", model.Department);
                    cmd.Parameters.AddWithValue("@Range", model.Range);
                    cmd.Parameters.AddWithValue("@RangeFrom", model.RangeFrom);
                    cmd.Parameters.AddWithValue("@RangeTo", model.RangeTo);
                    cmd.Parameters.AddWithValue("@FrequencyOfCalibration", model.FrequencyOfCalibration);
                    cmd.Parameters.AddWithValue("@CalibrationDoneDate", model.CalibrationDoneDate);
                    cmd.Parameters.AddWithValue("@CalibrationDueDate", model.CalibrationDueDate);
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
        /// Date: 11 March'23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CalibrationLogBO GetById(int Id)
        {
            var CalibrationLogBO = new CalibrationLogBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_CalibrationLog_GetByID]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CalibrationLogBO = new CalibrationLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NameOfEquipment = reader["NameOfEquipment"].ToString(),
                            IdNo = reader["IdNo"].ToString(),
                            Department = reader["Department"].ToString(),
                            Range = reader["Range"].ToString(),
                            RangeFrom = reader["RangeFrom"].ToString(),
                            RangeTo = reader["RangeTo"].ToString(),
                            FrequencyOfCalibration = reader["FrequencyOfCalibration"].ToString(),
                            CalibrationDoneDate = Convert.ToDateTime(reader["CalibrationDoneDate"]),
                            CalibrationDueDate = Convert.ToDateTime(reader["CalibrationDueDate"]),
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

            return CalibrationLogBO;
        }

        /// <summary>
        /// Date: 11 March'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(CalibrationLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_CalibrationLog_Update]", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@NameOfEquipment", model.NameOfEquipment);
                    cmd.Parameters.AddWithValue("@IdNo", model.IdNo);
                    cmd.Parameters.AddWithValue("@Department", model.Department);
                    cmd.Parameters.AddWithValue("@Range", model.Range);
                    cmd.Parameters.AddWithValue("@RangeFrom", model.RangeFrom);
                    cmd.Parameters.AddWithValue("@RangeTo", model.RangeTo);
                    cmd.Parameters.AddWithValue("@FrequencyOfCalibration", model.FrequencyOfCalibration);
                    cmd.Parameters.AddWithValue("@CalibrationDoneDate", model.CalibrationDoneDate);
                    cmd.Parameters.AddWithValue("@CalibrationDueDate", model.CalibrationDueDate);
                    cmd.Parameters.AddWithValue("@VerifyByName", model.VerifyByName);
                    cmd.Parameters.AddWithValue("@Remark", model.Remark);
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
        /// /// Date: 11 March'23
        /// Snehal: This function is for delete record of Calibration Log using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_CalibrationLog_Delete]", con);
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