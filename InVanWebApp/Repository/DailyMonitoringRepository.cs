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
    public class DailyMonitoringRepository : IDailyMonitoringRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(DailyMonitoringRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DailyMonitoringBO> GetAll()
        {
            List<DailyMonitoringBO> dailyMonitoringList  = new List<DailyMonitoringBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DailyMonitoring_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var dailyMonitoring = new DailyMonitoringBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            PersonalHygine = reader["PersonalHygine"].ToString(),
                            CleaningAndSanitation = reader["CleaningAndSanitation"].ToString(),
                            CleaningOfEquipment = reader["CleaningOfEquipment"].ToString(),
                            WaterPotability = reader["WaterPotability"].ToString(),
                            Allergic = reader["Allergic"].ToString(),
                            NonAllergic = reader["NonAllergic"].ToString(),
                            VegetableProcessingArea = reader["VegetableProcessingArea"].ToString(),
                            PackagingLabellingArea = reader["PackagingLabellingArea"].ToString(),
                            FgsArea = reader["FgsArea"].ToString(),
                            Inside = reader["Inside"].ToString(),
                            OutSide = reader["OutSide"].ToString(),
                            Dry = reader["Dry"].ToString(),
                            Wet = reader["Wet"].ToString(),
                            OutSiders = reader["OutSiders"].ToString(),
                            ProductionArea = reader["VerifyByName"].ToString(),
                            OfficeStaff = reader["OfficeStaff"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            
                        };
                        dailyMonitoringList.Add(dailyMonitoring);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return dailyMonitoringList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(DailyMonitoringBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DailyMonitoring_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@PersonalHygine", model.PersonalHygine);
                    cmd.Parameters.AddWithValue("@CleaningAndSanitation", model.CleaningAndSanitation);
                    cmd.Parameters.AddWithValue("@CleaningOfEquipment", model.CleaningOfEquipment);
                    cmd.Parameters.AddWithValue("@WaterPotability", model.WaterPotability);
                    cmd.Parameters.AddWithValue("@Allergic", model.Allergic);
                    cmd.Parameters.AddWithValue("@NonAllergic", model.NonAllergic);
                    cmd.Parameters.AddWithValue("@VegetableProcessingArea", model.VegetableProcessingArea);
                    cmd.Parameters.AddWithValue("@PackagingLabellingArea", model.PackagingLabellingArea);
                    cmd.Parameters.AddWithValue("@FgsArea", model.FgsArea);
                    cmd.Parameters.AddWithValue("@Inside", model.Inside);
                    cmd.Parameters.AddWithValue("@OutSide", model.OutSide);
                    cmd.Parameters.AddWithValue("@Dry", model.Dry);
                    cmd.Parameters.AddWithValue("@Wet", model.Wet);
                    cmd.Parameters.AddWithValue("@OutSiders", model.OutSiders);
                    cmd.Parameters.AddWithValue("@ProductionArea", model.ProductionArea);
                    cmd.Parameters.AddWithValue("@OfficeStaff", model.OfficeStaff);
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
        /// Date: 22 Feb'23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DailyMonitoringBO GetById(int Id)
        {
            var DailyMonitoringBO = new DailyMonitoringBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DailyMonitoring_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DailyMonitoringBO = new DailyMonitoringBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            PersonalHygine= reader["PersonalHygine"].ToString(),
                            CleaningAndSanitation = reader["CleaningAndSanitation"].ToString(),
                            CleaningOfEquipment = reader["CleaningOfEquipment"].ToString(),
                            WaterPotability = reader["WaterPotability"].ToString(),
                            Allergic = reader["Allergic"].ToString(),
                            NonAllergic = reader["NonAllergic"].ToString(),
                            VegetableProcessingArea = reader["VegetableProcessingArea"].ToString(),
                            PackagingLabellingArea = reader["PackagingLabellingArea"].ToString(),
                            FgsArea = reader["FgsArea"].ToString(),
                            Inside = reader["Inside"].ToString(),
                            OutSide = reader["OutSide"].ToString(),
                            Dry = reader["Dry"].ToString(),
                            Wet = reader["Wet"].ToString(),
                            OutSiders = reader["OutSiders"].ToString(),
                            ProductionArea = reader["ProductionArea"].ToString(),
                            OfficeStaff = reader["OfficeStaff"].ToString(),
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

            return DailyMonitoringBO;
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(DailyMonitoringBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DailyMonitoring_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@PersonalHygine", model.PersonalHygine);
                    cmd.Parameters.AddWithValue("@CleaningAndSanitation", model.CleaningAndSanitation);
                    cmd.Parameters.AddWithValue("@CleaningOfEquipment", model.CleaningOfEquipment);
                    cmd.Parameters.AddWithValue("@WaterPotability", model.WaterPotability);
                    cmd.Parameters.AddWithValue("@Allergic", model.Allergic);
                    cmd.Parameters.AddWithValue("@NonAllergic", model.NonAllergic);
                    cmd.Parameters.AddWithValue("@VegetableProcessingArea", model.VegetableProcessingArea);
                    cmd.Parameters.AddWithValue("@PackagingLabellingArea", model.PackagingLabellingArea);
                    cmd.Parameters.AddWithValue("@FgsArea", model.FgsArea);
                    cmd.Parameters.AddWithValue("@Inside", model.Inside);
                    cmd.Parameters.AddWithValue("@OutSide", model.OutSide);
                    cmd.Parameters.AddWithValue("@Dry", model.Dry);
                    cmd.Parameters.AddWithValue("@Wet", model.Wet);
                    cmd.Parameters.AddWithValue("@OutSiders", model.OutSiders);
                    cmd.Parameters.AddWithValue("@ProductionArea", model.ProductionArea);
                    cmd.Parameters.AddWithValue("@OfficeStaff", model.OfficeStaff);
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_DailyMonitoring_Delete]", con);
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
        /// Date: 09 March'23
        /// Snehal: This function is for fecthing list of Daily Monitoring.
        /// </summary>
        /// <returns></returns>
        //public List<DailyMonitoringBO> GetAllDailyMonitoringList(DateTime? fromDate = null, DateTime? toDate = null)
        public List<DailyMonitoringBO> GetAllDailyMonitoringList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null)
            {
            //SqlConnection con = new SqlConnection(conString);
            //DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("@flag", fromDate);
            //parameters.Add("@fromDate", fromDate);
            //parameters.Add("@toDate", toDate);

            //var dailyMonitoringList = con.Query<DailyMonitoringBO>("usp_tbl_DailyMonitoring_GetAllByDate", parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();

            //return dailyMonitoringList;


            List<DailyMonitoringBO> dailyMonitoringList = new List<DailyMonitoringBO>();
            try
            {
                if(fromDate==null && toDate==null)
                {
                    fromDate =DateTime.Today;
                    toDate = DateTime.Today;
                }
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DailyMonitoring_GetAllByDate", con);
                    cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var dailyMonitoring = new DailyMonitoringBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            PersonalHygine = reader["PersonalHygine"].ToString(),
                            CleaningAndSanitation = reader["CleaningAndSanitation"].ToString(),
                            CleaningOfEquipment = reader["CleaningOfEquipment"].ToString(),
                            WaterPotability = reader["WaterPotability"].ToString(),
                            Allergic = reader["Allergic"].ToString(),
                            NonAllergic = reader["NonAllergic"].ToString(),
                            VegetableProcessingArea = reader["VegetableProcessingArea"].ToString(),
                            PackagingLabellingArea = reader["PackagingLabellingArea"].ToString(),
                            FgsArea = reader["FgsArea"].ToString(),
                            Inside = reader["Inside"].ToString(),
                            OutSide = reader["OutSide"].ToString(),
                            Dry = reader["Dry"].ToString(),
                            Wet = reader["Wet"].ToString(),
                            OutSiders = reader["OutSiders"].ToString(),
                            ProductionArea = reader["ProductionArea"].ToString(),
                            OfficeStaff = reader["OfficeStaff"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        dailyMonitoringList.Add(dailyMonitoring);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return dailyMonitoringList;
        }
        #endregion
    }
}