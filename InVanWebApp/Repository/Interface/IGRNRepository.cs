using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IGRNRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<GRN_BO> GetAll();

        //Bind Inward number dropdown
        IEnumerable<InwardNoteBO> GetInwardNumberForDropdown();

        //Get inward note details by ID.
        IEnumerable<InwardNoteBO> GetInwardDetailsById(int InwId);

        //Insert function
        ResponseMessageBO Insert(GRN_BO model);

    }
}
