using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IItemRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<ItemMaster> GetAll();

        //Define function for fetching details of Item master by ID.
        ItemMaster GetById(int ItemId);

        //Function define for: Insert record.
        void Insert(ItemMaster itemMaster);

        //Function define for: Update master record.
        void Udate(ItemMaster itemMaster);

        //Function define for: Delete record of item using it's ID
        void Delete(int ItemId);

    }
}
