using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IStockAdjustmentRepository
    {
        IEnumerable<StockAdjustmentBO> GetAll();
        ResponseMessageBO Insert(StockAdjustmentBO model);
        void Delete(int Id, int userId);
        StockAdjustmentBO GetById(int ID);
    }
}
