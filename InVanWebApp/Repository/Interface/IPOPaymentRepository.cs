using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository.Interface
{
    public interface IPOPaymentRepository
    {
        //Define function for fetching details of Purchase Order Number.
        Dictionary<int, string> GetPONumbers();

        //Define function for inserting data into Purchase Order Payment Details
        ResponseMessageBO Insert(POPaymentBO POPaymentDetails);

        //Define function for return the Purchase Order Details behalf of PurchaseOrderId
        PurchaseOrderBO GetPurchaseOrderById(int purchaseOrderId);

        //Define function for get the PurchaseOrder Items.
        List<PurchaseOrderItemsDetail> GetPOItemsByPurchaseOrderId(int purchaseOrderId);

        //Define function for get all PO Payments
        IEnumerable<POPaymentBO> GetAll();

        //Function detine for get Purchase Order Payment Details
        PurchaseOrderPaymentDetails GetPOPaymentDetailsById(int Id);

        //Function define for the update of Payment Details.
        ResponseMessageBO Update(POPaymentBO model);

        //Function define for the delete payment details
        void Delete(int ID, int userId);
    }
}
