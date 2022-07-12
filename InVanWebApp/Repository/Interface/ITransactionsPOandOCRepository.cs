using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface ITransactionsPOandOCRepository
    {
        //Define function for fetching details of transaction.
        IEnumerable<PurchaseOrder> GetAll();

        //Define function for fetching details of OrderConfirmation master by ID.
        PurchaseOrder GetById(int PurchaseOrderId);

        //Function define for: Insert record for order confirmation.
        void Insert(PurchaseOrder orderConfirmation);

        //Function define for: Update master record for order confirmation.
        void Udate(PurchaseOrder orderConfirmation);

        //Function define for: View the transaction form.
        PurchaseOrder ViewTransactions(int PurchaseOrderId, int TransactionFlag);

        //Function define for: Delete record of item using it's ID
        void Delete(int PurchaseOrderID);

        //Function for fetching list of company.
        IEnumerable<Company> GetCompanyNameForDropDown();
        IEnumerable<Company> GetCompanyDetailsById(int Id);
        
        //For fetching the document no
        string GetDocumentNo(int DocumentType); //Logic is in SP: Document type=1 for Order Confirmation 
                                                //Document type=2 for Purchase Order
        //For fetching the list of items
        IEnumerable<Item> GetItemDetailsForDD(int ItemType);

        //Fetch the details of Item by it's ID
        Item GetItemDetails(int itemID);

        //This function is for fecthing list of order transaction for report.
        IEnumerable<PurchaseOrder> Report_OC_GetAll(DateTime FromDate, DateTime ToDate);
    }
}
