using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IStockMasterRepository
    {
        //List<StockReportBO> GetAllStock();

        IEnumerable<StockReportBO> GetAllStock();
    }
}
