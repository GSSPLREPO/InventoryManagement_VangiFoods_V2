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
    public class ChillerCCPRepository : IChillerCCPRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(ChillerCCPRepository));
        #region  Bind grid
        /// <summary>
        /// Date: 21 March '23
        /// Yatri: This function is for fecthing list of ChillerCCP.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ChillerCCPBO> GetAll()
        {
            List<ChillerCCPBO> ChillerCCPList = new List<ChillerCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ChillerCCP_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var ChillerCCP = new ChillerCCPBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = reader["Time"].ToString(),
                            VerifyByName = reader["VerifyByName"].ToString(),
                            WaterClorinated = reader["WaterClorinated"].ToString(),
                            TotleTimeInChiller = reader["TotleTimeInChiller"].ToString(),
                            QuntityOfPakedProduct = reader["QuntityOfPakedProduct"].ToString(),
                            NoOfCrates = reader ["NoOfCrates"].ToString(),
                            MandatotyTemperature=reader["MandatotyTemperature"].ToString(),
                        };
                        ChillerCCPList.Add(ChillerCCP);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ChillerCCPList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 21 March'23
        /// Yatri: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(ChillerCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ChillerCCP_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                   // cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@WaterClorinated", model.WaterClorinated);
                    cmd.Parameters.AddWithValue("@TotleTimeInChiller", model.TotleTimeInChiller);
                    cmd.Parameters.AddWithValue("@QuntityOfPakedProduct", model.QuntityOfPakedProduct);
                    cmd.Parameters.AddWithValue("@NoOfCrates", model.NoOfCrates);
                    cmd.Parameters.AddWithValue("@MandatotyTemperature", model.MandatotyTemperature);
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
        /// Date: 21 March'23
        /// Yatri: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ChillerCCPBO GetById(int Id)
        {
            var ChillerCCPBO = new ChillerCCPBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ChillerCCP_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChillerCCPBO = new ChillerCCPBO()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            //Time = reader["Time"].ToString(),
                            WaterClorinated = reader["WaterClorinated"].ToString(),
                            TotleTimeInChiller = reader["TotleTimeInChiller"].ToString(),
                            QuntityOfPakedProduct = reader["QuntityOfPakedProduct"].ToString(),
                            NoOfCrates = reader["NoOfCrates"].ToString(),
                            MandatotyTemperature = reader["MandatotyTemperature"].ToString(),
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

            return ChillerCCPBO;
        }

        /// <summary>
        /// Date: 21 March'23
        /// Yatri: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(ChillerCCPBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ChillerCCP_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    //cmd.Parameters.AddWithValue("@Time", model.Time);
                    cmd.Parameters.AddWithValue("@WaterClorinated", model.WaterClorinated);
                    cmd.Parameters.AddWithValue("@TotleTimeInChiller", model.TotleTimeInChiller);
                    cmd.Parameters.AddWithValue("@QuntityOfPakedProduct", model.QuntityOfPakedProduct);
                    cmd.Parameters.AddWithValue("@NoOfCrates", model.NoOfCrates);
                    cmd.Parameters.AddWithValue("@MandatotyTemperature", model.MandatotyTemperature);
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
        /// /// Date: 21 March'23
        /// Yatri: This function is for delete record of ChillerCCP using it's Id
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
                    SqlCommand cmd = new SqlCommand("[usp_tbl_ChillerCCP_Delete]", con);
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