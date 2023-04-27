using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface INotificationsRepository
    {
        //Function for reorder point On MinStock of available total stock
        List<StockMasterBO> GetReorderPointOnMinStock(int ItemId = 0);


    }
}
