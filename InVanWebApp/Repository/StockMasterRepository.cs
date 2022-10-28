using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InVanWebApp.Repository
{
    public class StockMasterRepository : IStockMasterRepository
    {
        private string conStr = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        public List<StockReportBO> GetAllStock()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                var stockList = con.Query<StockReportBO>("usp_rpt_stock_report", commandType: System.Data.CommandType.StoredProcedure).ToList();
                return stockList;
            }
        }
    }
}