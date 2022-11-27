using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteRepository));
        public List<StockMovementBO> GetAllTransfferedStock()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    var stockList = con.Query<StockMovementBO>("usp_rpt_StockMovement", commandType: System.Data.CommandType.StoredProcedure).ToList();

                    return stockList;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                List<StockMovementBO> result = new List<StockMovementBO>();
                result = null;

                return result;
            }
        }

    }
}