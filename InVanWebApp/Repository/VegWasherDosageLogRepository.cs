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
    public class VegWasherDosageLogRepository : IVegWasherDosageLogRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(VegWasherDosageLogRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 24 Feb'23
        /// Snehal: This function is for fecthing list of VegWasherDosageLog.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VegWasherDosageLogBO> GetAll()
        {
            List<VegWasherDosageLogBO> vegWasherDosageLogList = new List<VegWasherDosageLogBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_VegWasherDosageLog_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var vegWasherDosageLog = new VegWasherDosageLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //VegWasher1SolutionAMl = reader["VegWasher1SolutionAMl"].ToString(),
                            //VegWasher1SolutionBMl = reader["VegWasher1SolutionBMl"].ToString(),
                            VegWasher1SolutionAMl = Convert.ToDouble(reader["VegWasher1SolutionAMl"]),
                            VegWasher1SolutionBMl = Convert.ToDouble(reader["VegWasher1SolutionBMl"]),
                            NameOfItem1 = reader["NameOfItem1"].ToString(),
                            WashingTime1 = reader["WashingTime1"].ToString(),
                            Ppm1 = reader["Ppm1"].ToString(),
                            VegWasher2SolutionAMl = Convert.ToDouble(reader["VegWasher2SolutionAMl"]),
                            VegWasher2SolutionBMl = Convert.ToDouble(reader["VegWasher2SolutionBMl"]),
                            NameOfItem2 = reader["NameOfItem2"].ToString(),
                            WashingTime2 = reader["WashingTime2"].ToString(),
                            Ppm2 = reader["Ppm2"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString()
                        };
                        vegWasherDosageLogList.Add(vegWasherDosageLog);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return vegWasherDosageLogList;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 25 Feb'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(VegWasherDosageLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_VegWasherDosageLog_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@VegWasher1SolutionAMl", model.VegWasher1SolutionAMl);
                    cmd.Parameters.AddWithValue("@VegWasher1SolutionBMl", model.VegWasher1SolutionBMl);
                    cmd.Parameters.AddWithValue("@NameOfItem1", model.NameOfItem1);
                    cmd.Parameters.AddWithValue("@WashingTime1", model.WashingTime1);
                    cmd.Parameters.AddWithValue("@Ppm1", model.Ppm1);
                    cmd.Parameters.AddWithValue("@VegWasher2SolutionAMl", model.VegWasher2SolutionAMl);
                    cmd.Parameters.AddWithValue("@VegWasher2SolutionBMl", model.VegWasher2SolutionBMl);
                    cmd.Parameters.AddWithValue("@NameOfItem2", model.NameOfItem2);
                    cmd.Parameters.AddWithValue("@WashingTime2", model.WashingTime2);
                    cmd.Parameters.AddWithValue("@Ppm2", model.Ppm2);
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
        /// Date: 25 Feb'23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VegWasherDosageLogBO GetById(int Id)
        {
            var VegWasherDosageLogBO = new VegWasherDosageLogBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_VegWasherDosageLog_GetByID]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        VegWasherDosageLogBO = new VegWasherDosageLogBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //VegWasher1SolutionAMl = reader["VegWasher1SolutionAMl"].ToString(),
                            //VegWasher1SolutionBMl = reader["VegWasher1SolutionBMl"].ToString(),
                            VegWasher1SolutionAMl = Convert.ToDouble(reader["VegWasher1SolutionAMl"]),
                            VegWasher1SolutionBMl = Convert.ToDouble(reader["VegWasher1SolutionBMl"]),
                            NameOfItem1 = reader["NameOfItem1"].ToString(),
                            WashingTime1 = reader["WashingTime1"].ToString(),
                            Ppm1 = reader["Ppm1"].ToString(),
                            //VegWasher2SolutionAMl = reader["VegWasher2SolutionAMl"].ToString(),
                            //VegWasher2SolutionBMl = reader["VegWasher2SolutionBMl"].ToString(),

                            VegWasher2SolutionAMl = Convert.ToDouble(reader["VegWasher2SolutionAMl"]),
                            VegWasher2SolutionBMl = Convert.ToDouble(reader["VegWasher2SolutionBMl"]),
                            NameOfItem2 = reader["NameOfItem2"].ToString(),
                            WashingTime2 = reader["WashingTime2"].ToString(),
                            Ppm2 = reader["Ppm2"].ToString(),
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

            return VegWasherDosageLogBO;
        }

        /// <summary>
        /// Date: 25 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(VegWasherDosageLogBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_VegWasherDosageLog_Update]", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@VegWasher1SolutionAMl", model.VegWasher1SolutionAMl);
                    cmd.Parameters.AddWithValue("@VegWasher1SolutionBMl", model.VegWasher1SolutionBMl);
                    cmd.Parameters.AddWithValue("@NameOfItem1", model.NameOfItem1);
                    cmd.Parameters.AddWithValue("@WashingTime1", model.WashingTime1);
                    cmd.Parameters.AddWithValue("@Ppm1", model.Ppm1);
                    cmd.Parameters.AddWithValue("@VegWasher2SolutionAMl", model.VegWasher2SolutionAMl);
                    cmd.Parameters.AddWithValue("@VegWasher2SolutionBMl", model.VegWasher2SolutionBMl);
                    cmd.Parameters.AddWithValue("@NameOfItem2", model.NameOfItem2);
                    cmd.Parameters.AddWithValue("@WashingTime2", model.WashingTime2);
                    cmd.Parameters.AddWithValue("@Ppm2", model.Ppm2);
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
        /// /// Date: 25 Feb'23
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_VegWasherDosageLog_Delete]", con);
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