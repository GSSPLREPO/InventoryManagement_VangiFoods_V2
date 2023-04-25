using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using log4net;
using InVanWebApp_BO;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class PostProductionRN_Repository:IPostProductionRN_Repository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(SalesOrderRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostProductionRejectionNoteBO> GetAll()
        {
            List<PostProductionRejectionNoteBO> resultList = new List<PostProductionRejectionNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProdRN_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new PostProductionRejectionNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            PostProdRejectionNoteNo = reader["PostProdRejectionNoteNo"].ToString(),
                            PostProdRejectionNoteDate = Convert.ToDateTime(reader["PostProdRejectionNoteDate"]),
                            PostProdRejectionType = reader["PostProdRejectionType"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            SO_No = reader["SO_No"].ToString()
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