using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IChillerCCPRepository
    {
        //Define function for fetching details of ChillerCCP.
        IEnumerable<ChillerCCPBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(ChillerCCPBO model);

        //Function define for: Delete record of  ChillerCCP using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of  ChillerCCP by Id.
        ChillerCCPBO GetById(int Id);

        //Function define for: Update  ChillerCCP record.
        ResponseMessageBO Update(ChillerCCPBO model);

    }
}
