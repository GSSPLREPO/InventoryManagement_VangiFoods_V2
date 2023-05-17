using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly string conStr = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteRepository));
        public List<StockMovementBO> GetAllTransfferedStock(DateTime fromDate, DateTime toDate)
        {
            List<StockMovementBO> resultList = new List<StockMovementBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_StockMovement", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new StockMovementBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            Item_Code = reader["Item_Code"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            FromLocationName = reader["FromLocationName"].ToString(),
                            FromLocation_BeforeTransferQty = float.Parse(reader["FromLocation_BeforeTransferQty"].ToString()),
                            TransferQuantity = Convert.ToDouble(reader["TransferQuantity"].ToString()),
                            ValueOut = float.Parse(reader["ValueOut"].ToString()),
                            BalanceQty_FromLocation = Convert.ToInt32(reader["BalanceQty_FromLocation"]),
                            Action = reader["Action"].ToString(),
                            ToLocationName = reader["ToLocationName"].ToString(),
                            ToLocation_FinalQty = float.Parse(reader["ToLocation_FinalQty"].ToString()),
                            ValueIn = float.Parse(reader["ValueIn"].ToString()),
                            UnitPrice = float.Parse(reader["UnitPrice"].ToString()),
                            InwardDateOfItem = Convert.ToDateTime(reader["InwardDateOfItem"]).ToString("dd/MM/yyyy hh:mm:ss"),

                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }

    }
}