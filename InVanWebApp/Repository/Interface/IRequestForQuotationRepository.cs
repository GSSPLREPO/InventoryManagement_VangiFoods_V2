using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IRequestForQuotationRepository
    {
        //Define function for fetching details of Request For Quotation.
        IEnumerable<RequestForQuotationBO> GetAll();
        //Function define for: Insert record. 
        ResponseMessageBO Insert(RequestForQuotationBO requestForQuotationMaster);

        //Fetch the details of Item by it's ID
        ItemBO GetItemDetails(int itemID);
        //Fetch the details of Item by it's RequestForQuotationId 
        RequestForQuotationBO GetRFQbyId(int RequestForQuotationId);
        //Function define for: Update master record.
        ResponseMessageBO Update(RequestForQuotationBO model); 
        //Fetch the details of RFQ Item by it's ID 
        RequestForQuotationItemDetailsBO GetDetailsForRFQView(int RequestForQuotationId);
        //Function define for: Delete record of item type using it's RequestForQuotationId 
        void Delete(int RequestForQuotationId, int userId); 
    }
}
