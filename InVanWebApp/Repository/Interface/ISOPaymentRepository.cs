using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository.Interface
{
    public interface ISOPaymentRepository 
    {
        //Define function for fetching details of SalesOrder Order Number.
        Dictionary<int, string> GetSONumbers(); 

        //Define function for inserting data into SalesOrder Order Payment Details
        ResponseMessageBO Insert(SOPaymentBO SOPaymentDetails);

        //Define function for return the SalesOrders Order Details behalf of SalesOrderId
        SalesOrderBO GetSalesOrderById(int salesOrderId); 

        //Define function for get the SalesOrder Items.
        List<SalesOrderItemsDetailBO> GetSOItemsBySalesOrderId(int purchaseOrderId);

        //Define function for get all SO Payments
        IEnumerable<SOPaymentBO> GetAll();

        //Function detine for get SalesOrder Order Payment Details        
        SalesOrderPaymentDetail GetSOPaymentDetailsById(int Id); 

        //Function define for the update of Payment Details.
        ResponseMessageBO Update(SOPaymentBO model);

        //Function define for the delete payment details
        void Delete(int ID, int userId);

        //Update functionality for Update 
        SOPaymentBO GetByID(int Id);
    }
}
