using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;

namespace InVanWebApp.Repository
{
    public class DashboardRepository:IDashboardRepository
    {
        public List<StockTransferBO> GetDashboardData()
        {
            List<StockTransferBO> resultList = new List<StockTransferBO>();
            return resultList;
        }
    }
}