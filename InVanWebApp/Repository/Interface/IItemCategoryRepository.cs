using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using InVanWebApp.DAL;
using InVanWebApp_BO;

namespace InVanWebApp.Repository
{
    public interface IItemCategoryRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<ItemCategoryMasterBO> GetAll();

        //Define function for fetching details of Item master by ID.
        ItemCategoryMasterBO GetById(int ItemCategoryId);

        //Function define for: Insert record.
        bool Insert(ItemCategoryMasterBO itemCategory);

        //Function define for: Update master record.
        bool Udate(ItemCategoryMasterBO itemCategory);

        //Function define for: Delete record of item using it's ID
        void Delete(int ItemCategoryId);

        //Function for fetching list of item type.
        IEnumerable<ItemTypeBO> GetItemTypeForDropDown();
    }
}
