using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ITaxRepository
    {
        //Define function for fetching details of Location.
        IEnumerable<TaxBO> GetAll();

        //Define function for fetching details of location master by ID.
        TaxBO GetById(int ID);

        //Function define for: Insert record.
        void Insert(TaxBO taxMaster);

        //Function define for: Update master record.
        void Update(TaxBO taxMaster);

        //Function define for: Delete record of location using it's ID
        void Delete(int taxId);
    }
}
