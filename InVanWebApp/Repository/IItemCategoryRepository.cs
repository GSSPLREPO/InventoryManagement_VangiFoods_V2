using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IItemCategoryRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<ItemCategoryMaster> GetAll();

        //Define function for fetching details of Item master by ID.
        ItemCategoryMaster GetById(int ItemCategoryId);

        //Function define for: Insert record.
        void Insert(ItemCategoryMaster itemCategory);

        //Function define for: Update master record.
        void Udate(ItemCategoryMaster itemCategory);

        //Function define for: Delete record of item using it's ID
        void Delete(int ItemCategoryId);

        //Function for fetching list of item type.
        IEnumerable<ItemMaster> GetItemTypeForDropDown();
    }
}
