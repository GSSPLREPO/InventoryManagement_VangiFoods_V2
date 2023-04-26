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
        //Function for for displaying NameOfEquipment and CalibrationDueDate in notification.
        List<CalibrationLogBO> GetCalibrationDueDateData();
        //Function for for displaying PurchaseOrderId and PaymentDueDate in notification.
        List<POPaymentBO> GetPOPaymentDueDateData();
        //Function for for displaying SalesOrderId and PaymentDueDate in notification.
        List<SOPaymentBO> GetSOPaymentDueDateData();


    }
}
