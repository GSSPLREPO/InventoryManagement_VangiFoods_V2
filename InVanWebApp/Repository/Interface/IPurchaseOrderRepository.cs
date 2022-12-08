using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IPurchaseOrderRepository
    {
        //Define function for fetching details of Purchase Order.
        IEnumerable<PurchaseOrderBO> GetAll();
        //Define function for fetching details of company master by ID.
        PurchaseOrderBO GetById(int ID);
        //Define function for fetching details of Purchase Order by PurchaseOrderId.
        //PurchaseOrderBO GetById(int PurchaseOrderId);
        //Function define for: Insert record.
        //ResponseMessageBO Insert(PurchaseOrderBO purchaseOrderMaster, string json);
        ResponseMessageBO Insert(PurchaseOrderBO purchaseOrderMaster);

        //Function define for: Update master record.
        ResponseMessageBO Update(PurchaseOrderBO model);

        //Function for Get Company List dropdown 
        IEnumerable<PurchaseOrderBO> GetCompanyList();

        //Function for Get Company Address List dropdown 
        IEnumerable<PurchaseOrderBO> GetCompanyAddressList(int id); 

        //Function for Get Terms And Condition List dropdown 
        IEnumerable<PurchaseOrderBO> GetTermsAndConditionList();
         
        //Function for Get Organisations List  
        //IEnumerable<PurchaseOrderBO> GetOrganisationsList();

        //Function for Get Location Master  List  
        IEnumerable<PurchaseOrderBO> GetLocationMasterList(int id);
        //Function for Get Location Master  Location Name
        IEnumerable<PurchaseOrderBO> GetLocationNameList();

        //For fetching the list of items
        IEnumerable<ItemBO> GetItemDetailsForDD(int ItemType);
        //Fetch the details of Item by it's ID
        ItemBO GetItemDetails(int itemID, int currencyID);

        //Function define for: Delete record of item type using it's PurchaseOrderId 
        void Delete(int PurchaseOrderId, int userId);

        PurchaseOrderBO GetPurchaseOrderById(int PurchaseOrderId);

        //Function Define For: Save the Amendment details againts Purchase Order.
        ResponseMessageBO SaveAmendment(PurchaseOrderBO model);

        /// Function for timeline view of PO record by ID, Rahul 08/12/2022.
        PurchaseOrderBO GetDetailsForTimelineView(int PO_Id);
        IEnumerable<PurchaseOrderBO> GetCurrencyPriceList();

    }
}
