using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IRQCCPRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<RQCCPBO> GetAll();

        //Define function for fetching details of company master by ID.
        RQCCPBO GetById(int RQCCPID);

        //Function define for: Insert record.
        ResponseMessageBO Insert(RQCCPBO model);

        //Function define for: Update master record.
        ResponseMessageBO Update(RQCCPBO model);

        //Function define for: Delete record of company using it's ID
        void Delete(int RQCCPID, int userId);

        //Function define for: Uploading the bulk companies
        List<ResponseMessageBO> SaveRQCCPData(List<RQCCPBO> model);

    }
}
