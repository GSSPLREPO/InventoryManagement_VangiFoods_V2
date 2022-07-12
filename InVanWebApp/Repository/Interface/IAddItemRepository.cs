using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IAddItemRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<Item> GetAll();

        //Define function for fetching details of Item master by ID.
        Item GetById(int Item_ID);

        //Function define for: Insert record.
        void Insert(Item item);

        //Function define for: Update master record.
        void Udate(Item item);

        //Function define for: Delete record of item using it's ID
        void Delete(int Item_Id);

        //Function for fetching list of item category.
        IEnumerable<ItemCategoryMaster> GetItemCategoryForDropDown();

        //Function for fetching list of unit.
        IEnumerable<UnitMaster> GetUnitForDropdown();
    }
}
