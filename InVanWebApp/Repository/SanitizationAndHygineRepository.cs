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
    public class SanitizationAndHygineRepository : ISanitizationAndHygineRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(SanitizationAndHygineRepository));

        #region  Bind grid
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SanitizationAndHygineBO> GetAll()
        {
            List<SanitizationAndHygineBO> SanitizationAndHygineList = new List<SanitizationAndHygineBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SanitizationAndHygine_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var SanitizationAndHygine = new SanitizationAndHygineBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            //Date = Convert.ToDateTime(reader["Date"]),
                            NameOfEmpolyee = reader["NameOfEmpolyee"].ToString(),
                            Department = reader["Department"].ToString(),
                            BodyTemperature = reader["BodyTemperature"].ToString(),

                            //Normal = reader["Normal"].ToString(),
                            //Modrate = reader["Modrate"].ToString(),
                            //High = reader["High"].ToString(),
                            HandWash = reader["HandWash"].ToString(),
                            CleanNails = reader["CleanNails"].ToString(),
                            CleanUniform = reader["CleanUniform"].ToString(),
                            AppearAnyCutsandWounds = reader["AppearAnyCutsandWounds"].ToString(),
                            WearAnyJwellery = reader["WearAnyJwellery"].ToString(),
                            FullyCoverdHair = reader["FullyCoverdHair"].ToString(),
                            CleanShoes = reader["CleanShoes"].ToString(),
                            NoTobacoChewingum = reader["NoTobacoChewingum"].ToString(),
                            AnyKindOfIllnessSeakness = reader["AnyKindOfIllnessSeakness"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),

                        };
                        SanitizationAndHygineList.Add(SanitizationAndHygine);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return SanitizationAndHygineList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03/03/2023
        /// Maharshi: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(SanitizationAndHygineBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_SanitizationAndHygine_Insert]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@NameOfEmpolyee", model.NameOfEmpolyee);
                    cmd.Parameters.AddWithValue("@Department", model.Department);
                    
                    cmd.Parameters.AddWithValue("@BodyTemperature", model.BodyTemperature);
                    //cmd.Parameters.AddWithValue("@Normal", model.Normal);
                    //cmd.Parameters.AddWithValue("@Modrate", model.Modrate);
                    //cmd.Parameters.AddWithValue("@High", model.High);
                    cmd.Parameters.AddWithValue("@HandWash", model.HandWash);
                    cmd.Parameters.AddWithValue("@CleanNails", model.CleanNails);
                    cmd.Parameters.AddWithValue("@CleanUniform", model.CleanUniform);
                    cmd.Parameters.AddWithValue("@AppearAnyCutsandWounds", model.AppearAnyCutsandWounds);
                    cmd.Parameters.AddWithValue("@WearAnyJwellery", model.WearAnyJwellery);
                    cmd.Parameters.AddWithValue("@FullyCoverdHair", model.FullyCoverdHair);
                    cmd.Parameters.AddWithValue("@CleanShoes", model.CleanShoes);
                    cmd.Parameters.AddWithValue("@NoTobacoChewingum", model.NoTobacoChewingum);
                    cmd.Parameters.AddWithValue("@AnyKindOfIllnessSeakness", model.AnyKindOfIllnessSeakness);
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
        /// Date: 03/03/2023
        /// Maharshi: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SanitizationAndHygineBO GetById(int Id)
        {
            var SanitizationAndHygineBO = new SanitizationAndHygineBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SanitizationAndHygine_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SanitizationAndHygineBO = new SanitizationAndHygineBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            //Date = Convert.ToDateTime(reader["Date"]),
                            NameOfEmpolyee= reader["NameOfEmpolyee"].ToString(),
                            Department = reader["Department"].ToString(),
                            BodyTemperature = reader["BodyTemperature"].ToString(),
                            //Normal = reader["Normal"].ToString(),
                            //Modrate = reader["Modrate"].ToString(),
                            //High = reader["High"].ToString(),
                            HandWash = reader["HandWash"].ToString(),
                            CleanNails = reader["CleanNails"].ToString(),
                            CleanUniform = reader["CleanUniform"].ToString(),
                            AppearAnyCutsandWounds = reader["AppearAnyCutsandWounds"].ToString(),
                            WearAnyJwellery = reader["WearAnyJwellery"].ToString(),
                            FullyCoverdHair = reader["FullyCoverdHair"].ToString(),
                            CleanShoes = reader["CleanShoes"].ToString(),
                            NoTobacoChewingum = reader["NoTobacoChewingum"].ToString(),
                            AnyKindOfIllnessSeakness = reader["AnyKindOfIllnessSeakness"].ToString(),
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

            return SanitizationAndHygineBO;
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(SanitizationAndHygineBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SanitizationAndHygine_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@NameOfEmpolyee", model.NameOfEmpolyee);
                    cmd.Parameters.AddWithValue("@Department", model.Department);
                   
                    cmd.Parameters.AddWithValue("@BodyTemperature", model.BodyTemperature);

                    //cmd.Parameters.AddWithValue("@Normal", model.Normal);
                    //cmd.Parameters.AddWithValue("@Modrate", model.Modrate);
                    //cmd.Parameters.AddWithValue("@High", model.High);
                    cmd.Parameters.AddWithValue("@HandWash", model.HandWash);
                    cmd.Parameters.AddWithValue("@CleanNails", model.CleanNails);
                    cmd.Parameters.AddWithValue("@CleanUniform", model.CleanUniform);
                    cmd.Parameters.AddWithValue("@AppearAnyCutsandWounds", model.AppearAnyCutsandWounds);
                    cmd.Parameters.AddWithValue("@WearAnyJwellery", model.WearAnyJwellery);
                    cmd.Parameters.AddWithValue("@FullyCoverdHair", model.FullyCoverdHair);
                    cmd.Parameters.AddWithValue("@CleanShoes", model.CleanShoes);
                    cmd.Parameters.AddWithValue("@NoTobacoChewingum", model.NoTobacoChewingum);
                    cmd.Parameters.AddWithValue("@AnyKindOfIllnessSeakness", model.AnyKindOfIllnessSeakness);
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_SanitizationAndHygine_Delete]", con);
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