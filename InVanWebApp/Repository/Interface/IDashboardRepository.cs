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

        List<DashboardBO> GetFIFOSystem(DateTime fromDate, DateTime toDate, int ItemId = 0, int LocationID = 0);
        List<DashboardBO> GetYeildSystem(DateTime fromDate, DateTime toDate, int BatchNumberID = 0, int WorkOrderNumberIDs = 0);

        //List<ReportBO> GetAll();
    }
}
