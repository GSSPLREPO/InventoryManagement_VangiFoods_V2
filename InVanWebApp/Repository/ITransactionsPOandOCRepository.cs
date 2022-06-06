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

        //Define function for fetching details of Item master by ID.
        //Item GetById(int PurchaseOrderId);

        //Function define for: Insert record.
        void Insert(PurchaseOrder orderConfirmation);

        //Function define for: Update master record.
        //void Udate(PurchaseOrder purchaseOrder);

        //Function define for: Delete record of item using it's ID
       // void Delete(int Item_Id);

        //Function for fetching list of company.
        IEnumerable<Company> GetCompanyNameForDropDown();

    }
}
