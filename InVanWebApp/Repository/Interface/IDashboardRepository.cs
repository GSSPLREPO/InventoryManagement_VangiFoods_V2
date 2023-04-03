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
        List<ReportBO> GetProductionUtilityConsumptionByBatchDashboardData(DateTime fromDate, DateTime toDate, int BatchNumberId = 0, int WorkOrderNumberId = 0);
        List<ProductionMaterialIssueNoteBO> GetDashboardProductionCostByBatchNumber(int SOID,string BatchNumber = null, DateTime? fromDate = null, DateTime? toDate = null);
        List<ReportBO> GetDashboardUtilityConsumptionProduction(int SO_ID, DateTime? fromDate = null, DateTime? toDate = null);
        IEnumerable<ReportBO> GetAllWorkOrderNumber();
    }
}
