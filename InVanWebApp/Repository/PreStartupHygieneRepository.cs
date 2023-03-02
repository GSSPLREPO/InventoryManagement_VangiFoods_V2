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
    public class PreStartupHygieneRepository : IPreStartupHygieneRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PreStartupHygieneRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 20 Feb'23
        /// Snehal: This function is for fecthing list of PreStartupHygiene.
        /// /// </summary>
        /// <returns></returns>
        public IEnumerable<PreStartupHygieneBO> GetAll()
        {
            List<PreStartupHygieneBO> preStartupHygieneList  = new List<PreStartupHygieneBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PreStartupHygieneCheck_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var preStartupHygiene= new PreStartupHygieneBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            VerifyBy = reader["VerifyBy"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Roboqubos = reader["Roboqubos"].ToString()
                        };
                        preStartupHygieneList.Add(preStartupHygiene);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return preStartupHygieneList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 Feb'23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(PreStartupHygieneBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PreStartUpHygineCheck_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VerifyBy", model.VerifyBy);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@RMRecevingArea", model.RMRecevingArea);
                    cmd.Parameters.AddWithValue("@CratesBlue", model.CratesBlue);
                    cmd.Parameters.AddWithValue("@CratesYellow", model.CratesYellow);
                    cmd.Parameters.AddWithValue("@CratesRed", model.CratesRed);
                    cmd.Parameters.AddWithValue("@WeightingArea", model.WeightingArea);
                    cmd.Parameters.AddWithValue("@Water", model.Water);
                    cmd.Parameters.AddWithValue("@HygineArea", model.HygineArea);
                    cmd.Parameters.AddWithValue("@RawMaterial", model.RawMaterial);
                    cmd.Parameters.AddWithValue("@FinishGoods", model.FinishGoods);
                    cmd.Parameters.AddWithValue("@WalkWay", model.WalkWay);
                    cmd.Parameters.AddWithValue("@VegetableWashingArea", model.VegetableWashingArea);
                    cmd.Parameters.AddWithValue("@PeelingMachine", model.PeelingMachine);
                    cmd.Parameters.AddWithValue("@ColdStorage", model.ColdStorage);
                    cmd.Parameters.AddWithValue("@Roboqubos", model.Roboqubos);
                    cmd.Parameters.AddWithValue("@Silo", model.Silo);
                    cmd.Parameters.AddWithValue("@PackagingLine", model.PackagingLine);
                    cmd.Parameters.AddWithValue("@Chiller", model.Chiller);
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
        public PreStartupHygieneBO GetById(int Id)
        {
            var preStartupHygieneBO = new PreStartupHygieneBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PreStartUpHygineCheck_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        preStartupHygieneBO = new PreStartupHygieneBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            VerifyBy = reader["VerifyBy"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            RMRecevingArea= reader["RMRecevingArea"].ToString(),
                            CratesBlue = reader["CratesBlue"].ToString(),
                            CratesYellow = reader["CratesYellow"].ToString(),
                            CratesRed = reader["CratesRed"].ToString(),
                            WeightingArea = reader["WeightingArea"].ToString(),
                            Water = reader["Water"].ToString(),
                            HygineArea = reader["HygineArea"].ToString(),
                            RawMaterial = reader["RawMaterial"].ToString(),
                            FinishGoods = reader["FinishGoods"].ToString(),
                            WalkWay = reader["WalkWay"].ToString(),
                            VegetableWashingArea = reader["VegetableWashingArea"].ToString(),
                            PeelingMachine = reader["PeelingMachine"].ToString(),
                            ColdStorage = reader["ColdStorage"].ToString(),
                            Roboqubos = reader["Roboqubos"].ToString(),
                            Silo = reader["Silo"].ToString(),
                            PackagingLine = reader["PackagingLine"].ToString(),
                            Chiller = reader["Chiller"].ToString(),
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

            return preStartupHygieneBO;
        }

        /// <summary>
        /// Date: 22 Feb'23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(PreStartupHygieneBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PreStartUpHygineCheck_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@VerifyBy", model.VerifyBy);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@RMRecevingArea", model.RMRecevingArea);
                    cmd.Parameters.AddWithValue("@CratesBlue", model.CratesBlue);
                    cmd.Parameters.AddWithValue("@CratesYellow", model.CratesYellow);
                    cmd.Parameters.AddWithValue("@CratesRed", model.CratesRed);
                    cmd.Parameters.AddWithValue("@WeightingArea", model.WeightingArea);
                    cmd.Parameters.AddWithValue("@Water", model.Water);
                    cmd.Parameters.AddWithValue("@HygineArea", model.HygineArea);
                    cmd.Parameters.AddWithValue("@RawMaterial", model.RawMaterial);
                    cmd.Parameters.AddWithValue("@FinishGoods", model.FinishGoods);
                    cmd.Parameters.AddWithValue("@WalkWay", model.WalkWay);
                    cmd.Parameters.AddWithValue("@VegetableWashingArea", model.VegetableWashingArea);
                    cmd.Parameters.AddWithValue("@PeelingMachine", model.PeelingMachine);
                    cmd.Parameters.AddWithValue("@ColdStorage", model.ColdStorage);
                    cmd.Parameters.AddWithValue("@Roboqubos", model.Roboqubos);
                    cmd.Parameters.AddWithValue("@Silo", model.Silo);
                    cmd.Parameters.AddWithValue("@PackagingLine", model.PackagingLine);
                    cmd.Parameters.AddWithValue("@Chiller", model.Chiller);
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
        /// /// Date: 20 Feb'23
        /// Snehal: This function is for delete record of PreHygiene using it's Id
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_PreStartupHygieneCheck_Delete", con);
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