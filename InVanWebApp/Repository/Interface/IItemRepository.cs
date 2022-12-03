using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;
//using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IItemRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<ItemBO> GetAll();

        //Define function for fetching details of Item master by ID.
        ItemBO GetById(int Item_ID);

        //Function define for: Insert record.
        ResponseMessageBO Insert(ItemBO item);

        List<ResponseMessageBO> SaveItemData(List<ItemBO> items);

        //Function define for: Update master record.
        ResponseMessageBO Update(ItemBO item);

        //Function define for: Delete record of item using it's ID
        void Delete(int Item_Id, int userId);

        //Function for fetching list of item category.
        IEnumerable<ItemCategoryMasterBO> GetItemCategoryForDropDown();

        //Function for fetching list of unit.
        IEnumerable<ItemTypeBO> GetItemTypeForDropdown();
    }
}
