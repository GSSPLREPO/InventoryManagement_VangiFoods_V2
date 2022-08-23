using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;
//using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IItemTypeRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<ItemTypeBO> GetAll();

        //Define function for fetching details of Item type by ID.
        ItemTypeBO GetById(int ItemTypeId);

        //Function define for: Insert record.
        bool Insert(ItemTypeBO itemTypeMaster);

        //Function define for: Update master record.
        bool Udate(ItemTypeBO itemTypeMaster);

        //Function define for: Delete record of item type using it's ID
        void Delete(int ItemTypeId);

    }
}
