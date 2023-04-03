using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ISILOCCPRepository
    {
        //Define function for fetching details of SILOCCPP.
        IEnumerable<SILOCCPBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(SILOCCPBO model);

        //Function define for: Delete record of SILOCCP using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of SILOCCP by Id.
        SILOCCPBO GetById(int Id);

        //Function define for: Update SILOCCP record.
        ResponseMessageBO Update(SILOCCPBO model);

    }
}
