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
        IEnumerable<InwardNoteBO> GetInwardDetailsById(int Id);

        //Insert function
        ResponseMessageBO Insert(GRN_BO model);

        //Delete function
        void Delete(int Id, int userId);

        //Function define for: Get GRN record by it's ID.
        GRN_BO GetById(int ID);

        //Function fetch the GRN details for View/pdf
        List<GRN_BO> GetGRNItemDetails(int Id);
    }
}
