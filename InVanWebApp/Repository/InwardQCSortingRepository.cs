using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class InwardQCSortingRepository:IInwardQCSortingRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(InwardQCSortingRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of inward QC data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InwardQCBO> GetAll()
        {
            List<InwardQCBO> resultList= new List<InwardQCBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardQCSorting_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new InwardQCBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardQCDate=Convert.ToDateTime(reader["InwardQCDate"]),
                            InwardQCNo = reader["InwardQCNo"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            InwardQuantity = Convert.ToInt32(reader["InwardQuantity"]),
                            RejectedQuantity = Convert.ToInt32(reader["RejectedQuantity"])
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