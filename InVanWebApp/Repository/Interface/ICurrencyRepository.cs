using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;


namespace InVanWebApp.Repository
{
    public interface ICurrencyRepository
    {
        //Define function for fetching details of Currency.
        IEnumerable<CurrencyBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(CurrencyBO model);

        //Function define for: Delete record of Currency using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of Currency by Id.
        CurrencyBO GetById(int Id);

        //Function define for: Update Currency record.
        ResponseMessageBO Update(CurrencyBO model);
    }
}
