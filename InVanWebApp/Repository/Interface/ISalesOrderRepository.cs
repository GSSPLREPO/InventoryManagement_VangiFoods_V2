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
        IEnumerable<InquiryFormBO> GetInquiryList();

        List<InquiryFormItemDetailsBO> GetInquiryFormById(int id, int CurrencyId = 0);
    }
}
