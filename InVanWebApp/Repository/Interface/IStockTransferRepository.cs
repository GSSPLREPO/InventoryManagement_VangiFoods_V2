using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IStockTransferRepository
    {
        //Define function for fetching details of Purchase Order.
        IEnumerable<StockTransferBO> GetAll();
        //Function for Get Location Master  Location Name
        IEnumerable<StockTransferBO> GetFromLocationNameList();
        //Function for Get Location Master To Location Name
        //Function for Get Location Master  List  
        IEnumerable<LocationWiseStockBO> GetFromLocationMasterList(int id);   
        IEnumerable<StockTransferBO> GetToLocationNameList(); 
        //For fetching the list of items
        IEnumerable<ItemBO> GetItemDetailsForDD(int ItemType);
        //Fetch the details of Item and Location wise stock by it's ID
        ItemBO GetItemDetails(int itemID, int LocationId);   
        //Function define for: Insert record.
        ResponseMessageBO Insert(StockTransferBO stockTransferMaster);
        //Fetch the details of Item by it's ID 
        //StockTransferBO GetStockTransferById(int ID);  

    }
}
