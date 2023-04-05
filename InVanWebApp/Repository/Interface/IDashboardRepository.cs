using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IDashboardRepository
    {
        List<LocationWiseStockBO> GetDashboardData(int id, int ItemId=0);
        List<StockMasterBO> GetReorderPointDashboardData(int ItemId=0);
        List<ReportBO> GetYeildDashboardData(DateTime fromDate, DateTime toDate, string BatchNumberId = "0", int WorkOrderNumberId = 0);
        List<DashboardBO> GetFIFOSystem(DateTime fromDate, DateTime toDate, int ItemId = 0, int LocationID = 0);
        List<ProductionMaterialIssueNoteBO> GetDashboardProductionCostByBatchNumber(int SOID, string BatchNumber = null, DateTime? fromDate = null, DateTime? toDate = null);
        List<OrderSummaryBO> GetOrderSummaryDashboardData(DateTime fromDate, DateTime toDate, int DurationID = 0);
        List<DashboardBO> GetWorkOrderwiseProductionCost(DateTime FromDate, DateTime ToDate, int SalesOrderId = 0);
        //Function For Production Utility Consumption By Batch Wise Data.    
        List<ReportBO> GetProductionUtilityConsumptionByBatchDashboardData(DateTime fromDate, DateTime toDate, string BatchNumber = "0", string WorkOrderNumber = "0");
        List<ReportBO> GetDashboardUtilityConsumptionProduction(int SO_ID, DateTime? fromDate = null, DateTime? toDate = null);
        IEnumerable<ReportBO> GetAllWorkOrderNumber();
        List<LocationWiseStockBO> GetTotalInventoryValue();

    }
}
