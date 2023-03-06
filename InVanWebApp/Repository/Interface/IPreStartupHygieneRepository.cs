using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IPreStartupHygieneRepository
    {
        //Define function for fetching details of PreHygiene .
        IEnumerable<PreStartupHygieneBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(PreStartupHygieneBO model);

        //Function define for: Delete record of PreHygiene using it's Id
        //void Delete(int Id);
        void Delete(int Id, int userId);

        //Define function for fetching details of PreHygiene  by Id.
        PreStartupHygieneBO GetById(int Id);

        //Function define for: Update PreHygiene record.
        ResponseMessageBO Update(PreStartupHygieneBO model);

    }
}
