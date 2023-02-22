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
        List<StockMasterBO> GetReorderPointDashboardData();
    }
}
