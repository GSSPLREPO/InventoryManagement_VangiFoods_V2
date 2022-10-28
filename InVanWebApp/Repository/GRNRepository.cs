using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class GRNRepository: IGRNRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GRN_BO> GetAll()
        {
            List<GRN_BO> resultList = new List<GRN_BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GRN_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new GRN_BO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            GRNCode = reader["GRNCode"].ToString(),
                            InwardNoteNumber = reader["InwardNoteNumber"].ToString(),
                            GRNDate = Convert.ToDateTime(reader["GRNDate"]),
                            Remark = reader["Remark"].ToString()
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

    }
}