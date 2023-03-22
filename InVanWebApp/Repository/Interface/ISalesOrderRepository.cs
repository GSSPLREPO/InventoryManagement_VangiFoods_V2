using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ISalesOrderRepository
    {
        //Define function for fetching details of Sales Order.
        IEnumerable<SalesOrderBO> GetAll();
        ResponseMessageBO Insert(SalesOrderBO model);
        SalesOrderBO GetSalesOrderById(int Id);
        ResponseMessageBO Update(SalesOrderBO model);
        //Function define for: Delete record of item type using it's sales order id 
        void Delete(int ID, int userId);
        IEnumerable<InquiryFormBO> GetInquiryList();
        List<InquiryFormItemDetailsBO> GetInquiryFormById(int id, int CurrencyId = 0);
    }
}
