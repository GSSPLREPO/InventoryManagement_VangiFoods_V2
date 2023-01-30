using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IDeliveryChallanRepository
    {
        IEnumerable<DeliveryChallanBO> GetAll();
        ResponseMessageBO Insert(DeliveryChallanBO model);
        IEnumerable<SalesOrderBO> GetSONumberList();
        IEnumerable<SalesOrderBO> GetSODetailsById(int SOId);
        ResponseMessageBO Delete(int Id, int userId);
        DeliveryChallanBO GetById(int ID);
    }
}
