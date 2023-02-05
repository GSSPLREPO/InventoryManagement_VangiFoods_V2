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
        //Function define for: Insert record into  RFQ Supplier Details.  
        ResponseMessageBO InsertRFQSupplierDetails(RFQ_VendorDetailsBO rfqSupplierDetailsMater); 

        //Fetch the details of Item by it's ID
        ItemBO GetItemDetails(int itemID);
        //Fetch the details of Item by it's RequestForQuotationId and Vendor details  by it's VendorsID 
        RequestForQuotationBO GetRFQbyId(int RequestForQuotationId, int VendorsID);
        //Function define for: Update master record.
        ResponseMessageBO Update(RequestForQuotationBO model);        

        //Fetch the details of RFQ Item by it's ID 
        RequestForQuotationBO GetDetailsForRFQView(int RequestForQuotationId);
        IEnumerable<RequestForQuotationBO> GetCompanyNameForRFQView(int ID);        
        
        //Function define for: Delete record of item type using it's RequestForQuotationId 
        void Delete(int RequestForQuotationId, int userId); 

        //Added the below function for setting the item details in the puch quotation view 
        RFQ_VendorDetailsBO ItemDetailsVendorWise(int RFQ_ID, int ID = 0, int VenColNo = 0);
        ResponseMessageBO InsertPO(RFQ_VendorDetailsBO model);
    }
}
