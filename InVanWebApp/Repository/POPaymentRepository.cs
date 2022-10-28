using InVanWebApp.Repository.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InVanWebApp.Repository
{
    public class POPaymentRepository : IPOPaymentRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderRepository));


        #region Bind Grid
        /// <summary>
        /// Raj : This function is for fecthing list of PONumbers from Purchase Order.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetPONumbers()
        {
            Dictionary<int,string> PONumbers = new Dictionary<int, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        int PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]);
                        string PONumber = reader["PONumber"].ToString();
                        PONumbers.Add(PurchaseOrderId,PONumber);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return PONumbers;
        }
        #endregion
    }
}