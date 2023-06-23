using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class StockMasterRepository : IStockMasterRepository
    {
        private string conStr = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());

        public IEnumerable<StockReportBO> GetAllStock(int ItemId = 0,int ItemCategoryNameID=0) 
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ItemId", ItemId);
                parameters.Add("@ItemCategoryNameID", ItemCategoryNameID);

                var stockList = con.Query<StockReportBO>("usp_rpt_stock_report",parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();

                return stockList;
            }
        }
    }
}