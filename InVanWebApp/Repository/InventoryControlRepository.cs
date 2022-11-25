using System;
using InVanWebApp_BO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using System.Configuration;
using System.Data.SqlClient;
using log4net;
using System.Data;

namespace InVanWebApp.Repository
{
    public class InventoryControlRepository : IInventoryControlRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(OrganisationRepository));
        public List<InventoryControlReportBO> GetAllInventoryControl()
        {
            List<InventoryControlReportBO> inventoryControlReports = new List<InventoryControlReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_InventoryControl", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new InventoryControlReportBO()
                        {
                            //ID = Convert.ToInt32(reader["Item_Id"]),
                            ItemName = reader["Item_Name"].ToString(),
                            ItemCode = reader["Item_Code"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString()),
                            PurchaseDate = Convert.ToDateTime(reader["PODate"]).ToString("dd/MM/yyyy"),
                            PurchaseQuantity = Convert.ToInt32(reader["PQuantity"]),                          
                            PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString()),
                            UsedDate = Convert.ToDateTime(reader["IssueNoteDate"]).ToString("dd/MM/yyyy"),
                            UsedQuantity=Convert.ToInt32(reader["QuantityIssued"]),
                            UsedPrice= Convert.ToDecimal(reader["UsedPrice"].ToString()),
                           AvailableQuantity= Convert.ToInt32(reader["StockQuantity"]),
                           AvailablePrice = Convert.ToDecimal(reader["AvailablePrice"].ToString())
                        };
                        inventoryControlReports.Add(result);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return inventoryControlReports;
        }
    }
}